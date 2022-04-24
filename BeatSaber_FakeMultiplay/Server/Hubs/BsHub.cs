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
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task SongStart(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName)) return; // No room id given

            await Clients.Group(groupName).SendAsync(BsHubMessage.SongStart);
        }

        /// <summary>
        /// Broadcasts player score to all subscribers when
        /// an <see cref="BsHubMessage.ScoreUpdate" /> event is received
        /// </summary>
        /// <param name="playerName">The name of the joined player</param>
        /// <param name="groupName"></param>
        /// <param name="httpStatusJson"></param>
        /// <returns></returns>
        public async Task ScoreUpdate(string playerName, string groupName, string httpStatusJson)
        {
            if (string.IsNullOrWhiteSpace(groupName)) return; // No room id given

            await Clients.Group(groupName).SendAsync(BsHubMessage.ScoreUpdate, playerName, httpStatusJson);
        }

        /// <summary>
        /// Receive events when player enters the room
        /// </summary>
        /// <param name="player">The display name given by the player</param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task JoinRoom(string player, string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName)) return; // No room id given

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync(BsHubMessage.JoinRoom, player);
        }

        /// <summary>
        /// Adds leader board subscribers page to the group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public async Task Subscribe(string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName)) return; // No room id given

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
