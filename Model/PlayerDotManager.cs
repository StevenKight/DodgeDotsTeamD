using System;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Manages the Player Dot in the game.
    /// </summary>
    public class PlayerDotManager
    {
        #region Data members

        /// <summary>
        ///     The player color swap ability level.
        /// </summary>
        public int ColorSwapLevel = 1;

        private readonly Canvas backgroundCanvas;
        private readonly Color[] colors;
        private int outsideColorIndex;
        private bool isUpKeyDown;
        private bool isDownKeyDown;
        private bool isLeftKeyDown;
        private bool isRightKeyDown;
        private bool isSpaceKeyDown;

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

            Window.Current.CoreWindow.KeyDown += this.playerKeyDownFlag;
            Window.Current.CoreWindow.KeyUp += this.playerKeyUpFlag;

            var playerTimer = new DispatcherTimer();
            playerTimer.Tick += this.PlayerTimer_Tick;
            playerTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            playerTimer.Start();

            this.colors = new[]
            {
                GameSettings.PrimaryDotColor, GameSettings.SecondaryDotColor
            };
        }

        #endregion

        #region Methods

        private void PlayerTimer_Tick(object sender, object e)
        {
            if (this.isLeftKeyDown && this.PlayerDot.X - this.PlayerDot.SpeedX >= 0)
            {
                this.PlayerDot.MoveLeft();
            }

            if (this.isRightKeyDown && this.PlayerDot.X + this.PlayerDot.SpeedX <=
                this.backgroundCanvas.Width - this.PlayerDot.Width)
            {
                this.PlayerDot.MoveRight();
            }

            if (this.isUpKeyDown && this.PlayerDot.Y - this.PlayerDot.SpeedY >= 0)
            {
                this.PlayerDot.MoveUp();
            }

            if (this.isDownKeyDown && this.PlayerDot.Y + this.PlayerDot.SpeedY <=
                this.backgroundCanvas.Height - this.PlayerDot.Height)
            {
                this.PlayerDot.MoveDown();
            }

            if (this.isSpaceKeyDown)
            {
                this.colorSwap();
                this.isSpaceKeyDown = false;
            }
        }

        private void playerKeyDownFlag(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.isLeftKeyDown = true;
                    break;
                case VirtualKey.Right:
                    this.isRightKeyDown = true;
                    break;
                case VirtualKey.Up:
                    this.isUpKeyDown = true;
                    break;
                case VirtualKey.Down:
                    this.isDownKeyDown = true;
                    break;
                case VirtualKey.Space:
                    this.isSpaceKeyDown = true;
                    break;
            }
        }

        private void playerKeyUpFlag(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    this.isLeftKeyDown = false;
                    break;
                case VirtualKey.Right:
                    this.isRightKeyDown = false;
                    break;
                case VirtualKey.Up:
                    this.isUpKeyDown = false;
                    break;
                case VirtualKey.Down:
                    this.isDownKeyDown = false;
                    break;
                case VirtualKey.Space:
                    this.isSpaceKeyDown = false;
                    break;
            }
        }

        private void colorSwap()
        {
            if (this.ColorSwapLevel == 1)
            {
                switch (this.outsideColorIndex)
                {
                    case 0:
                        this.PlayerDot.SetOuterColor(this.colors[1]);
                        this.PlayerDot.SetInnerColor(this.colors[0]);
                        this.outsideColorIndex++;
                        break;
                    case 1:
                        this.PlayerDot.SetOuterColor(this.colors[0]);
                        this.PlayerDot.SetInnerColor(this.colors[1]);
                        this.outsideColorIndex = 0;
                        break;
                }
            }
        }

        /// <summary>
        ///     Moves the player to the left, if player is not a the left border.
        ///     Precondition: none
        ///     Post-condition: The player has moved left.
        /// </summary>
        private void placePlayerCenteredInGameArena()
        {
            this.PlayerDot.X = this.backgroundCanvas.Width / 2 - this.PlayerDot.Width / 2.0;
            this.PlayerDot.Y = this.backgroundCanvas.Height / 2 - this.PlayerDot.Height / 2.0;
        }

        #endregion
    }
}