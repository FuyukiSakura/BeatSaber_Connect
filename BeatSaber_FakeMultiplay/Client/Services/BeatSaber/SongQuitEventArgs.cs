using BeatSaber_FakeMultiplay.Shared.Models.BeatSaber;

namespace BeatSaber_FakeMultiplay.Client.Services.BeatSaber
{
    /// <summary>
    /// Is sent when a <see cref="IBeatSaberSocket.SongQuit"/>
    /// </summary>
    public class SongQuitEventArgs
    {
        /// <summary>
        /// The song info of the map quit
        /// </summary>
        public BeatMapInfo BeatMapInfo { get; set; } = new();

        /// <summary>
        /// The status of the game play when user quits the map
        /// </summary>
        public string Scene { get; set; } = "";
    }

    /// <summary>
    /// The status of the game play when user quits a map
    /// </summary>
    public static class QuitStatus 
    {
        /// <summary>
        /// User quits with full combo
        /// </summary>
        public const string FullCombo = "BS-FullCombo";

        /// <summary>
        /// User clears the map
        /// </summary>
        public const string Finish = "BS-Finish";

        /// <summary>
        /// User failed a map
        /// </summary>
        public const string Fail = "BS-Fail";

        /// <summary>
        /// This is only a pause
        /// </summary>
        public const string Pause = "BS-Pause";
    }
}
