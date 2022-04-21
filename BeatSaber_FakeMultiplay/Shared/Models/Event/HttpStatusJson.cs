using System.Diagnostics;
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

        /// <summary>
        /// Serialize json string returned from http-status
        /// returns null when serialize error occurs
        /// </summary>
        /// <param name="httpStatusJson"></param>
        /// <returns></returns>
        public static SocketEvent? Serialize(string httpStatusJson)
        {
            try
            {
                return JsonSerializer.Deserialize<SocketEvent>(httpStatusJson, SerializerOptions);
            }
            catch (JsonException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
