// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

using Windows.UI;
using Windows.UI.Xaml.Media;

namespace DodgeDots.View.Sprites
{
    /// <summary>
    ///     Draws the play object.
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    public sealed partial class PlayerSprite
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerSprite" /> class.
        ///     Precondition: none
        ///     Post-condition: Sprite created.
        /// </summary>
        public PlayerSprite()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Changes the color of the outer dot.
        /// </summary>
        /// <param name="color">The color.</param>
        public void ChangeDotOuterColor(Color color)
        {
            this.outerCircle.Fill = new SolidColorBrush(color);
        }

        /// <summary>
        ///     Changes the color of the inner dot.
        /// </summary>
        /// <param name="color">The color.</param>
        public void ChangeDotInnerColor(Color color)
        {
            this.innerCircle.Fill = new SolidColorBrush(color);
        }

        #endregion
    }
}