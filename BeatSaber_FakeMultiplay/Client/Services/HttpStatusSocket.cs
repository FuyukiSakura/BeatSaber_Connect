using System.Net.WebSockets;
using System.Text;
using BeatSaber_FakeMultiplay.Shared.Models.Event;

namespace BeatSaber_FakeMultiplay.Client.Services
{
    /// <summary>
    /// Connects to http-status socket
    /// </summary>
    public class HttpStatusSocket
    {
        CancellationTokenSource _cancellationSource = new ();
        ClientWebSocket _httpStatusConnection = new ();

        public event EventHandler<Performance>? ScoreChanged;
        public event EventHandler? SongStart;

        /// <summary>
        /// Starts the http-status socket
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            // Cancel existing request
            _cancellationSource.Cancel();

            _httpStatusConnection = new ClientWebSocket();
            await _httpStatusConnection.ConnectAsync(new Uri("ws://127.0.0.1:6557/socket"), CancellationToken.None);

            _cancellationSource = new CancellationTokenSource();
            _ = ListenAsync();
        }

        /// <summary>
        /// Stops listening to the http-status socket
        /// </summary>
        public void Stop()
        {
            _cancellationSource.Cancel();
        }

        /// <summary>
        /// Tries to reconnect to the socket
        /// </summary>
        /// <returns></returns>
        async Task ReconnectAsync()
        {
            try
            {
                _httpStatusConnection = new ClientWebSocket();
                await _httpStatusConnection.ConnectAsync(new Uri("ws://127.0.0.1:6557/socket"), CancellationToken.None);
            }
            catch (WebSocketException)
            {
                // Try reconnects every second
                await Task.Delay(1000);
                await ReconnectAsync();
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
                if (_httpStatusConnection.State != WebSocketState.Open)
                {
                    // Reconnects if the socket has errors
                    await ReconnectAsync();
                }

                var socketEvent = await ReceiveAsync();
                if (socketEvent == null) continue; // Cannot parse result, listen for next event
                
                switch (socketEvent.Event)
                {
                    case EventType.Hello:
                    case EventType.ScoreChanged:
                        if (socketEvent.Status.Performance == null) break;
                        ScoreChanged?.Invoke(this, socketEvent.Status.Performance);
                        break;
                    case EventType.SongStart:
                        SongStart?.Invoke(this, EventArgs.Empty);
                        break;
                }
            }
        }

        /// <summary>
        /// Receives update from Beat Saber HttpStatus
        /// and broadcast it to other players
        /// </summary>
        /// <returns>Socket event data</returns>
        async Task<SocketEvent?> ReceiveAsync()
        {
            var ms = new MemoryStream();
            WebSocketReceiveResult result;
            do
            {
                var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
                result = await _httpStatusConnection.ReceiveAsync(messageBuffer, CancellationToken.None);
                ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);
            }
            while (!result.EndOfMessage);

            var httpStatusJson = Encoding.UTF8.GetString(ms.ToArray());
            return HttpStatusJson.Serialize(httpStatusJson);
        }
    }
}
