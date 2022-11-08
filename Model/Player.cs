using System;
using System.Collections.ObjectModel;
using System.Drawing;
using Windows.UI.Xaml;
using DodgeDots.View.Sprites;
using Color = Windows.UI.Color;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Manages the player.
    /// </summary>
    /// <seealso cref="DodgeDots.Model.GameObject" />
    public class Player : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 3;

        private const double TickModifier = 7.5;
        private const int TicksForPowerUpAnimation = 10;

        private readonly DispatcherTimer timer;
        private int tickCount;
        private int ticksForAnimation;

        private int currentColorIndex;
        private bool playerDied;
        private bool poweredUp;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the color of the outer circle.
        /// </summary>
        /// <value>
        ///     The color of the outer.
        /// </value>
        public Color OuterColor { get; private set; }

        /// <summary>
        ///     A collection of colors the player can swap through.
        /// </summary>
        /// <value>
        ///     A collection of colors.
        /// </value>
        public Collection<Color> Colors { get; set; }

        /// <summary>
        ///     Gets the players sprite object.
        /// </summary>
        public PlayerSprite PlayerSprite { get; }

        /// <summary>
        ///     The amount of lives the player has in the game.
        /// </summary>
        public int PlayerLives { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        public Player()
        {
            Sprite = new PlayerSprite();
            this.PlayerSprite = (PlayerSprite)Sprite;
            SetSpeed(SpeedXDirection, SpeedYDirection);

            this.PlayerLives = GameSettings.PlayerLives;

            this.currentColorIndex = 0;
            this.playerDied = false;
            this.poweredUp = false;

            this.timer = new DispatcherTimer();
            this.timer.Tick += this.Timer_Tick;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Rotates through to the next set of colors the player has.
        /// </summary>
        public void SetColors()
        {
            this.PlayerSprite.ChangeDotOuterColor(this.Colors[0]);
            this.OuterColor = this.Colors[0];

            Color innerColor;
            if (this.Colors.Count > 1)
            {
                innerColor = this.Colors[1];
            }
            else
            {
                innerColor = this.Colors[0];
            }

            this.PlayerSprite.ChangeDotInnerColor(innerColor);
        }

        /// <summary>
        ///     Sets the color of the player.
        /// </summary>
        public void SwapNextOuterColor()
        {
            this.currentColorIndex++;
            if (this.currentColorIndex >= this.Colors.Count)
            {
                this.currentColorIndex = 0;
            }

            var newColor = this.Colors[this.currentColorIndex];
            this.PlayerSprite.ChangeDotOuterColor(newColor);
            this.OuterColor = newColor;

            var nextIndex = this.currentColorIndex + 1;
            if (nextIndex >= this.Colors.Count)
            {
                nextIndex = 0;
            }

            var nextColor = this.Colors[nextIndex];
            this.PlayerSprite.ChangeDotInnerColor(nextColor);
        }

        /// <summary>
        ///     Moves the player left.
        /// </summary>
        public void MoveLeft()
        {
            SetSpeed(-1 * SpeedXDirection, 0);
            Move();
        }

        /// <summary>
        ///     Moves the player right.
        /// </summary>
        public void MoveRight()
        {
            SetSpeed(SpeedXDirection, 0);
            Move();
        }

        /// <summary>
        ///     Moves the player up.
        /// </summary>
        public void MoveUp()
        {
            SetSpeed(0, -1 * SpeedYDirection);
            Move();
        }

        /// <summary>
        ///     Moves the player down.
        /// </summary>
        public void MoveDown()
        {
            SetSpeed(0, SpeedYDirection);
            Move();
        }

        /// <summary>
        ///     Tests if the player overlaps the given GameObject.
        /// </summary>
        /// <param name="dot">The GameObject to test overlap of.</param>
        /// <returns>
        ///     <value>True</value>
        ///     if overlap and
        ///     <value>false</value>
        ///     otherwise.
        /// </returns>
        public bool IsCircleOverlapPlayer(GameObject dot)
        {
            var playerRectangle = new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
            var dotRectangle = new Rectangle((int)dot.X, (int)dot.Y, (int)dot.Width, (int)dot.Height);

            var radiusSum = playerRectangle.Width / 2.0 + dotRectangle.Width / 2.0;
            var deltaX = playerRectangle.X - dotRectangle.X;
            var deltaY = playerRectangle.Y - dotRectangle.Y;

            return deltaX * deltaX + deltaY * deltaY <= radiusSum * radiusSum;
        }

        /// <summary>
        ///     Tests if the player and the given GameObject have the same color.
        /// </summary>
        /// <param name="dot">The GameObject to test color of.</param>
        /// <returns>
        ///     <value>True</value>
        ///     if colors are the same and
        ///     <value>false</value>
        ///     otherwise.
        /// </returns>
        public bool IsColorSameAsPlayer(Dot dot)
        {
            return this.OuterColor == dot.Color;
        }

        /// <summary>
        ///     Starts the dying animation timer.
        /// </summary>
        public void DyingAnimation()
        {
            this.ticksForAnimation = (int)(GameSettings.DyingAnimationLength * TickModifier);
            this.playerDied = true;
            this.poweredUp = false;

            this.setupTimer(100);
        }

        /// <summary>
        ///     Starts the dying animation timer.
        /// </summary>
        public void PowerUpAnimation()
        {
            this.ticksForAnimation = TicksForPowerUpAnimation;
            this.poweredUp = true;
            this.playerDied = false;

            this.setupTimer(500);
        }

        private void setupTimer(int milliseconds)
        {
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, milliseconds);
            this.tickCount = 0;
            this.timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            this.tickCount++;

            if (this.playerDied)
            {
                this.PlayerSprite.DyingAnimation();
            }

            if (this.poweredUp)
            {
                if (this.tickCount % 2 == 0)
                {
                    this.PlayerSprite.ResetPlayerSize();
                }
                else
                {
                    this.PlayerSprite.IncreasePlayerSize();
                }
            }

            if (this.tickCount > this.ticksForAnimation)
            {
                this.timer.Stop();
                this.tickCount = 0;
                this.PlayerSprite.ResetPlayerSize();
            }
        }

        #endregion
    }
}