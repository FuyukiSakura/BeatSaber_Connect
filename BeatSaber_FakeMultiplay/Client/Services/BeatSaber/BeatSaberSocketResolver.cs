using System.Net.WebSockets;

namespace BeatSaber_FakeMultiplay.Client.Services.BeatSaber
{
    /// <summary>
    /// Detects which socket connection will be used for score information
    /// </summary>
    public class BeatSaberSocketResolver
    {
        /// <summary>
        /// Gets or sets the successfully resolved socket
        /// </summary>
        IBeatSaberSocket? _beatSaberSocket;

        public readonly IEnumerable<IBeatSaberSocket> Sockets;

        /// <summary>
        /// Creates a new instance of <see cref="BeatSaberSocketResolver"/>
        /// </summary>
        /// <param name="sockets"></param>
        public BeatSaberSocketResolver(IEnumerable<IBeatSaberSocket> sockets)
        {
            Sockets = sockets;
        }

        /// <summary>
        /// Gets the first beat saber socket implementation that connects successfully
        /// </summary>
        /// <returns></returns>
        public async Task<IBeatSaberSocket?> ResolveAsync()
        {
            if (_beatSaberSocket != null)
            {
                return _beatSaberSocket;
            }

            foreach (var socket in Sockets)
            {
                try
                {
                    await socket.StartAsync();
                    _beatSaberSocket = socket;
                    return socket;
                }
                catch (WebSocketException)
                {
                    // connection failed
                }
            }

            // None of the socket can connect, return null
            return null;
        }
    }
}
