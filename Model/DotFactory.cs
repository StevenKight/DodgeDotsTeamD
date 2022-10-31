using System;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Dot Factory class
    /// </summary>
    /// <seealso cref="DodgeDots.Model.GameObject" />
    public class DotFactory : Dot
    {
        #region Data members

        private const int FinalBlitzSpeedMultiplier = 2;

        #endregion

        #region Methods

        /// <summary>
        ///     Creates the dot.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">direction - null</exception>
        public Dot CreateDot(GameSettings.DotType type, GameSettings.Direction direction)
        {
            var dot = new DotFactory();
            switch (type)
            {
                case GameSettings.DotType.NormalDot:
                    setDirectionalSpeeds(direction, dot);

                    break;
                case GameSettings.DotType.FinalBlitzDot:
                    setDirectionalSpeeds(direction, dot);
                    dot.SetColor(GameSettings.FinalBlitzDotColor);
                    dot.SetSpeed(dot.SpeedX * FinalBlitzSpeedMultiplier, dot.SpeedY * FinalBlitzSpeedMultiplier);
                    break;
            }

            return dot;
        }

        private static void setDirectionalSpeeds(GameSettings.Direction direction, DotFactory dot)
        {
            switch (direction)
            {
                case GameSettings.Direction.Up:
                    dot.SetColor(GameSettings.PrimaryDotColor);
                    dot.SetSpeed(0, -1 * dot.SpeedY);
                    break;
                case GameSettings.Direction.Down:
                    dot.SetColor(GameSettings.PrimaryDotColor);
                    dot.SetSpeed(0, dot.SpeedY);
                    break;
                case GameSettings.Direction.Left:
                    dot.SetColor(GameSettings.SecondaryDotColor);
                    dot.SetSpeed(-1 * dot.SpeedX, 0);
                    break;
                case GameSettings.Direction.Right:
                    dot.SetSpeed(dot.SpeedX, 0);
                    dot.SetColor(GameSettings.SecondaryDotColor);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        #endregion
    }
}