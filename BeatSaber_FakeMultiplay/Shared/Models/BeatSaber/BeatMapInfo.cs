
namespace BeatSaber_FakeMultiplay.Shared.Models.BeatSaber
{
    /// <summary>
    /// Layouts the beatmap information
    /// </summary>
    public class BeatMapInfo
    {
        /// <summary>
        /// Gets or sets the artist name of the song
        /// </summary>
        public string Artist { get; set; } = "Artist";

        /// <summary>
        /// Gets or sets the songs BPM
        /// </summary>
        public int BPM { get; set; } = 0;

        /// <summary>
        /// Gets or sets the key for BSR of the beat map
        /// </summary>
        public string BSR { get; set; } = "00000";

        /// <summary>
        /// Gets or sets the cover image data string
        /// </summary>
        public string CoverImage { get; set; } = "";

        /// <summary>
        /// Gets or sets the custom difficulty label of current game play
        /// specified by the mapper
        /// </summary>
        public string CustomDifficulty { get; set; } = "Custom Difficulty";

        /// <summary>
        /// Gets or sets the difficulty of the current game play
        /// </summary>
        public string Difficulty { get; set; } = "Easy";

        /// <summary>
        /// Gets or sets the mapper name of the beat map
        /// </summary>
        public string Mapper { get; set; } = "Mapper";

        /// <summary>
        /// Gets or sets the NJS of the current difficulty
        /// </summary>
        public float NJS { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the performance point of the beat map
        /// </summary>
        public float PP { get; set; } = 0f;

        /// <summary>
        /// Gets or sets the start difficulty of the beat map
        /// </summary>
        public float Star { get; set; } = 0f;
        
        /// <summary>
        /// Gets or sets the song name of the beat map
        /// </summary>
        public string SongName { get; set; } = "Name";

        /// / <summary>
        /// Gets or sets the alternative song name of the beat map
        /// </summary>
        public string SongSubName { get; set; } = "Subtitle";
    }
}
