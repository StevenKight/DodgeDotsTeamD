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
        #region Properties

        /// <summary>
        ///     Gets if the wave is diagonal or not.
        /// </summary>
        public bool DiagonalSpeed { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates the dot.
        /// </summary>
        /// <returns>The new dot created</returns>
        /// <param name="color">The color of the dot.</param>
        /// <param name="wave">The wave the dot is in.</param>
        /// <param name="finalMultiplier">The multiplier to adjust speed for final blitz.</param>
        /// <param name="diagonal">If the wave is diagonal or not.</param>
        public Dot CreateDot(Color color, GameSettings.Wave wave, int finalMultiplier, bool diagonal)
        {
            this.DiagonalSpeed = diagonal;

            var dot = new DotFactory();
            switch (wave)
            {
                case GameSettings.Wave.North:
                case GameSettings.Wave.West:
                case GameSettings.Wave.South:
                case GameSettings.Wave.East:
                default:
                    this.setDirectionalSpeeds(wave, dot);
                    dot.SetColor(color);

                    break;
                case GameSettings.Wave.NsFinalBlitz:
                    this.setFinalBlitzSpeeds(color, wave, finalMultiplier, dot);
                    break;
                case GameSettings.Wave.DiagonalFinalBlitz:
                    this.setFinalBlitzSpeeds(color, wave, finalMultiplier, dot);
                    break;
            }

            return dot;
        }

        private void setFinalBlitzSpeeds(Color color, GameSettings.Wave wave, int finalMultiplier,
            DotFactory dot)
        {
            this.setDirectionalSpeeds(wave, dot);
            dot.SetColor(color);
            dot.SetSpeed(dot.SpeedX * finalMultiplier,
                dot.SpeedY * finalMultiplier);
        }

        private void setDirectionalSpeeds(GameSettings.Wave wave, DotFactory dot)
        {
            if (!this.DiagonalSpeed)
            {
                normalDotSpeed(wave, dot);
            }
            else
            {
                diagonalDotSpeed(wave, dot);
            }
        }

        private static void normalDotSpeed(GameSettings.Wave wave, DotFactory dot)
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
                case GameSettings.Wave.NsFinalBlitz:
                case GameSettings.Wave.DiagonalFinalBlitz:
                default:
                    throw new ArgumentOutOfRangeException(nameof(wave), wave, null);
            }
        }

        private static void diagonalDotSpeed(GameSettings.Wave wave, DotFactory dot)
        {
            switch (wave)
            {
                case GameSettings.Wave.South:
                case GameSettings.Wave.East:
                    dot.SetSpeed(-dot.SpeedX, -dot.SpeedY);
                    break;
                case GameSettings.Wave.North:
                case GameSettings.Wave.West:
                case GameSettings.Wave.NsFinalBlitz:
                case GameSettings.Wave.DiagonalFinalBlitz:
                default:
                    throw new ArgumentOutOfRangeException(nameof(wave), wave, null);
            }
        }

        #endregion
    }
}