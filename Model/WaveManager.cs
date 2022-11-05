using System;
using System.Collections.Generic;
using System.Drawing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Color = Windows.UI.Color;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Manages all the waves of dots in the game
    /// </summary>
    public class WaveManager
    {
        #region Types and Delegates

        /// <summary>
        ///     A delegate for hitting a point object
        /// </summary>
        /// <param name="pointAmount">The point amount.</param>
        public delegate void PointHitHandler(int pointAmount);

        #endregion

        #region Data members

        private readonly Canvas backgroundCanvas;
        private readonly Player player;
        private readonly PointManager pointManager;
        private PointObject lastHitPoint;

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
            this.pointManager = new PointManager(background);

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
        ///     Occurs when Player hits a Point.
        /// </summary>
        public event PointHitHandler PointHit;

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
                    var waveMoveDown = new DotManager(this.backgroundCanvas, GameSettings.Wave.North, color);
                    var waveMoveUp = new DotManager(this.backgroundCanvas, GameSettings.Wave.South, color);

                    // TODO Use level class to store these
                    waveMoveDown.FinalBlitzMultiplier = 2;
                    waveMoveUp.FinalBlitzMultiplier = 2;

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
        ///     Determines whether [has player hit a point].
        /// </summary>
        /// <returns>
        ///     <c>true</c> if [has player hit a point]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasPlayerHitAPoint()
        {
            foreach (var point in this.pointManager)
            {
                if (this.isCircleCollisionForPlayerAndDot(point))
                {
                    this.lastHitPoint = point;
                    this.onPointHit();
                    return true;
                }
            }

            return false;
        }

        private void onPointHit()
        {
            this.PointHit?.Invoke(this.lastHitPoint.PointAmount);
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
            this.pointManager.RemovePointObjects();
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
            if (this.HasPlayerHitAPoint())
            {
                this.removePointObject(this.lastHitPoint);
            }
        }

        private void removePointObject(PointObject point)
        {
            if (point != null)
            {
                this.pointManager.Remove(point);
                this.backgroundCanvas.Children.Remove(point.Sprite);
            }
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

        private bool isCircleCollisionForPlayerAndDot(GameObject dot)
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