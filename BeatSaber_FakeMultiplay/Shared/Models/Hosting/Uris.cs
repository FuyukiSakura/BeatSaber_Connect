
namespace BeatSaber_FakeMultiplay.Shared.Models.Hosting
{
    /// <summary>
    /// List of server endpoints
    /// </summary>
    public static class Uris
    {
        /// <summary>
        /// Gets the address to the SignalR Hub
        /// </summary>
        #if DEBUG
        public const string Hub = "https://localhost:7218";
        #else
        public const string Hub = "https://bs-multiplay.azurewebsites.net";
        #endif
    }
}
