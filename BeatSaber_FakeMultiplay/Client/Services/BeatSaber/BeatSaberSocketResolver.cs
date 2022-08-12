using System.Net.WebSockets;

namespace BeatSaber_FakeMultiplay.Client.Services.BeatSaber
{
    /// <summary>
    /// Detects which socket connection will be used for score information
    /// </summary>
    public class BeatSaberSocketResolver
    {
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
            foreach (var socket in Sockets)
            {
                try
                {
                    await socket.StartAsync();
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
