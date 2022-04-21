
namespace BeatSaber_FakeMultiplay.Shared.Models.Socket
{
    /// <summary>
    /// List of strings of events sent from BsHub server
    /// </summary>
    public static class BsHubMessage
    {
        /// <summary>
        /// Gets the message name of the pub/sub event of player's current score details
        /// </summary>
        public const string ScoreUpdate = "ScoreUpdated";
    }
}
