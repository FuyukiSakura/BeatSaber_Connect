
namespace BeatSaber_FakeMultiplay.Shared.Models.BeatSaber
{
    /// <summary>
    /// Represents the statistics of the current game play of Beat Saber Lounge
    /// </summary>
    public class PlayerStats
    {
        /// <summary>
        /// Gets or sets the score of the current game play
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the rank of the current game play
        /// </summary>
        public string Rank { get; set; } = "SSS";

        /// <summary>
        /// Gets or sets the accuracy of the current game play
        /// </summary>
        public float Accuracy { get; set; } = float.MinValue;
    }
}
