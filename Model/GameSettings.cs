using Windows.UI;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Holds the settings used in the game
    /// </summary>
    public class GameSettings
    {
        #region Types and Delegates

        public enum PointType
        {
            /// <summary>
            ///     Basic points that spawn with normal frequency.
            /// </summary>
            Basic = 5,

            /// <summary>
            ///     Medium value points that spawn uncommonly.
            /// </summary>
            Mid = 15,

            /// <summary>
            ///     Max value points that spawn rarely
            /// </summary>
            Max = 30
        }

        /// <summary>
        ///     The waves the game can have in a level.
        /// </summary>
        public enum Wave
        {
            /// <summary>
            ///     The wave coming from the North.
            /// </summary>
            North,

            /// <summary>
            ///     The wave coming from the West.
            /// </summary>
            West,

            /// <summary>
            ///     The wave coming from the South.
            /// </summary>
            South,

            /// <summary>
            ///     The wave coming from the East.
            /// </summary>
            East,

            /// <summary>
            ///     The wave coming from the North and South.
            /// </summary>
            NsFinalBlitz,

            /// <summary>
            ///     The wave coming diagonally.
            /// </summary>
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

        /// <summary>
        ///     The color of the basic point object.
        /// </summary>
        public static Color BasicPointColor = Colors.SaddleBrown;

        /// <summary>
        ///     The rarity of the basic point object. (1 - 10)
        /// </summary>
        public const double BasicRarity = 1;

        /// <summary>
        ///     The color of the Mid point object.
        /// </summary>
        public static Color MidPointColor = Colors.Silver;

        /// <summary>
        ///     The rarity of the mid point object. (1 - 10)
        /// </summary>
        public const double MidRarity = 2;

        /// <summary>
        ///     The color of the Max point object.
        /// </summary>
        public static Color MaxPointColor = Colors.Gold;

        /// <summary>
        ///     The rarity of the max point object. (1 - 10)
        /// </summary>
        public const double MaxRarity = 3;

        #endregion
    }
}