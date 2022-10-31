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
            Window.Current.CoreWindow.KeyDown += this.CoreWindow_KeyDown;
            this.colors = new[]
            {
                GameSettings.PrimaryDotColor, GameSettings.SecondaryDotColor
            };
        }

        #endregion

        #region Methods

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Space && this.ColorSwapLevel == 1)
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