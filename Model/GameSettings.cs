using System.Collections.ObjectModel;
using Windows.UI;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Holds the settings used in the game
    /// </summary>
    public class GameSettings
    {
        #region Types and Delegates

        /// <summary>
        ///     Dot Type Enumeration used to determine the type of dot
        /// </summary>
        public enum DotType
        {
            /// <summary>
            ///     The Normal Dot Enum
            /// </summary>
            NormalDot,

            /// <summary>
            ///     The Final Blitz Dot Enum
            /// </summary>
            FinalBlitzDot
        }

        public enum Wave
        {
            North,

            West,

            South,

            East,

            UpDownFinalBlitz,

            DiagonalFinalBlitz
        }

        #endregion

        #region Data members

        /// <summary>
        ///     The length in seconds of the dying animation.
        /// </summary>
        public const int DyingAnimationLength = 2;

        /// <summary>
        ///     The number of lives the player gets
        /// </summary>
        public const int PlayerLives = 3;

        /// <summary>
        ///     The application height
        /// </summary>
        public const double ApplicationHeight = 400;

        /// <summary>
        ///     The application width
        /// </summary>
        public const double ApplicationWidth = 400;

        // TODO Put in a level class
        public static Color PrimaryDotColor = Colors.MediumSpringGreen;
        public static Color SecondaryDotColor = Colors.DarkViolet;
        public const int WaveInterval = 5;
        public const int FinalBlitzSpeedMultiplier = 2;
        public const int GameSurvivalTime = 25;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the final blitz dot color.
        /// </summary>
        /// <value>
        ///     The final blitz dot color.
        /// </value>
        public static Color FinalBlitzDotColor => Colors.Yellow;

        #endregion

        #region Methods

        /// <summary>
        ///     Level 1 for the game.
        /// </summary>
        public static class Level1
        {
            #region Data members

            public const string Title = "Level 1";

            public const int GameSurvivalTime = 25;

            public const int WaveInterval = 5;

            public static Collection<Wave> Waves = new Collection<Wave>
            {
                Wave.North,
                Wave.West,
                Wave.South,
                Wave.East
            };

            public static Collection<Color> WaveColors = new Collection<Color>
            {
                Colors.MediumSpringGreen,
                Colors.DarkViolet
            };

            #endregion
        }

        /// <summary>
        ///     Level 1 for the game.
        /// </summary>
        public static class Level2
        {
            #region Data members

            public const string Title = "Level 2";

            public const int GameSurvivalTime = 30;

            public const int WaveInterval = 5;

            public static Collection<Wave> Waves = new Collection<Wave>
            {
                Wave.North,
                Wave.West,
                Wave.South,
                Wave.East,
                Wave.UpDownFinalBlitz
            };

            public static Collection<Color> WaveColors = new Collection<Color>
            {
                Colors.MediumSpringGreen,
                Colors.DarkViolet
            };

            #endregion
        }

        #endregion
    }
}