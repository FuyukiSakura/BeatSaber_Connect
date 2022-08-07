using System.Net.WebSockets;
using System.Text;

namespace BeatSaber_FakeMultiplay.Client.Services
{
    /// <summary>
    /// A event based implementation of <see cref="ClientWebSocket"/>
    /// </summary>
    public class WebSocket
    {
        readonly string _socketUri;

        CancellationTokenSource _cancellationSource = new ();
        ClientWebSocket _ws = new ();

        public event EventHandler<string>? MessageReceived;
        public event EventHandler<string?>? Closed;

        /// <summary>
        /// Creates a new instance of <see cref="WebSocket"/>
        /// </summary>
        /// <param name="uri"></param>
        public WebSocket(string uri)
        {
            _socketUri = uri;
        }

        /// <summary>
        /// Starts the socket
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            // Cancel existing request
            _cancellationSource.Cancel();
            _cancellationSource = new CancellationTokenSource();

            _ws = new ClientWebSocket();
            await _ws.ConnectAsync(new Uri(_socketUri), _cancellationSource.Token);

            _ = ListenAsync();
            _ = KeepAliveAsync();
        }

        /// <summary>
        /// Keeps pinging the server to keep the connection alive
        /// </summary>
        /// <returns></returns>
        async Task KeepAliveAsync()
        {
            while (!_cancellationSource.IsCancellationRequested)
            {
                var buffer = Encoding.ASCII.GetBytes("ping");
                await _ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
                await Task.Delay (5000);
            }
        }

        /// <summary>
        /// Listens to incoming event returned by http-status
        /// </summary>
        /// <returns></returns>
        public async Task ListenAsync()
        {
            while(!_cancellationSource.IsCancellationRequested)
            {
                if (_ws.State != WebSocketState.Open)
                {
                    // Reconnects if the socket has errors
                    Closed?.Invoke(this, _ws.CloseStatusDescription);
                }

                var message = await ReceiveAsync();
                MessageReceived?.Invoke(this, message);
            }
        }

        /// <summary>
        /// Slices the message into smaller chunks and write them into buffer
        /// return when the full message is received
        /// </summary>
        /// <remarks>
        /// This is due to limitation of <see cref="System.Net.WebSockets.WebSocket"/> not receiving
        /// the full message when it is too long
        /// </remarks>
        /// <returns>Socket message</returns>
        async Task<string> ReceiveAsync()
        {
            var ms = new MemoryStream();
            WebSocketReceiveResult result;
            do
            {
                var messageBuffer = System.Net.WebSockets.WebSocket.CreateClientBuffer(1024, 16);
                result = await _ws.ReceiveAsync(messageBuffer, CancellationToken.None);
                ms.Write(messageBuffer.Array!, messageBuffer.Offset, result.Count);
            }
            while (!result.EndOfMessage);

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        /// <summary>
        /// Tries to reconnect to the socket
        /// </summary>
        /// <returns></returns>
        public async Task ReconnectAsync()
        {
            try
            {
                await ConnectAsync();
            }
            catch (WebSocketException)
            {
                // Try reconnects every second
                await Task.Delay(1000);
                await ReconnectAsync();
            }
        }

        /// <summary>
        /// Stops listening to the http-status socket
        /// </summary>
        public void Close()
        {
            _cancellationSource.Cancel();
        }
    }
}
