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

        #endregion
    }
}