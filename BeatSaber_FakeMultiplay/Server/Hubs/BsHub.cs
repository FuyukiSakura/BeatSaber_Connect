using BeatSaber_FakeMultiplay.Shared.Models.Socket;
using Microsoft.AspNetCore.SignalR;

namespace BeatSaber_FakeMultiplay.Server.Hubs
{
    public class BsHub : Hub
    {
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
