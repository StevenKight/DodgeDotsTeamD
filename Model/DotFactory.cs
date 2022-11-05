using System;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Dot Factory class
    /// </summary>
    /// <seealso cref="DodgeDots.Model.GameObject" />
    public class DotFactory : Dot
    {
        #region Methods

        /// <summary>
        ///     Creates the dot.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">direction - null</exception>
        public Dot CreateDot(GameSettings.DotType type, GameSettings.Wave wave)
        {
            var dot = new DotFactory();
            switch (type)
            {
                case GameSettings.DotType.NormalDot:
                    setDirectionalSpeeds(wave, dot);

                    break;
                case GameSettings.DotType.FinalBlitzDot:
                    setDirectionalSpeeds(wave, dot);
                    dot.SetColor(GameSettings.FinalBlitzDotColor);
                    dot.SetSpeed(dot.SpeedX * GameSettings.FinalBlitzSpeedMultiplier,
                        dot.SpeedY * GameSettings.FinalBlitzSpeedMultiplier);
                    break;
            }

            return dot;
        }

        private static void setDirectionalSpeeds(GameSettings.Wave wave, DotFactory dot)
        {
            switch (wave)
            {
                case GameSettings.Wave.South:
                    dot.SetColor(GameSettings.PrimaryDotColor);
                    dot.SetSpeed(0, -dot.SpeedY);
                    break;
                case GameSettings.Wave.North:
                    dot.SetColor(GameSettings.PrimaryDotColor);
                    dot.SetSpeed(0, dot.SpeedY);
                    break;
                case GameSettings.Wave.East:
                    dot.SetColor(GameSettings.SecondaryDotColor);
                    dot.SetSpeed(-dot.SpeedX, 0);
                    break;
                case GameSettings.Wave.West:
                    dot.SetSpeed(dot.SpeedX, 0);
                    dot.SetColor(GameSettings.SecondaryDotColor);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(wave), wave, null);
            }
        }

        #endregion
    }
}