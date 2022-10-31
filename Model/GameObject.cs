using Windows.Foundation;
using DodgeDots.View.Sprites;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Defines basics of every game object.
    /// </summary>
    public abstract class GameObject
    {
        #region Data members

        private Point location;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the x location of the game object.
        /// </summary>
        /// <value>
        ///     The x.
        /// </value>
        public double X
        {
            get => this.location.X;
            set
            {
                this.location.X = value;
                this.render();
            }
        }

        /// <summary>
        ///     Gets or sets the y location of the game object.
        /// </summary>
        /// <value>
        ///     The y.
        /// </value>
        public double Y
        {
            get => this.location.Y;
            set
            {
                this.location.Y = value;
                this.render();
            }
        }

        /// <summary>
        ///     Gets the x speed of the game object.
        /// </summary>
        /// <value>
        ///     The speed x.
        /// </value>
        public int SpeedX { get; private set; }

        /// <summary>
        ///     Gets the y speed of the game object.
        /// </summary>
        /// <value>
        ///     The speed y.
        /// </value>
        public int SpeedY { get; private set; }

        /// <summary>
        ///     Gets the width of the game object.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        public double Width => this.Sprite.Width;

        /// <summary>
        ///     Gets the height of the game object.
        /// </summary>
        /// <value>
        ///     The height.
        /// </value>
        public double Height => this.Sprite.Height;

        /// <summary>
        ///     Gets or sets the sprite associated with the game object.
        /// </summary>
        /// <value>
        ///     The sprite.
        /// </value>
        public BaseSprite Sprite { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Moves the game object down.
        ///     Precondition: None
        ///     Post-condition: Y == Y@prev + SpeedY
        /// </summary>
        public void Move()
        {
            this.moveX(this.SpeedX);
            this.moveY(this.SpeedY);
        }

        private void moveX(int x)
        {
            this.X += x;
        }

        private void moveY(int y)
        {
            this.Y += y;
        }

        private void render()
        {
            var render = this.Sprite as ISpriteRenderer;

            render?.RenderAt(this.X, this.Y);
        }

        /// <summary>
        ///     Sets the speed of the game object.
        ///     Precondition: speedX >= 0 AND speedY >=0
        ///     Post-condition: SpeedX == speedX AND SpeedY == speedY
        /// </summary>
        /// <param name="speedX">The speed x.</param>
        /// <param name="speedY">The speed y.</param>
        protected void SetSpeed(int speedX, int speedY)
        {
            this.SpeedX = speedX;
            this.SpeedY = speedY;
        }

        #endregion
    }
}