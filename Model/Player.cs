using System;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml;
using DodgeDots.View.Sprites;

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
        private const int TicksForAnimation = (int)(GameSettings.DyingAnimationLength * TickModifier);

        private readonly DispatcherTimer timer;
        private int tickCount;

        private int currentColorIndex;

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
        ///     TODO Potentially move out to PlayerManager
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

            this.timer = new DispatcherTimer();
            this.timer.Tick += this.Timer_Tick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            this.tickCount = 0;
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

            var lastIndex = this.Colors.Count - 1;
            this.PlayerSprite.ChangeDotInnerColor(this.Colors[lastIndex]);
        }

        /// <summary>
        ///     Sets the color of the player.
        /// </summary>
        public void SwapOuterColor()
        {
            this.currentColorIndex++;
            if (this.currentColorIndex >= this.Colors.Count)
            {
                this.currentColorIndex = 0;
            }

            this.PlayerSprite.ChangeDotInnerColor(this.OuterColor);

            var newColor = this.Colors[this.currentColorIndex];
            this.PlayerSprite.ChangeDotOuterColor(newColor);
            this.OuterColor = newColor;
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
        ///     Starts the dying animation timer.
        /// </summary>
        public void DyingAnimation()
        {
            this.timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            this.tickCount++;

            this.PlayerSprite.DyingAnimation();

            if (this.tickCount > TicksForAnimation)
            {
                this.timer.Stop();
            }
        }

        #endregion
    }
}