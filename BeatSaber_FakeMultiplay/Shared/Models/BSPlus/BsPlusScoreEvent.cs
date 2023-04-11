namespace BeatSaber_FakeMultiplay.Shared.Models.BSPlus
{
    /// <summary>
    /// Represents BS+ overlay score event json structure options
    /// </summary>
    public class BsPlusScoreEvent
    {
        /// <summary>
        /// The current time of the song
        /// </summary>
        public float Time { get; set; } = 0f;

        /// <summary>
        /// The current score of the player
        /// </summary>
        public int Score { get; set; } = 0;

        /// <summary>
        /// The current accuracy of the player
        /// </summary>
        public float Accuracy { get; set; } = 0f;

        /// <summary>
        /// The current combo of the player
        /// </summary>
        public int Combo { get; set; } = 0;

        /// <summary>
        /// The current miss count of the player
        /// </summary>
        public int MissCount { get; set; } = 0;

        /// <summary>
        /// The current energy of the player
        /// </summary>
        public float CurrentHealth { get; set; } = 0f;
    }
}
