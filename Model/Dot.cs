using System;
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
        private const int FinalBlitzSpeedMultiplier = 2;
        private int originalSpeed;

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
        ///     Sets the dot speed to SpeedMultiplier times the original dot speed.
        /// </summary>
        public void SetDotSpeedToFinalBlitzSpeed()
        {
            SetSpeed(this.originalSpeed * FinalBlitzSpeedMultiplier, this.originalSpeed * FinalBlitzSpeedMultiplier);
        }

        /// <summary>
        ///     Sets the dot speed to the original dot speed.
        /// </summary>
        public void SetDotSpeedToOriginalSpeed()
        {
            SetSpeed(this.originalSpeed, this.originalSpeed);
        }

        private void setDotSpeed()
        {
            var rnd = new Random();
            var randomSpeed = rnd.Next(MinimumSpeed, MaximumSpeed + 1);
            SetSpeed(randomSpeed, randomSpeed);
            this.originalSpeed = randomSpeed;
        }

        #endregion
    }
}