using Windows.UI;
using Windows.UI.Xaml.Media;
using DodgeDots.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace DodgeDots.View.Sprites
{
    /// <summary>
    ///     Draws a enemy Dot object
    /// </summary>
    /// <seealso cref="DodgeDots.View.Sprites.BaseSprite" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class DotSprite
    {
        #region Properties

        /// <summary>
        ///     Gets the color of the dot sprite.
        /// </summary>
        /// <value>
        ///     The color of the dot sprite.
        /// </value>
        public Color DotColor { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="DotSprite" /> class.
        /// </summary>
        public DotSprite()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Changes the dot sprite color to primary color.
        ///     Precondition: none
        ///     Post-condition: DotSprite Fill set to the primary color
        /// </summary>
        public void ChangeDotColorToPrimary()
        {
            this.dotShape.Fill = new SolidColorBrush(GameSettings.PrimaryDotColor);
            this.DotColor = GameSettings.PrimaryDotColor;
        }

        /// <summary>
        ///     Changes the dot sprite color to secondary.
        ///     Precondition: none
        ///     Post-condition: DotSprite Fill set to the secondary color
        /// </summary>
        public void ChangeDotColorToSecondary()
        {
            this.dotShape.Fill = new SolidColorBrush(GameSettings.SecondaryDotColor);
            this.DotColor = GameSettings.SecondaryDotColor;
        }

        /// <summary>
        ///     Changes the dot sprite color to final blitz color.
        ///     Precondition: none
        ///     Post-condition: DotSprite Fill set to the final blitz color
        /// </summary>
        public void ChangeDotColorToFinalBlitzColor()
        {
            this.dotShape.Fill = new SolidColorBrush(GameSettings.FinalBlitzDotColor);
        }

        #endregion
    }
}