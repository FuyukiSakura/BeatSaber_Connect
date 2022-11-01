using BeatSaber_FakeMultiplay.Client.Services.BeatSaber;

namespace BeatSaber_FakeMultiplay.Client.Services.Integrations.VTPlus
{
    /// <summary>
    /// Connects to VTube Plus web socket
    /// </summary>
    /// <remarks>
    /// For more info, please refer to https://vtuberplus.com/#websockets
    /// </remarks>
    public class VTPlusSocket
    {
        readonly WebSocket _ws = new ("ws://127.0.0.1:4430/vtplus");
        readonly BeatSaberSocketResolver _bsSocketResolver;

        /// <summary>
        /// Creates a new instance of <see cref="VTPlusSocket" />
        /// </summary>
        /// <param name="bsSocketResolver"></param>
        public VTPlusSocket(BeatSaberSocketResolver bsSocketResolver)
        {
            _bsSocketResolver = bsSocketResolver;
        }

        /// <summary>
        /// Starts the VTube Plus socket and listen for bs miss events
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            var bsSocket = await _bsSocketResolver.ResolveAsync();
            if (bsSocket == null)
            {
                // None of the sockets supported can connect
                return;
            }

            bsSocket.Missed += BsSocket_OnMissed;
            await bsSocket.StartAsync();
            await _ws.ConnectAsync();
        }

        /// <summary>
        /// Tests throw items
        /// </summary>
        /// <returns></returns>
        public async Task TestAsync()
        {
            if (!_ws.IsConnected)
            {
                await _ws.ConnectAsync();
            }
            await _ws.SendTextAsync("VTP_Throw:10:0:-1:2");
        }

        /// <summary>
        /// Handles a missed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void BsSocket_OnMissed(object? sender, EventArgs e)
        {
            await _ws.SendTextAsync("VTP_Throw:1:0:-1:2");
        }
    }
}
