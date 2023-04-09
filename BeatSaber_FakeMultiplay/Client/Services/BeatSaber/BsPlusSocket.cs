using BeatSaber_FakeMultiplay.Shared.Models.BeatSaber;
using BeatSaber_FakeMultiplay.Shared.Models.BSPlus;

namespace BeatSaber_FakeMultiplay.Client.Services.BeatSaber
{
    /// <summary>
    /// Connects to http-status socket
    /// </summary>
    public class BsPlusSocket : IBeatSaberSocket
    {
        bool _previousInLevel;
        bool _failed;

        const string Url = "ws://localhost:2947/socket";
        const string GameStatePlaying = "Playing";

        WebSocket _httpStatusConnection = new (Url);

        public event EventHandler<PlayerStats>? ScoreChanged;
        public event EventHandler<BeatMapInfo?>? SongStart;
        public event EventHandler<BeatMapInfo>? SongUpdate;
        public event EventHandler<SongQuitEventArgs>? SongQuit;
        public event EventHandler? Failed;

        /// <summary>
        /// Starts the http-status socket
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            _httpStatusConnection = new WebSocket(Url);
            _httpStatusConnection.MessageReceived += HttpStatusConnection_OnMessageReceived;
            _httpStatusConnection.Closed += HttpStatusConnection_OnClosed;
            await _httpStatusConnection.ConnectAsync();
        }

        /// <summary>
        /// Handles http-status socket connection closed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        async void HttpStatusConnection_OnClosed(object? sender, string? e)
        {
            await _httpStatusConnection.ReconnectAsync();
        }

        /// <summary>
        /// Handles http-status socket message when received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HttpStatusConnection_OnMessageReceived(object? sender, string e)
        {
            var msg = BsPlusMessage.Serialize(e);
            if (msg == null) return; // Cannot parse result, listen for next event
                
            switch (msg.Event)
            {
                case "gameState":
                    InvokeSongStartStop(msg);
                    _previousInLevel = msg.GameStateChanged == GameStatePlaying;
                    break;
                case "mapInfo":
                    var beatmapInfo = new BeatMapInfo
                    {
                        Artist = msg.MapInfoChanged.Artist,
                        BPM = (int) msg.MapInfoChanged.BPM,
                        BSR = msg.MapInfoChanged.BSRKey,
                        CoverImage = "data:image/gif;base64," + msg.MapInfoChanged.CoverRaw,
                        Difficulty = msg.MapInfoChanged.Characteristic,
                        CustomDifficulty = msg.MapInfoChanged.Difficulty,
                        Mapper = msg.MapInfoChanged.Mapper,
                        PP = msg.MapInfoChanged.PP,
                        SongName = msg.MapInfoChanged.Name,
                        SongSubName = msg.MapInfoChanged.SubName,
                    };
                    SongUpdate?.Invoke(this, beatmapInfo);
                    break;
                case "score":
                    var stats = new PlayerStats
                    {
                        Combo = msg.ScoreEvent.Combo,
                        Energy = msg.ScoreEvent.CurrentHealth,
                        Misses = msg.ScoreEvent.MissCount,
                        TimeElapsed = (int) msg.ScoreEvent.Time,
                        Score = msg.ScoreEvent.Score,
                        Accuracy = msg.ScoreEvent.Accuracy * 100
                    };
                    ScoreChanged?.Invoke(this, stats);
                    break;
            }
        }

        /// <summary>
        /// Checks if a user enters/quits a level and invoke the corresponding
        /// <see cref="SongStart"/> or <see cref="SongQuit"/> events
        /// </summary>
        /// <param name="msg">The original event received from BS+ overlay</param>
        void InvokeSongStartStop(BsPlusEvent msg)
        {
            if (msg.GameStateChanged == GameStatePlaying == _previousInLevel)
            {
                // Do not trigger event if the state is the same
                return;
            }

            if (msg.GameStateChanged == GameStatePlaying != _previousInLevel)
            {
                _failed = false;
                SongStart?.Invoke(this, null);
            }
            else
            {
                SongQuit?.Invoke(this, new SongQuitEventArgs
                {
                    Scene = _failed ? QuitStatus.Fail : QuitStatus.Finish
                });
            }
        }

        /// <summary>
        /// Stops listening to the bs+ overlay socket
        /// </summary>
        public void Stop()
        {
            _httpStatusConnection.Close();
        }
    }
}
