using System.Text.Json.Serialization;

namespace BeatSaber_FakeMultiplay.Shared.Models.BSPlus
{
    /// <summary>
    /// An event from BS+ socket
    /// </summary>
    public class BsPlusEvent
    {
        /// <summary>
        /// The event type from BS+
        /// </summary>
        [JsonPropertyName("_event")]
        public string Event { get; set; } = "";
        
        /// <summary>
        /// The current location of the game Menu/Playing
        /// </summary>
        public string GameStateChanged { get; set; } = "";

        /// <summary>
        /// The time that the song resumes after a pause
        /// </summary>
        public float ResumeTime { get; set; } = 0f;

        /// <summary>
        /// The time that the song pauses
        /// </summary>
        public float PauseTime { get; set; } = 0f;

        /// <summary>
        /// Is included when map info changes
        /// </summary>
        public BsPlusMapInfo MapInfoChanged { get; set; } = new();

        /// <summary>
        /// Is included when score event occurs
        /// </summary>
        public BsPlusScoreEvent ScoreEvent { get; set; } = new();
    }
}
