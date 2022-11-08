using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static DodgeDots.Model.GameSettings;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Manages all the waves of dots in the game
    /// </summary>
    public class WaveManager
    {
        #region Data members

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
        public void StartWave(Wave waveStart, Color color)
        {
            switch (waveStart)
            {
                case Wave.North:
                case Wave.West:
                case Wave.South:
                case Wave.East:
                default:
                    var wave = new DotManager(this.backgroundCanvas, waveStart, color);
                    this.waves.Add(wave);
                    break;
                case Wave.NsFinalBlitz:
                    this.northSouthFinalBlitz(color);
                    break;
                case Wave.DiagonalFinalBlitz:
                    this.diagonalFinalBlitz(color);
                    break;
            }
        }

        private void diagonalFinalBlitz(Color color)
        {
            var waves = Enum.GetValues(typeof(Wave));

            foreach (Wave wave in waves)
            {
                var generatedWave = new DotManager(this.backgroundCanvas, wave, color)
                {
                    FinalBlitzMultiplier = 2,
                    DiagonalWave = true
                };

                this.waves.Add(generatedWave);
            }
        }

        private void northSouthFinalBlitz(Color color)
        {
            var waveMoveDown = new DotManager(this.backgroundCanvas, Wave.North, color);
            var waveMoveUp = new DotManager(this.backgroundCanvas, Wave.South, color);

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

        /// <summary>
        ///     Removes dots that the player has hit.
        /// </summary>
        /// <returns>An asynchronous operation.</returns>
        public async Task RemoveHitDotsAsync()
        {
            foreach (var wave in this.waves)
            {
                var dotCount = wave.Count();
                for (var i = 0; i < dotCount; i++)
                {
                    var dot = wave.ElementAt(i);
                    if (this.player.IsCircleOverlapPlayer(dot))
                    {
                        wave.RemoveSingleDot(dot);

                        var file = await AudioManager.AudioFolder.Result.GetFileAsync("DotDestroyed.wav");
                        AudioManager.PlayAudio(file);

                        i--;
                        dotCount--;
                    }
                }
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