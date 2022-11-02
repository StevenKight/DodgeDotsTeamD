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
        ///     Direction Enumeration used to determine movement direction
        /// </summary>
        public enum Direction
        {
            /// <summary>
            ///     The Down Direction Enum
            /// </summary>
            Down,

            /// <summary>
            ///     The Left Direction Enum
            /// </summary>
            Left,

            /// <summary>
            ///     The Right Direction Enum
            /// </summary>
            Right,

            /// <summary>
            ///     The Up direction Enum
            /// </summary>
            Up
        }

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

        #endregion

        #region Data members

        /// <summary>
        ///     The game survival time
        /// </summary>
        public const int GameSurvivalTime = 25;

        /// <summary>
        ///     The application height
        /// </summary>
        public const double ApplicationHeight = 450;

        /// <summary>
        ///     The application width
        /// </summary>
        public const double ApplicationWidth = 450;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the primary dot color.
        /// </summary>
        /// <value>
        ///     The primary dot color.
        /// </value>
        public static Color PrimaryDotColor => Colors.MediumSpringGreen;

        /// <summary>
        ///     Gets the secondary dot color.
        /// </summary>
        /// <value>
        ///     The secondary dot color.
        /// </value>
        public static Color SecondaryDotColor => Colors.DarkViolet;

        /// <summary>
        ///     Gets the final blitz dot color.
        /// </summary>
        /// <value>
        ///     The final blitz dot color.
        /// </value>
        public static Color FinalBlitzDotColor => Colors.Yellow;

        #endregion
    }
}