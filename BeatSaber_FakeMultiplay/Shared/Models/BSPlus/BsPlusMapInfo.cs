
using System.Text.Json.Serialization;

namespace BeatSaber_FakeMultiplay.Shared.Models.BSPlus
{
    /// <summary>
    /// Map information from BS+ socket
    /// </summary>
    public class BsPlusMapInfo
    {
        /// <summary>
        /// The displayed name of the song
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// The sub-displayed name of the song
        /// </summary>
        [JsonPropertyName("sub_name")]
        public string SubName { get; set; } = "";

        /// <summary>
        /// The artist of the song
        /// </summary>
        public string Artist { get; set; } = "Artist";

        /// <summary>
        /// The mapper of the map
        /// </summary>
        public string Mapper { get; set; } = "Mapper";

        /// <summary>
        /// The in game difficulty of the song
        /// </summary>
        public string Characteristic { get; set; } = "Standard";

        /// <summary>
        /// The displayed difficulty of the song
        /// </summary>
        public string Difficulty { get; set; } = "Easy";

        /// <summary>
        /// The duration of the song in seconds
        /// </summary>
        public int Duration { get; set; } = 0;

        /// <summary>
        /// The BPM of the song
        /// </summary>
        [JsonPropertyName("BPM")]
        public float BPM { get; set; } = 0f;

        /// <summary>
        /// The performance point of this map
        /// </summary>
        [JsonPropertyName("PP")]
        public float PP { get; set; } = 0f;

        /// <summary>
        /// The BSR key of this map
        /// </summary>
        [JsonPropertyName("BSRKey")]
        public string BSRKey { get; set; } = "00000";

        /// <summary>
        /// The cover image of this map
        /// </summary>
        public string CoverRaw { get; set; } = "";
    }
}
