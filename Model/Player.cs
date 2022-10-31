using Windows.UI;
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

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        public Player()
        {
            Sprite = new PlayerSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the color of the player.
        /// </summary>
        /// <param name="color">The color.</param>
        public void SetOuterColor(Color color)
        {
            var player = (PlayerSprite)Sprite;
            player.ChangeDotOuterColor(color);
            this.OuterColor = color;
        }

        /// <summary>
        ///     Sets the color of the player.
        /// </summary>
        /// <param name="color">The color.</param>
        public void SetInnerColor(Color color)
        {
            var player = (PlayerSprite)Sprite;
            player.ChangeDotInnerColor(color);
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

        #endregion
    }
}