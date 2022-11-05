using System;
using Windows.UI;

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
        /// <returns>The new dot created</returns>
        /// <param name="color">The color of the dot.</param>
        /// <param name="wave">The wave the dot is in.</param>
        /// <param name="finalMultiplier">The multiplier to adjust speed for final blitz.</param>
        public Dot CreateDot(Color color, GameSettings.Wave wave, int finalMultiplier)
        {
            var dot = new DotFactory();
            switch (wave)
            {
                default:
                    setDirectionalSpeeds(wave, dot);
                    dot.SetColor(color);

                    break;
                case GameSettings.Wave.NsFinalBlitz:
                    setDirectionalSpeeds(wave, dot);
                    dot.SetColor(color);
                    dot.SetSpeed(dot.SpeedX * finalMultiplier,
                        dot.SpeedY * finalMultiplier);
                    break;
            }

            return dot;
        }

        private static void setDirectionalSpeeds(GameSettings.Wave wave, DotFactory dot)
        {
            switch (wave)
            {
                case GameSettings.Wave.South:
                    dot.SetSpeed(0, -dot.SpeedY);
                    break;
                case GameSettings.Wave.North:
                    dot.SetSpeed(0, dot.SpeedY);
                    break;
                case GameSettings.Wave.East:
                    dot.SetSpeed(-dot.SpeedX, 0);
                    break;
                case GameSettings.Wave.West:
                    dot.SetSpeed(dot.SpeedX, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(wave), wave, null);
            }
        }

        #endregion
    }
}