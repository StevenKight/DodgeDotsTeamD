using System;
using Windows.UI;
using DodgeDots.View.Sprites;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Point Object class.
    /// </summary>
    /// <seealso cref="DodgeDots.Model.GameObject" />
    public class PointObject : GameObject
    {
        #region Properties

        /// <summary>
        ///     Gets the point amount.
        /// </summary>
        /// <value>
        ///     The point amount.
        /// </value>
        public int PointAmount { get; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PointObject" /> class.
        ///     precondition: pointAmount > 0
        ///     post-condition: PointAmount == pointAmount
        /// </summary>
        public PointObject(int pointAmount, Color color)
        {
            Sprite = new PointSprite(color);

            if (pointAmount < 0)
            {
                throw new ArgumentException("Point amount must be greater than 0");
            }

            this.PointAmount = pointAmount;
        }

        #endregion
    }
}