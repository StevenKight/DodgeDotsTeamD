using Windows.UI.Xaml.Controls;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Manages the Player Dot in the game.
    /// </summary>
    public class PlayerDotManager
    {
        #region Data members

        private readonly Canvas backgroundCanvas;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the player dot.
        /// </summary>
        /// <value>
        ///     The player dot.
        /// </value>
        public Player PlayerDot { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerDotManager" /> class.
        /// </summary>
        /// <param name="background">The background.</param>
        public PlayerDotManager(Canvas background)
        {
            this.PlayerDot = new Player();
            this.backgroundCanvas = background;
            background.Children.Add(this.PlayerDot.Sprite);
            this.placePlayerCenteredInGameArena();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Moves the player to the left, if player is not a the left border.
        ///     Precondition: none
        ///     Post-condition: The player has moved left.
        /// </summary>
        public void MovePlayerLeft()
        {
            if (this.PlayerDot.X - this.PlayerDot.SpeedX >= 0)
            {
                this.PlayerDot.MoveLeft();
            }
        }

        /// <summary>
        ///     Moves the player to the right only if the player is not at the right-side border.
        ///     Precondition: none
        ///     Post-condition: The player has moved right.
        /// </summary>
        public void MovePlayerRight()
        {
            if (this.PlayerDot.X + this.PlayerDot.SpeedX <= this.backgroundCanvas.Width - this.PlayerDot.Width)
            {
                this.PlayerDot.MoveRight();
            }
        }

        /// <summary>
        ///     Moves the player up, only if player is not at the top-side border.
        ///     Precondition: none
        ///     Post-condition: The player has moved up.
        /// </summary>
        public void MovePlayerUp()
        {
            if (this.PlayerDot.Y - this.PlayerDot.SpeedY >= 0)
            {
                this.PlayerDot.MoveUp();
            }
        }

        /// <summary>
        ///     Moves the player down, only if player is not at the bottom-side border.
        ///     Precondition: none
        ///     Post-condition: The player has moved down.
        /// </summary>
        public void MovePlayerDown()
        {
            if (this.PlayerDot.Y + this.PlayerDot.SpeedY <= this.backgroundCanvas.Height - this.PlayerDot.Height)
            {
                this.PlayerDot.MoveDown();
            }
        }

        private void placePlayerCenteredInGameArena()
        {
            this.PlayerDot.X = this.backgroundCanvas.Width / 2 - this.PlayerDot.Width / 2.0;
            this.PlayerDot.Y = this.backgroundCanvas.Height / 2 - this.PlayerDot.Height / 2.0;
        }

        #endregion
    }
}