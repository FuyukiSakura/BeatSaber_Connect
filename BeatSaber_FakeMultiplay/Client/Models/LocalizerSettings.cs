namespace BeatSaber_FakeMultiplay.Client.Models
{
    public static class LocalizerSettings
    {
        /// <summary>
        /// Gets the default culture of the app
        /// </summary>
        public const string NeutralCulture = "en-AU";

        /// <summary>
        /// Gets a list of cultures supported by the app
        /// </summary>
        public static readonly string[] SupportedCultures = { NeutralCulture, "ja-JP", "zh-TW" };

        /// <summary>
        /// Custom translations of cultures
        /// </summary>
        public static readonly (string, string)[] SupportedCulturesWithName =
        {
            ("English", NeutralCulture),
            ("日本語", "ja-JP"),
            ("繁體中文", "zh-TW")
        };
    }
}
