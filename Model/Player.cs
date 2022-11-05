using System;
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
        ///     Gets the color of the inner circle.
        /// </summary>
        /// <value>
        ///     The color of the inner.
        /// </value>
        public Color InnerColor { get; private set; }

        /// <summary>
        ///     Gets the players sprite object.
        /// </summary>
        public PlayerSprite PlayerSprite { get; }

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

            this.timer = new DispatcherTimer();
            this.timer.Tick += this.Timer_Tick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            this.tickCount = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the color of the player.
        /// </summary>
        /// <param name="color">The color.</param>
        public void SetOuterColor(Color color)
        {
            this.PlayerSprite.ChangeDotOuterColor(color);
            this.OuterColor = color;
        }

        /// <summary>
        ///     Sets the color of the player.
        /// </summary>
        /// <param name="color">The color.</param>
        public void SetInnerColor(Color color)
        {
            this.PlayerSprite.ChangeDotInnerColor(color);
            this.InnerColor = color;
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