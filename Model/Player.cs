using DodgeDots.View.Sprites;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Manages the player.
    /// </summary>
    /// <seealso cref="DodgeDots.Model.GameObject" />
    public class Player : GameObject
    {
        #region Data members

        private const int SpeedXDirection = 3;
        private const int SpeedYDirection = 3;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        public Player()
        {
            Sprite = new PlayerSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
        }

        #endregion
    }
}