using Windows.UI;
using Windows.UI.Xaml.Media;

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
        ///     Gets the color of the filter.
        /// </summary>
        /// <value>
        ///     The color of the filter.
        /// </value>
        public Color FilterColor { get; private set; }

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
        ///     Changes the color of the dot to the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        public void ChangeDotColor(Color color)
        {
            this.dotShape.Fill = new SolidColorBrush(color);
        }

        /// <summary>
        ///     Sets the weak mode.
        /// </summary>
        /// <param name="isSet">if set to <c>true</c> [is set].</param>
        /// <param name="weakModePrimaryColor">Primary weak mode color.</param>
        public void SetWeakMode(bool isSet, Color weakModePrimaryColor)
        {
            this.filter.Fill = new SolidColorBrush(weakModePrimaryColor);
            this.FilterColor = weakModePrimaryColor;
            if (isSet)
            {
                this.filter.Opacity = 100;
            }
            else
            {
                this.filter.Opacity = 0;
            }
        }

        /// <summary>
        ///     Swaps the the weak mode color.
        /// </summary>
        /// <param name="weakModePrimaryColor">Primary weak mode color.</param>
        /// <param name="weakModeSecondaryColor">Secondary weak mode color.</param>
        public void SwapWeakColor(Color weakModePrimaryColor, Color weakModeSecondaryColor)
        {
            if (this.FilterColor == weakModePrimaryColor)
            {
                this.filter.Fill = new SolidColorBrush(weakModeSecondaryColor);
                this.FilterColor = weakModeSecondaryColor;
            }
            else
            {
                this.filter.Fill = new SolidColorBrush(weakModePrimaryColor);
                this.FilterColor = weakModePrimaryColor;
            }
        }

        #endregion
    }
}