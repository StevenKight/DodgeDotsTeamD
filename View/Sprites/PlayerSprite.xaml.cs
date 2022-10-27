// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using DodgeDots.Model;

namespace DodgeDots.View.Sprites
{
    /// <summary>
    ///     Draws the play object.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    public sealed partial class PlayerSprite
    {
        #region Data members

        private bool isOuterCirclePrimaryColor;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the color of the outer circle of the player sprite.
        /// </summary>
        /// <value>
        ///     The color of the outer circle of the player sprite.
        /// </value>
        public Color OuterCircleColor { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerSprite" /> class.
        ///     Precondition: none
        ///     Post-condition: Sprite created.
        /// </summary>
        public PlayerSprite()
        {
            this.InitializeComponent();
            this.swapOuterCircleToPrimaryColorAndInnerCircleToSecondaryColor();
            Window.Current.CoreWindow.KeyDown += this.CoreWindow_KeyDown;
        }

        #endregion

        #region Methods

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Space)
            {
                if (this.isOuterCirclePrimaryColor)
                {
                    this.swapOuterCircleToSecondaryColorAndInnerCircleToPrimaryColor();
                }
                else
                {
                    this.swapOuterCircleToPrimaryColorAndInnerCircleToSecondaryColor();
                }
            }
        }

        private void swapOuterCircleToPrimaryColorAndInnerCircleToSecondaryColor()
        {
            this.outerCircle.Fill = new SolidColorBrush(GameSettings.PrimaryDotColor);
            this.innerCircle.Fill = new SolidColorBrush(GameSettings.SecondaryDotColor);
            this.isOuterCirclePrimaryColor = true;
            this.OuterCircleColor = GameSettings.PrimaryDotColor;
        }

        private void swapOuterCircleToSecondaryColorAndInnerCircleToPrimaryColor()
        {
            this.outerCircle.Fill = new SolidColorBrush(GameSettings.SecondaryDotColor);
            this.innerCircle.Fill = new SolidColorBrush(GameSettings.PrimaryDotColor);
            this.isOuterCirclePrimaryColor = false;
            this.OuterCircleColor = GameSettings.SecondaryDotColor;
        }

        #endregion
    }
}