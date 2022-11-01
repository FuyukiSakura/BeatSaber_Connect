using BeatSaber_FakeMultiplay.Shared.Models;
using BeatSaber_FakeMultiplay.Shared.Models.BeatSaber;
using BeatSaber_FakeMultiplay.Shared.Models.BsDataPuller;

namespace BeatSaber_FakeMultiplay.Client.Services.BeatSaber
{
    /// <summary>
    /// Connects to BsDataPuller socket
    /// </summary>
    public class DataPullerSocket : IBeatSaberSocket
    {
        const string DataPullerUrl = "ws://127.0.0.1:2946/BSDataPuller/";
        const string MapDataUrl = DataPullerUrl + "MapData";
        const string LiveDataUrl = DataPullerUrl + "LiveData";

        bool _previousInLevel;
        int _previousMisses;
        public event EventHandler<PlayerStats>? ScoreChanged;
        public event EventHandler<BeatMapInfo>? SongStart;
        public event EventHandler<BeatMapInfo>? SongUpdate;
        public event EventHandler<SongQuitEventArgs>? SongQuit;
        public event EventHandler? Missed;
        public event EventHandler? Failed;

        readonly WebSocket _mapDataWs = new (MapDataUrl);
        readonly WebSocket _liveDataWs = new (LiveDataUrl);

        /// <summary>
        /// Creates a new instance of <see cref="DataPullerSocket" />
        /// </summary>
        public DataPullerSocket()
        {
            _mapDataWs.MessageReceived += MapDataWsOnMessageReceived;
            _mapDataWs.Closed += WebSocket_OnClosed;

            _liveDataWs.MessageReceived += LiveDataWs_OnMessageReceived;
            _liveDataWs.Closed += WebSocket_OnClosed;
        }

        ///
        /// <inheritdoc />
        ///
        public async Task StartAsync()
        {
            await _mapDataWs.ConnectAsync();
            await _liveDataWs.ConnectAsync();
        }

        /// <summary>
        /// Handles on map data received from data puller event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MapDataWsOnMessageReceived(object? sender, string e)
        {
            var mapData = NullableJsonSerializer.Deserialize<MapData>(e);
            if (mapData == null) return; // Cannot parse result, ignore

            var beatmapInfo = new BeatMapInfo
            {
                Artist = mapData.SongAuthor,
                BPM = mapData.BPM,
                BSR = mapData.BSRKey,
                CoverImage = mapData.coverImage,
                Difficulty = mapData.Difficulty,
                CustomDifficulty = mapData.CustomDifficultyLabel,
                Mapper = mapData.Mapper,
                NJS = mapData.NJS,
                PP = mapData.PP,
                Star = mapData.Star,
                SongSubName = mapData.SongSubName,
                SongName = mapData.SongName
            };

            InvokeSongStartStop(mapData, beatmapInfo);
            if (mapData.LevelFailed)
            {
                Failed?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Checks if a user enters/quits a level and invoke the corresponding
        /// <see cref="SongStart"/> or <see cref="SongQuit"/> events
        /// </summary>
        /// <param name="mapData">The original map data received from DataPuller</param>
        /// <param name="beatmapInfo">The converted map data for bs connect</param>
        void InvokeSongStartStop(MapData mapData, BeatMapInfo beatmapInfo)
        {
            if (mapData.InLevel == _previousInLevel)
            {
                // In level status not changed, indicates a song is not entered or quit
                SongUpdate?.Invoke(this, beatmapInfo);
                return;
            }

            if (mapData.InLevel)
            {
                _previousMisses = 0;
                SongStart?.Invoke(this, beatmapInfo);
            }
            else
            {
                SongQuit?.Invoke(this, new SongQuitEventArgs
                {
                    BeatMapInfo = beatmapInfo,
                    Scene = GetQuitStatus(mapData)
                });
            }

            _previousInLevel = mapData.InLevel;
        }

        /// <summary>
        /// Gets the quit status of the map from map data
        /// </summary>
        /// <param name="mapData"></param>
        /// <returns></returns>
        static string GetQuitStatus(MapData mapData)
        {
            return mapData.LevelFailed ? QuitStatus.Fail // When NF is off
                : mapData.LevelFinished ?
                    mapData.LevelFailed ? QuitStatus.Fail // When NF is on and user failed
                        : QuitStatus.Finish : "";
        }

        /// <summary>
        /// Handles on live data received from data puller event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        void LiveDataWs_OnMessageReceived(object? sender, string e)
        {
            var liveData = NullableJsonSerializer.Deserialize<LiveData>(e);
            if (liveData == null) return; // Cannot parse result, ignore

            var stats = new PlayerStats
            {
                Combo = liveData.Combo,
                Energy = liveData.PlayerHealth,
                Misses = liveData.Misses,
                Score = liveData.Score,
                TimeElapsed = liveData.TimeElapsed,
                Accuracy = liveData.Accuracy,
                Rank = liveData.Rank
            };
            ScoreChanged?.Invoke(this, stats);

            CheckMiss(liveData.Misses);
        }

        /// <summary>
        /// Checks if the score update contains a miss
        /// </summary>
        /// <param name="misses"></param>
        void CheckMiss(int misses)
        {
            if (misses > _previousMisses)
            {
                Missed?.Invoke(this, EventArgs.Empty);
                _previousMisses = misses;
            }
        }

        /// <summary>
        /// Handles web socket on closed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void WebSocket_OnClosed(object? sender, string? e)
        {
            var ws = (WebSocket) sender!;
            await ws.ReconnectAsync();
        }

        ///
        /// <inheritdoc />
        ///
        public void Stop()
        {
            _mapDataWs.Close();
            _liveDataWs.Close();
        }
    }
}
