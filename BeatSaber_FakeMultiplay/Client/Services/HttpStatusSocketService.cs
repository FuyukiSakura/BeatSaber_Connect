using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using BeatSaber_FakeMultiplay.Shared.Models.Event;
using Microsoft.AspNetCore.SignalR.Client;

namespace BeatSaber_FakeMultiplay.Client.Services
{
    /// <summary>
    /// Handles http-status socket communications
    /// </summary>
    public class HttpStatusSocketService
    {
        /// <summary>
        /// Receives update from Beat Saber HttpStatus
        /// and broadcast it to other players
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="beatSaberConnection">The websocket client to receive from</param>
        /// <param name="bsHubConnection">The target websocket to send the data to</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task ReceiveAsync(
            string playerName,
            WebSocket beatSaberConnection,
            HubConnection bsHubConnection,
            CancellationTokenSource cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var ms = new MemoryStream();
                WebSocketReceiveResult result;
                do
                {
                    var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
                    result = await beatSaberConnection.ReceiveAsync(messageBuffer, CancellationToken.None);
                    ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                var httpStatusJson = Encoding.UTF8.GetString(ms.ToArray());
                var socketEvent = HttpStatusJson.Serialize(httpStatusJson);
                if (socketEvent == null) continue; // Cannot determine result

                if (socketEvent.Event != EventType.Hello // is hello event
                    && socketEvent.Event != EventType.ScoreChanged // is score update event
                    || socketEvent.Status.Performance == null // No performance data
                   ) continue;

                var performanceJson = JsonSerializer.Serialize(socketEvent.Status.Performance);
                await bsHubConnection.SendAsync("ScoreUpdate", playerName, performanceJson);

            }
        }
    }
}
