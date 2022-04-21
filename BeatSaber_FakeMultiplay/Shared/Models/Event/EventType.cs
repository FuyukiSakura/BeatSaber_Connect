
namespace BeatSaber_FakeMultiplay.Shared.Models.Event
{
    /// <summary>
    /// Represents http-status socket events
    /// </summary>
    public enum EventType
    {
        Hello,
        SongStart,
        Finished,
        SoftFailed,
        Failed,
        Menu,
        Pause,
        Resume,
        NoteSpawned,
        NoteCut,
        NoteFullyCut,
        NoteMissed,
        BombCut,
        BombMissed,
        ObstacleEnter,
        ObstacleExit,
        ScoreChanged,
        BeatmapEvent
    }
}
