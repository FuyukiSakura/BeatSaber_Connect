using System;
using System.Collections.Generic;
using System.Linq;

namespace BeatSaber_FakeMultiplay.Shared.Models.BsDataPuller
{
    /// <summary>
    /// Represents the current player stats from data puller
    /// 
    /// Pasted from https://github.com/ReadieFur/BSDataPuller
    /// </summary>
    public class LiveData
    {
        public int Score { get; set; }
        public int ScoreWithMultipliers { get; set; }
        public int MaxScore { get; set; }
        public int MaxScoreWithMultipliers { get; set; }
        public string Rank { get; set; }
        public bool FullCombo { get; set; }
        public int Combo { get; set; }
        public int Misses { get; set; }
        public float Accuracy { get; set; }
        public int[] BlockHitScore { get; set; }
        public float PlayerHealth { get; set; }
        public int TimeElapsed { get; set; }
        public long unixTimestamp { get; set; }
    }
}
