using BeatSaber_FakeMultiplay.Shared.Models.Socket;
using Microsoft.AspNetCore.SignalR;

namespace BeatSaber_FakeMultiplay.Server.Hubs
{
    public class BsHub : Hub
    {
        /// <summary>
        /// Broadcasts song start event
        /// when a <see cref="BsHubMessage.SongStart" /> event is received
        /// </summary>
        /// <returns></returns>
        public async Task SongStart()
        {
            await Clients.All.SendAsync(BsHubMessage.SongStart);
        }

        /// <summary>
        /// Broadcasts player score to all subscribers when
        /// an <see cref="BsHubMessage.ScoreUpdate" /> event is received
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="httpStatusJson"></param>
        /// <returns></returns>
        public async Task ScoreUpdate(string playerName, string httpStatusJson)
        {
            await Clients.All.SendAsync(BsHubMessage.ScoreUpdate, playerName, httpStatusJson);
        }

        /// <summary>
        /// Receive events when player enters the room
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public async Task JoinRoom(string player)
        {
            await Clients.All.SendAsync("PlayerEntered", player);
        }
    }
}
