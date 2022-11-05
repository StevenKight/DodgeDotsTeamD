namespace DodgeDots.Model
{
    /// <summary>
    ///     Holds the settings used in the game
    /// </summary>
    public class GameSettings
    {
        #region Types and Delegates

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
        ///     TODO Move to level
        /// </summary>
        public const int PointItemAmount = 10;

        #endregion
    }
}