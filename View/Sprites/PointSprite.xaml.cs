// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Windows.UI;
using Windows.UI.Xaml.Media;

namespace DodgeDots.View.Sprites
{
    /// <summary>
    ///     The PointSprite CodeBehind
    /// </summary>
    /// <seealso cref="DodgeDots.View.Sprites.BaseSprite" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class PointSprite
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PointSprite" /> class.
        /// </summary>
        public PointSprite(Color filterColor)
        {
            this.InitializeComponent();

            this.filter.Fill = new SolidColorBrush(filterColor);
        }

        #endregion
    }
}