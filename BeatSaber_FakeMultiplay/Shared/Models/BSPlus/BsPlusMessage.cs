using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics;

namespace BeatSaber_FakeMultiplay.Shared.Models.BSPlus
{
    /// <summary>
    /// Represents BS+ overlay json structure options
    /// </summary>
    public static class BsPlusMessage
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
        /// Serialize json string returned from BS+ overlay
        /// returns null when serialize error occurs
        /// </summary>
        /// <param name="bsPlusOverlayJson"></param>
        /// <returns></returns>
        public static BsPlusEvent? Serialize(string bsPlusOverlayJson)
        {
            try
            {
                return JsonSerializer.Deserialize<BsPlusEvent>(bsPlusOverlayJson, SerializerOptions);
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
