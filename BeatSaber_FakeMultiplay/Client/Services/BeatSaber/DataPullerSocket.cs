﻿using BeatSaber_FakeMultiplay.Shared.Models;
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

        public event EventHandler<PlayerStats>? ScoreChanged;
        public event EventHandler<BeatMapInfo>? SongStart;
        public event EventHandler? Failed;

        WebSocket _mapDataWs = new (MapDataUrl);
        WebSocket _liveDataWs = new (LiveDataUrl);

        ///
        /// <inheritdoc />
        ///
        public async Task StartAsync()
        {
            _mapDataWs = new WebSocket(MapDataUrl);
            _mapDataWs.MessageReceived += MapDataWsOnMessageReceived;
            _mapDataWs.Closed += WebSocket_OnClosed;
            await _mapDataWs.ConnectAsync();

            _liveDataWs = new WebSocket(LiveDataUrl);
            _liveDataWs.MessageReceived += LiveDataWs_OnMessageReceived;
            _liveDataWs.Closed += WebSocket_OnClosed;
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
            if (mapData.InLevel)
            {
                SongStart?.Invoke(this, beatmapInfo);
            }

            if (mapData.LevelFailed)
            {
                Failed?.Invoke(this, EventArgs.Empty);
            }
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
