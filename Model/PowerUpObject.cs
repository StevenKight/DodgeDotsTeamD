using DodgeDots.View.Sprites;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Creates the Power Up
    /// </summary>
    public class PowerUpObject : GameObject
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new <see cref="PowerUpObject" />
        /// </summary>
        public PowerUpObject()
        {
            Sprite = new PowerUpSprite();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the speed of the power-up
        /// </summary>
        /// <param name="speedX">Speed in the x direction.</param>
        /// <param name="speedY">Speed in the y direction.</param>
        public void SetPowerUpSpeed(int speedX, int speedY)
        {
            SetSpeed(speedX, speedY);
        }

        #endregion
    }
}