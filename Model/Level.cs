using System.Collections.ObjectModel;
using Windows.UI;

namespace DodgeDots.Model.Levels
{
    public class Level
    {
        #region Properties

        /// <summary>
        ///     The title of the given level.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     The length of the given level in seconds.
        /// </summary>
        public int GameSurvivalTime { get; set; }

        /// <summary>
        ///     The interval between waves in level, in seconds.
        /// </summary>
        public int WaveInterval { get; set; }

        /// <summary>
        ///     List of waves in the given level
        /// </summary>
        public Collection<GameSettings.Wave> Waves { get; set; }

        /// <summary>
        ///     The color of the given waves
        /// </summary>
        public Collection<Color> WaveColors { get; set; }

        /// <summary>
        ///     The multiplier applied to final blitz level max speed.
        /// </summary>
        public int FinalBlitzSpeedMultiplier { get; set; }

        #endregion
    }
}