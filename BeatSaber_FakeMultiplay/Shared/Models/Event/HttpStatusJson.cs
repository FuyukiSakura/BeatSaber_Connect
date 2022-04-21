using System.Text.Json;
using System.Text.Json.Serialization;

namespace BeatSaber_FakeMultiplay.Shared.Models.Event
{
    /// <summary>
    /// Represents http-status json structure options
    /// </summary>
    public static class HttpStatusJson
    {
        /// <summary>
        /// Gets the default SerializerOptions
        /// </summary>
        public static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters ={
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            },
        };
    }
}
