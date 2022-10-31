using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
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

        #region Properties

        /// <summary>
        ///     Gets the color of the outer circle.
        /// </summary>
        /// <value>
        ///     The color of the outer.
        /// </value>
        public Color OuterColor { get; private set; }

        /// <summary>
        ///     Gets the color of the inner circle.
        /// </summary>
        /// <value>
        ///     The color of the inner.
        /// </value>
        public Color InnerColor { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Player" /> class.
        /// </summary>
        public Player()
        {
            Sprite = new PlayerSprite();
            SetSpeed(SpeedXDirection, SpeedYDirection);
            Window.Current.CoreWindow.KeyDown += this.coreWindowOnKeyDown;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the color of the player.
        /// </summary>
        /// <param name="color">The color.</param>
        public void SetOuterColor(Color color)
        {
            var player = (PlayerSprite)Sprite;
            player.ChangeDotOuterColor(color);
            this.OuterColor = color;
        }

        /// <summary>
        ///     Sets the color of the player.
        /// </summary>
        /// <param name="color">The color.</param>
        public void SetInnerColor(Color color)
        {
            var player = (PlayerSprite)Sprite;
            player.ChangeDotInnerColor(color);
            this.InnerColor = color;
        }

        private void coreWindowOnKeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    SetSpeed(-1 * SpeedXDirection, 0);
                    Move();
                    break;
                case VirtualKey.Right:
                    SetSpeed(SpeedXDirection, 0);
                    Move();
                    break;
                case VirtualKey.Up:
                    SetSpeed(0, -1 * SpeedYDirection);
                    Move();
                    break;
                case VirtualKey.Down:
                    SetSpeed(0, SpeedYDirection);
                    Move();
                    break;
            }
        }

        #endregion
    }
}