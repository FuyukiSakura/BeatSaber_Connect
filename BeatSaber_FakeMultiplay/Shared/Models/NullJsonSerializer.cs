using System.Diagnostics;
using System.Text.Json;

namespace BeatSaber_FakeMultiplay.Shared.Models
{
    /// <summary>
    /// Json serializer that returns null instead of Json exception to avoid app crash
    /// </summary>
    public class NullableJsonSerializer
    {
        /// <summary>
        /// Serialize json string returned from data puller map data
        /// returns null when serialize error occurs
        /// </summary>
        /// <param name="dataPullerJson"></param>
        /// <returns></returns>
        public static T? Deserialize<T>(string dataPullerJson)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(dataPullerJson);
            }
            catch (JsonException ex)
            {
                Debug.WriteLine(ex.Message);
                return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default;
            }
        }
    }
}
