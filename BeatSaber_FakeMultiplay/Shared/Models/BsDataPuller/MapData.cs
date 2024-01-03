
namespace BeatSaber_FakeMultiplay.Shared.Models.BsDataPuller
{
    /// <summary>
    /// Represents the current map info from data puller
    /// 
    /// Pasted from https://github.com/ReadieFur/BSDataPuller
    /// </summary>
    public class MapData
    {
        public string GameVersion { get; set; } = "";
        public string PluginVersion { get; set; } = "";
        public bool InLevel { get; set; }
        public bool LevelPaused { get; set; }
        public bool LevelFinished { get; set; }
        public bool LevelFailed { get; set; }
        public bool LevelQuit { get; set; }
        public string Hash { get; set; } = "";
        public string SongName { get; set; } = "";
        public string SongSubName { get; set; } = "";
        public string SongAuthor { get; set; } = "";
        public string Mapper { get; set; } = "";
        public string BSRKey { get; set; } = "";
        public string CoverImage { get; set; } = "";
        public int Length { get; set; }
        public float TimeScale { get; set; }
        public string MapType { get; set; } = "";
        public string Difficulty { get; set; } = "";
        public string CustomDifficultyLabel { get; set; } = "";
        public int BPM { get; set; } = 0;
        public float NJS { get; set; } = 0;
        public Modifiers Modifiers { get; set; } = new();
        public float ModifiersMultiplier { get; set; } = 0;
        public bool PracticeMode { get; set; }
        public PracticeModeModifiers PracticeModeModifiers { get; set; } = new();
        public float PP { get; set; } = 0f;
        public float Star { get; set; } = 0f;
        public bool IsMultiplayer { get; set; }
        public int PreviousRecord { get; set; } = 0;
        public string PreviousBSR { get; set; }
        public long unixTimestamp { get; set; }
    }

    public class Modifiers
    {
        public bool instaFail { get; set; }
        public bool batteryEnergy { get; set; }
        public bool disappearingArrows { get; set; }
        public bool ghostNotes { get; set; }
        public bool fasterSong { get; set; }
        public bool noFail { get; set; }
        public bool noObstacles { get; set; }
        public bool noBombs { get; set; }
        public bool slowerSong { get; set; }
        public bool noArrows { get; set; }
    }

    public class PracticeModeModifiers
    {
        public float songSpeedMul { get; set; }
    }
}
