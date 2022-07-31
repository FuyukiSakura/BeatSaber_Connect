
namespace BeatSaber_FakeMultiplay.Shared.Models.BeatSaber
{
    /// <summary>
    /// Represents the statistics of the current game play of Beat Saber Lounge
    /// </summary>
    public class PlayerStats
    {
        /// <summary>
        /// Gets or sets the current combo of the game play
        /// </summary>
        public int Combo { get; set; }

        /// <summary>
        /// Gets or sets the current energy level of the game play in percentage
        /// </summary>
        public float Energy { get; set; } = 50f;

        /// <summary>
        /// Gets or sets the number of missed notes of the current game play
        /// </summary>
        public int Misses { get; set; }

        /// <summary>
        /// Gets or sets the score of the current game play
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the current time of the song in seconds
        /// </summary>
        public int TimeElapsed { get; set; }

        /// <summary>
        /// Gets or sets the rank of the current game play
        /// </summary>
        public string Rank { get; set; } = "SSS";

        /// <summary>
        /// Gets or sets the accuracy of the current game play in percentage
        /// </summary>
        public float Accuracy { get; set; } = 0.0f;
    }
}
