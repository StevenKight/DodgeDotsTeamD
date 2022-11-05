using System;
using System.Collections.Generic;
using System.Drawing;
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
        public void StartWave(GameSettings.Wave waveStart, GameSettings.DotType type)
        {
            switch (type)
            {
                case GameSettings.DotType.NormalDot:
                    var wave = new DotManager(this.backgroundCanvas, waveStart, type);
                    this.waves.Add(wave);
                    break;
                case GameSettings.DotType.FinalBlitzDot:
                    var waveMoveDown = new DotManager(this.backgroundCanvas, GameSettings.Wave.North,
                        GameSettings.DotType.FinalBlitzDot);
                    var waveMoveUp = new DotManager(this.backgroundCanvas, GameSettings.Wave.South,
                        GameSettings.DotType.FinalBlitzDot);
                    this.waves.Add(waveMoveDown);
                    this.waves.Add(waveMoveUp);
                    break;
            }
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
        public void StopAllActiveDotManagers()
        {
            foreach (var newWave in this.waves)
            {
                newWave.StopDotManager();
                this.timer.Stop();
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
                if (this.isCircleCollisionForPlayerAndDot(dot))
                {
                    if (this.isCollidingDotSameColorAsPlayerDot(dot))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool isCollidingDotSameColorAsPlayerDot(Dot dot)
        {
            return this.player.OuterColor != dot.Color;
        }

        private bool isCircleCollisionForPlayerAndDot(Dot dot)
        {
            var playerRectangle = new Rectangle((int)this.player.X, (int)this.player.Y, (int)this.player.Width,
                (int)this.player.Height);
            var dotRectangle = new Rectangle((int)dot.X, (int)dot.Y, (int)dot.Width, (int)dot.Height);
            var radiusSum = playerRectangle.Width / 2.0 + dotRectangle.Width / 2.0;
            var deltaX = playerRectangle.X - dotRectangle.X;
            var deltaY = playerRectangle.Y - dotRectangle.Y;
            return deltaX * deltaX + deltaY * deltaY <= radiusSum * radiusSum;
        }

        #endregion
    }
}