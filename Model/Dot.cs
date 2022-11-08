using Windows.UI;
using DodgeDots.View.Sprites;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Manages the dot
    /// </summary>
    /// <seealso cref="DodgeDots.Model.GameObject" />
    public class Dot : GameObject
    {
        #region Data members

        private const int MinimumSpeed = 1;
        private const int MaximumSpeed = 5;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the color of the dot.
        /// </summary>
        /// <value>
        ///     The color.
        /// </value>
        public Color Color { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Dot" /> class.
        /// </summary>
        public Dot()
        {
            Sprite = new DotSprite();
            this.setDotSpeed();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the color of the dot.
        /// </summary>
        /// <param name="color">The color.</param>
        public void SetColor(Color color)
        {
            var dot = (DotSprite)Sprite;
            dot.ChangeDotColor(color);
            this.Color = color;
        }

        private void setDotSpeed()
        {
            var randomSpeed = GameSettings.rnd.Next(MinimumSpeed, MaximumSpeed + 1);
            SetSpeed(randomSpeed, randomSpeed);
        }

        #endregion
    }
}