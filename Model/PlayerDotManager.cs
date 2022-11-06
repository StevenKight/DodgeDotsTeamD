using System;
using Windows.System;
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

        private readonly Canvas backgroundCanvas;
        private readonly DispatcherTimer playerTimer;

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
            this.PlacePlayerCenteredInGameArena();

            Window.Current.CoreWindow.KeyDown += this.playerKeyDownFlag;
            Window.Current.CoreWindow.KeyUp += this.playerKeyUpFlag;

            this.playerTimer = new DispatcherTimer();
            this.playerTimer.Tick += this.PlayerTimer_Tick;
            this.playerTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            this.playerTimer.Start();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Place the player in the center of the display.
        /// </summary>
        public void PlacePlayerCenteredInGameArena()
        {
            this.PlayerDot.X = this.backgroundCanvas.Width / 2 - this.PlayerDot.Width / 2.0;
            this.PlayerDot.Y = this.backgroundCanvas.Height / 2 - this.PlayerDot.Height / 2.0;
        }

        /// <summary>
        ///     Stops the player movement.
        /// </summary>
        public void StopPlayer()
        {
            this.playerTimer.Stop();
        }

        /// <summary>
        ///     Restarts the player movement.
        /// </summary>
        public void RestartPlayer()
        {
            this.playerTimer.Start();
        }

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
            this.PlayerDot.SwapOuterColor();
        }

        #endregion
    }
}