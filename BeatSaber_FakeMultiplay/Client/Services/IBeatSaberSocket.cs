using BeatSaber_FakeMultiplay.Shared.Models.BeatSaber;

namespace BeatSaber_FakeMultiplay.Client.Services
{
    public interface IBeatSaberSocket
    {
        /// <summary>
        /// Emits when a score change is detected
        /// </summary>
        event EventHandler<PlayerStats>? ScoreChanged;

        /// <summary>
        /// Emits when a song has started
        /// </summary>
        event EventHandler? SongStart;

        /// <summary>
        /// Emits when a user has failed
        /// </summary>
        event EventHandler? Failed;

        /// <summary>
        /// Starts the socket connection to the corresponding plugin
        /// </summary>
        /// <returns></returns>
        Task StartAsync();

        /// <summary>
        /// Stops listening to the socket
        /// </summary>
        void Stop();
    }
}
