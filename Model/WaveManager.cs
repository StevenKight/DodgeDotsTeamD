using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Manages all the waves of dots in the game
    /// </summary>
    public class WaveManager
    {
        #region Data members

        private const int NumberNormalWaves = 4;

        private readonly Canvas backgroundCanvas;
        private readonly Player player;

        private readonly DispatcherTimer timer;
        private readonly IList<DotManager> waves;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WaveManager" /> class.
        /// </summary>
        /// <param name="background">The background.</param>
        /// <param name="player">The player.</param>
        public WaveManager(Canvas background, Player player)
        {
            this.waves = new List<DotManager>();

            this.backgroundCanvas = background;
            this.player = player;

            this.timer = new DispatcherTimer();
            this.timer.Tick += this.Timer_Tick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            this.timer.Start();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Starts a new wave that moves newly populated dots
        ///     from the top of the screen to the bottom of the screen.
        /// </summary>
        /// <param name="waveStart">The wave to create</param>
        /// <param name="color">The color of the dots within the wave</param>
        public void StartWave(GameSettings.Wave waveStart, Color color)
        {
            switch (waveStart)
            {
                default:
                    var wave = new DotManager(this.backgroundCanvas, waveStart, color);
                    this.waves.Add(wave);
                    break;
                case GameSettings.Wave.NsFinalBlitz:
                    this.northSouthFinalBlitz(color);
                    break;
                case GameSettings.Wave.DiagonalFinalBlitz:
                    this.diagonalFinalBlitz(color);
                    break;
            }
        }

        private void diagonalFinalBlitz(Color color)
        {
            var waves = Enum.GetValues(typeof(GameSettings.Wave)).Cast<GameSettings.Wave>();

            for (var i = 0; i < NumberNormalWaves; i++)
            {
                var wave = waves.ElementAt(i);
                var generatedWave = new DotManager(this.backgroundCanvas, wave, color);

                generatedWave.FinalBlitzMultiplier = 2;
                generatedWave.DiagonalWave = true;

                this.waves.Add(generatedWave);
            }
        }

        private void northSouthFinalBlitz(Color color)
        {
            var waveMoveDown = new DotManager(this.backgroundCanvas, GameSettings.Wave.North, color);
            var waveMoveUp = new DotManager(this.backgroundCanvas, GameSettings.Wave.South, color);

            // TODO Use level class to store these
            waveMoveDown.FinalBlitzMultiplier = 2;
            waveMoveUp.FinalBlitzMultiplier = 2;

            this.waves.Add(waveMoveDown);
            this.waves.Add(waveMoveUp);
        }

        /// <summary>
        ///     Checks if the player hits any dot in the game.
        /// </summary>
        /// <returns> true if player hits a dot, false otherwise </returns>
        public bool HasPlayerHitADot()
        {
            foreach (var wave in this.waves)
            {
                if (this.checkPlayerHitsDotInSpecificWave(wave))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Stops all active dot managers.
        /// </summary>
        public void StopAllActiveWaveObjects()
        {
            foreach (var newWave in this.waves)
            {
                newWave.StopDotManager();
            }

            this.timer.Stop();
        }

        /// <summary>
        ///     Removes all waves from tracking and display.
        /// </summary>
        public void RemoveAllWaves()
        {
            foreach (var wave in this.waves)
            {
                wave.RemoveAllDots();
                this.StopAllActiveWaveObjects();
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            this.HasPlayerHitADot();
        }

        private bool checkPlayerHitsDotInSpecificWave(DotManager wave)
        {
            foreach (var dot in wave)
            {
                if (this.player.IsCircleOverlapPlayer(dot))
                {
                    if (!this.player.IsColorSameAsPlayer(dot))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}