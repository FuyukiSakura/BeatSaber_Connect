using System.Net.WebSockets;
using BeatSaber_FakeMultiplay.Shared.Models.BeatSaber;
using BeatSaber_FakeMultiplay.Shared.Models.Event;

namespace BeatSaber_FakeMultiplay.Client.Services
{
    /// <summary>
    /// Connects to http-status socket
    /// </summary>
    public class HttpStatusSocket : IBeatSaberSocket
    {
        const string HttpStatusUrl = "ws://127.0.0.1:6557/socket";

        WebSocket _httpStatusConnection = new (HttpStatusUrl);

        public event EventHandler<PlayerStats>? ScoreChanged;
        public event EventHandler? SongStart;
        public event EventHandler? Failed;

        /// <summary>
        /// Starts the http-status socket
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            _httpStatusConnection = new WebSocket(HttpStatusUrl);
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
            var socketEvent = HttpStatusJson.Serialize(e);
            if (socketEvent == null) return; // Cannot parse result, listen for next event
                
            switch (socketEvent.Event)
            {
                case EventType.Hello:
                case EventType.ScoreChanged:
                    if (socketEvent.Status.Performance == null) break;
                    var performance = socketEvent.Status.Performance;
                    var stats = new PlayerStats
                    {
                        Score = performance.Score,
                        Accuracy = performance.RelativeScore * 100,
                        Rank = performance.Rank
                    };
                    ScoreChanged?.Invoke(this, stats);
                    break;
                case EventType.SongStart:
                    SongStart?.Invoke(this, EventArgs.Empty);
                    break;
                case EventType.Failed:
                    Failed?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        /// <summary>
        /// Stops listening to the http-status socket
        /// </summary>
        public void Stop()
        {
            _httpStatusConnection.Close();
        }
    }
}
