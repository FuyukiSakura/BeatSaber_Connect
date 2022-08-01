
namespace BeatSaber_FakeMultiplay.Shared.Models.Reselim
{
    /// <summary>
    /// Modifiers of reselim beat saber overlay (advanced)
    /// </summary>
    /// <remarks>
    /// Please refer to
    /// - https://github.com/rynan4818/beat-saber-overlay/
    /// - https://github.com/Reselim/beat-saber-overlay
    /// </remarks>
    public class ReselimModifiers
    {
        /// <summary>
        /// Gets or sets if all data should appear
        /// </summary>
        public bool All { get; set; }

        /// <summary>
        /// Gets or sets if the BSR key should appear
        /// </summary>
        public bool Bsr { get; set; }

        /// <summary>
        /// Gets or sets if the Energy bar should appear
        /// </summary>
        public bool Energy { get; set; }

        /// <summary>
        /// Gets or sets if Custom Difficulty name should appear
        /// </summary>
        public bool Label { get; set; }

        /// <summary>
        /// Gets or sets if Miss count should appear
        /// </summary>
        public bool Miss { get; set; }

        /// <summary>
        /// Gets or sets if the current PP point should appear
        /// </summary>
        public bool PP { get; set; }

        /// <summary>
        /// Gets or sets if the info should display from right to left 
        /// </summary>
        public bool Rtl { get; set; }

        /// <summary>
        /// Gets or sets if the window should scale for FHD(1920x1080) streams
        /// </summary>
        public bool Scale { get; set; }

        /// <summary>
        /// Gets or sets the debug status
        /// </summary>
        public bool Test { get; set; }

        /// <summary>
        /// Gets or sets if hte info should display from top
        /// </summary>
        public bool Top { get; set; }
    }
}
