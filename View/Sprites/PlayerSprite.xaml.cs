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
        #region Data members

        private const double CircleModifier = 1.25;

        private readonly double innerCircleWidth;
        private readonly double innerCircleHeight;
        private readonly double outerCircleWidth;
        private readonly double outerCircleHeight;

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

            this.innerCircleWidth = this.innerCircle.Width;
            this.innerCircleHeight = this.innerCircle.Height;
            this.outerCircleWidth = this.outerCircle.Width;
            this.outerCircleHeight = this.outerCircle.Height;
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

        /// <summary>
        ///     Runs the dying animation of the player dot.
        /// </summary>
        public void DyingAnimation()
        {
            (this.innerCircle.Fill, this.outerCircle.Fill) = (this.outerCircle.Fill, this.innerCircle.Fill);
        }

        /// <summary>
        ///     Increases player sprite size by a factor of 1.25.
        /// </summary>
        public void IncreasePlayerSize()
        {
            this.innerCircle.Width *= CircleModifier;
            this.innerCircle.Height *= CircleModifier;

            this.outerCircle.Width *= CircleModifier;
            this.outerCircle.Height *= CircleModifier;
        }

        /// <summary>
        ///     Resets the player size back to default.
        /// </summary>
        public void ResetPlayerSize()
        {
            this.innerCircle.Width = this.innerCircleWidth;
            this.innerCircle.Height = this.innerCircleHeight;

            this.outerCircle.Width = this.outerCircleWidth;
            this.outerCircle.Height = this.outerCircleHeight;
        }

        #endregion
    }
}