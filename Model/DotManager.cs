using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DodgeDots.View.Sprites;

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
        private readonly GameSettings.Direction direction;
        private bool isFinalBlitz;

        private readonly DispatcherTimer timer;
        private int tickCount;
        private int randomSpawnTick;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DotManager" /> class.
        ///     Precondition: None
        ///     Post-condition: dots list is created, and tick is setup
        /// </summary>
        /// <param name="background">The background.</param>
        /// <param name="direction">The direction.</param>
        public DotManager(Canvas background, GameSettings.Direction direction)
        {
            this.dots = new List<Dot>();
            this.canvas = background;
            this.direction = direction;
            this.isFinalBlitz = false;
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

        private void Timer_Tick(object sender, object e)
        {
            if (this.tickCount == this.randomSpawnTick)
            {
                this.createDot();
                this.tickCount = 0;
                var rnd = new Random();
                var randomSpawnNext = rnd.Next(20, 40);
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
            var dot = new Dot();
            var dotSprite = (DotSprite)dot.Sprite;
            if (this.isFinalBlitz)
            {
                dot.SetDotSpeedToFinalBlitzSpeed();
            }

            this.setDotColor(dotSprite);
            this.dots.Add(dot);
            this.canvas.Children.Add(dot.Sprite);
            this.setDotInPosition(dot);
        }

        private void setDotColor(DotSprite dotSprite)
        {
            if (!this.isFinalBlitz)
            {
                switch (this.direction)
                {
                    case GameSettings.Direction.Left:
                    case GameSettings.Direction.Right:
                        dotSprite.ChangeDotColorToSecondary();
                        break;
                    case GameSettings.Direction.Up:
                    case GameSettings.Direction.Down:
                        dotSprite.ChangeDotColorToPrimary();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                dotSprite.ChangeDotColorToFinalBlitzColor();
            }
        }

        private void setDotInPosition(Dot dot)
        {
            switch (this.direction)
            {
                case GameSettings.Direction.Up:
                case GameSettings.Direction.Down:
                    this.randomlyPositionUpAndDownDirectionalDots(dot);
                    break;
                case GameSettings.Direction.Left:
                case GameSettings.Direction.Right:
                    this.randomlyPositionLeftAndRightDirectionalDots(dot);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void randomlyPositionLeftAndRightDirectionalDots(Dot dot)
        {
            var yPositionsInBoundary = Enumerable.Range(0, (int)(this.canvas.Height - dot.Height)).ToArray();
            var positionsWithRespectToDotHeight =
                yPositionsInBoundary.Where(x => x % (int)dot.Height == 0).ToArray();
            var rnd = new Random();
            var randomIndex = rnd.Next(0, positionsWithRespectToDotHeight.Length);
            dot.Y = positionsWithRespectToDotHeight[randomIndex];

            if (this.direction == GameSettings.Direction.Right)
            {
                dot.X = -1 * dot.Width;
            }
            else
            {
                dot.X = this.canvas.Width;
            }
        }

        private void randomlyPositionUpAndDownDirectionalDots(Dot dot)
        {
            var xPositionsInBoundary = Enumerable.Range(0, (int)(this.canvas.Width - dot.Width)).ToArray();
            var positionsWithRespectToDotWidth = xPositionsInBoundary.Where(x => x % (int)dot.Width == 0).ToArray();
            var rnd = new Random();
            var randomIndex = rnd.Next(0, positionsWithRespectToDotWidth.Length);
            dot.X = positionsWithRespectToDotWidth[randomIndex];

            if (this.direction == GameSettings.Direction.Down)
            {
                dot.Y = -1 * dot.Height;
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
                switch (this.direction)
                {
                    case GameSettings.Direction.Up:
                        dot.MoveUp();
                        break;
                    case GameSettings.Direction.Down:
                        dot.MoveDown();
                        break;
                    case GameSettings.Direction.Left:
                        dot.MoveLeft();
                        break;
                    case GameSettings.Direction.Right:
                        dot.MoveRight();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
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

        /// <summary>
        ///     Toggles the final blitz settings.
        /// </summary>
        public void ToggleFinalBlitz()
        {
            if (!this.isFinalBlitz)
            {
                this.isFinalBlitz = true;
                foreach (var dot in this)
                {
                    dot.SetDotSpeedToFinalBlitzSpeed();
                    var dotSprite = (DotSprite)dot.Sprite;
                    this.setDotColor(dotSprite);
                }
            }
            else
            {
                this.isFinalBlitz = false;
                foreach (var dot in this)
                {
                    dot.SetDotSpeedToOriginalSpeed();
                    var dotSprite = (DotSprite)dot.Sprite;
                    this.setDotColor(dotSprite);
                }
            }
        }

        private void removeDot(Dot dot)
        {
            this.dots.Remove(dot);
        }

        #endregion
    }
}