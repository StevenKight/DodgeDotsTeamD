using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Manages the dots in a game.
    /// </summary>
    public class DotManager : IEnumerable<Dot>
    {
        #region Data members

        private readonly IList<Dot> dots;
        private readonly Canvas canvas;
        private readonly GameSettings.Wave wave;
        private readonly Color color;
        private readonly DotFactory dotFactory;

        private readonly DispatcherTimer timer;
        private int tickCount;
        private int randomSpawnTick;

        #endregion

        #region Properties

        /// <summary>
        ///     The amount to multiply the speed of the final blitz by.
        /// </summary>
        public int FinalBlitzMultiplier { get; set; }

        /// <summary>
        ///     Creates dots with diagonal movement.
        /// </summary>
        public bool DiagonalWave { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DotManager" /> class.
        ///     Precondition: None
        ///     Post-condition: dots list is created, and tick is setup
        /// </summary>
        /// <param name="background">The background.</param>
        /// <param name="wave">The wave to generate.</param>
        /// <param name="color">The color of dots.</param>
        public DotManager(Canvas background, GameSettings.Wave wave, Color color)
        {
            this.dots = new List<Dot>();
            this.dotFactory = new DotFactory();
            this.canvas = background;
            this.wave = wave;
            this.color = color;
            this.FinalBlitzMultiplier = 1;
            this.DiagonalWave = false;

            this.tickCount = 0;
            this.randomSpawnTick = 0;
            this.timer = new DispatcherTimer();
            this.timer.Tick += this.Timer_Tick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            this.timer.Start();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Ability to iterate through the collection of dots.
        /// </summary>
        /// <returns> an enumerator that iterates through the collection of dots </returns>
        public IEnumerator<Dot> GetEnumerator()
        {
            return this.dots.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.dots).GetEnumerator();
        }

        /// <summary>
        ///     Stops the dots from running.
        /// </summary>
        public void StopDotManager()
        {
            this.timer.Stop();
        }

        /// <summary>
        ///     Removes all dots from tracking and from the display.
        /// </summary>
        public void RemoveAllDots()
        {
            foreach (var dot in this.dots)
            {
                this.canvas.Children.Remove(dot.Sprite);
            }

            this.dots.Clear();
            this.StopDotManager();
        }

        /// <summary>
        ///     Removes a single dot from the game.
        /// </summary>
        /// <param name="dot">The dot to remove</param>
        public void RemoveSingleDot(Dot dot)
        {
            this.dots.Remove(dot);
            this.canvas.Children.Remove(dot.Sprite);
        }

        private void Timer_Tick(object sender, object e)
        {
            if (this.tickCount == this.randomSpawnTick)
            {
                this.createDot();
                this.tickCount = 0;

                var randomSpawnNext = GameSettings.rnd.Next(20, 40);
                this.randomSpawnTick = randomSpawnNext;
            }
            else
            {
                this.tickCount++;
            }

            this.moveDots();
            this.removeDot(this.checkDotsOffCanvas());
        }

        private void createDot()
        {
            var dot = this.dotFactory.CreateDot(this.color, this.wave, this.FinalBlitzMultiplier, this.DiagonalWave);
            this.dots.Add(dot);
            this.canvas.Children.Add(dot.Sprite);
            this.setDotInPosition(dot);
        }

        private void setDotInPosition(Dot dot)
        {
            switch (this.wave)
            {
                case GameSettings.Wave.North:
                case GameSettings.Wave.South:
                    this.randomlyPositionUpAndDownDirectionalDots(dot);
                    break;
                case GameSettings.Wave.West:
                case GameSettings.Wave.East:
                    this.randomlyPositionLeftAndRightDirectionalDots(dot);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void randomlyPositionLeftAndRightDirectionalDots(Dot dot)
        {
            var yPositionsInBoundary =
                Enumerable.Range(0, (int)(GameSettings.ApplicationHeight - dot.Height)).ToArray();
            var positionsWithRespectToDotHeight =
                yPositionsInBoundary.Where(x => x % (int)dot.Height == 0).ToArray();

            var randomIndex = GameSettings.rnd.Next(0, positionsWithRespectToDotHeight.Length);
            dot.Y = positionsWithRespectToDotHeight[randomIndex];

            if (this.wave == GameSettings.Wave.West)
            {
                dot.X = -dot.Width;
            }
            else
            {
                dot.X = this.canvas.Width;
            }
        }

        private void randomlyPositionUpAndDownDirectionalDots(Dot dot)
        {
            var xPositionsInBoundary = Enumerable.Range(0, (int)(GameSettings.ApplicationWidth - dot.Width)).ToArray();
            var positionsWithRespectToDotWidth = xPositionsInBoundary.Where(x => x % (int)dot.Width == 0).ToArray();

            var randomIndex = GameSettings.rnd.Next(0, positionsWithRespectToDotWidth.Length);
            dot.X = positionsWithRespectToDotWidth[randomIndex];

            if (this.wave == GameSettings.Wave.North)
            {
                dot.Y = -dot.Height;
            }
            else
            {
                dot.Y = this.canvas.Height;
            }
        }

        private void moveDots()
        {
            foreach (var dot in this)
            {
                dot.Move();
            }
        }

        private Dot checkDotsOffCanvas()
        {
            foreach (var dot in this)
            {
                if (dot.X >= this.canvas.Width || dot.Y >= this.canvas.Height || dot.X < -1 * dot.Width ||
                    dot.Y < -1 * dot.Height)
                {
                    dot.Sprite.Visibility = Visibility.Collapsed;
                    return dot;
                }
            }

            return null;
        }

        private void removeDot(Dot dot)
        {
            this.dots.Remove(dot);
        }

        #endregion
    }
}