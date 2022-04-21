
namespace BeatSaber_FakeMultiplay.Shared.Models.Event
{
    /// <summary>
    /// Represents a http-status socket event
    /// </summary>
    public class SocketEvent
    {
        /// <summary>
        /// Gets or sets the event type this request is about
        /// </summary>
        public EventType Event { get; set; }

        /// <summary>
        /// Gets or sets the data returned by this request
        /// </summary>
        public Status Status { get; set; } = new();
    }
}
