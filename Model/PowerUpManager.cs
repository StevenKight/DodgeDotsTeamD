using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Defines how the power up will display and act.
    /// </summary>
    public class PowerUpManager
    {
        #region Data members

        private const int MinRandomSpawnTick = 80;
        private const int MaxRandomSpawnTick = 400;

        private readonly Canvas backgroundCanvas;
        private readonly AudioPlayer audioPlayer;

        private readonly DispatcherTimer timer;
        private int tickCount;
        private readonly int randomSpawnTick;

        #endregion

        #region Properties

        /// <summary>
        ///     The power up object.
        /// </summary>
        public PowerUpObject PowerUp { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new <see cref="PowerUpManager" />
        ///     TODO Add movement
        /// </summary>
        /// <param name="background">The canvas to display the power-up on.</param>
        public PowerUpManager(Canvas background)
        {
            this.backgroundCanvas = background;
            this.audioPlayer = new AudioPlayer();

            var randomSpawnNext = GameSettings.rnd.Next(MinRandomSpawnTick, MaxRandomSpawnTick);
            this.randomSpawnTick = randomSpawnNext;

            this.timer = new DispatcherTimer();
            this.timer.Tick += this.Timer_Tick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            this.timer.Start();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Removes all point objects from tacking and canvas.
        /// </summary>
        public void StopPowerUpManager()
        {
            if (this.PowerUp?.Sprite != null)
            {
                this.backgroundCanvas.Children.Remove(this.PowerUp.Sprite);
            }

            this.timer.Stop();
        }

        private void Timer_Tick(object sender, object e)
        {
            this.tickCount++;

            if (this.tickCount == this.randomSpawnTick)
            {
                if (this.tickCount != 0)
                {
                    _ = this.createRandomPowerUpAsync();
                }
            }
            else
            {
                if (this.isOnScreen())
                {
                    this.PowerUp?.Move();
                }
                else
                {
                    this.StopPowerUpManager();
                }
            }
        }

        private bool isOnScreen()
        {
            if (this.PowerUp == null)
            {
                return true;
            }

            var xSideOne = this.PowerUp.X - this.PowerUp.SpeedX >= 0;
            var xSideTwo = this.PowerUp.X + this.PowerUp.SpeedX <=
                           this.backgroundCanvas.Width - this.PowerUp.Width;

            var ySideOne = this.PowerUp.Y - this.PowerUp.SpeedY >= 0;
            var ySideTwo = this.PowerUp.Y + this.PowerUp.SpeedY <=
                           this.backgroundCanvas.Height - this.PowerUp.Height;

            return xSideOne && xSideTwo && ySideOne && ySideTwo;
        }

        private async Task createRandomPowerUpAsync()
        {
            this.PowerUp = new PowerUpObject();

            var randomXPosition = GameSettings.rnd.Next(0, (int)(this.backgroundCanvas.Width - this.PowerUp.Width) + 1);
            var randomYPosition =
                GameSettings.rnd.Next(0, (int)(this.backgroundCanvas.Height - this.PowerUp.Height) + 1);

            this.setupPowerUp(randomXPosition, randomYPosition);

            Canvas.SetZIndex(this.PowerUp.Sprite, 0);

            this.backgroundCanvas.Children.Add(this.PowerUp.Sprite);

            var file = await this.audioPlayer.AudioFolder.Result.GetFileAsync("PowerUpAppears.wav");
            this.audioPlayer.PlayAudio(file);
        }

        private void setupPowerUp(int randomXPosition, int randomYPosition)
        {
            var waveSide = GameSettings.rnd.Next(0, 5);
            this.placePowerUp(randomXPosition, randomYPosition, waveSide);
            this.setSpeedPowerUp(waveSide);

            while (!this.isOnScreen())
            {
                this.PowerUp.Move();
            }
        }

        private void setSpeedPowerUp(int waveSide)
        {
            var randXSpeed = GameSettings.rnd.Next(1, 5);
            var randYSpeed = GameSettings.rnd.Next(1, 5);

            switch (waveSide)
            {
                case 1:
                    this.PowerUp.SetPowerUpSpeed(0, randYSpeed);
                    break;
                case 2:
                    this.PowerUp.SetPowerUpSpeed(0, -randYSpeed);
                    break;
                case 3:
                    this.PowerUp.SetPowerUpSpeed(-randXSpeed, 0);
                    break;
                case 4:
                    this.PowerUp.SetPowerUpSpeed(randXSpeed, 0);
                    break;
            }
        }

        private void placePowerUp(int randomXPosition, int randomYPosition, int waveSide)
        {
            switch (waveSide)
            {
                case 1:
                    this.PowerUp.X = randomXPosition;
                    this.PowerUp.Y = -this.PowerUp.Height;
                    break;
                case 2:
                    this.PowerUp.X = randomXPosition;
                    this.PowerUp.Y = GameSettings.ApplicationHeight;
                    break;
                case 3:
                    this.PowerUp.Y = randomYPosition;
                    this.PowerUp.X = GameSettings.ApplicationWidth;
                    break;
                case 4:
                    this.PowerUp.Y = randomYPosition;
                    this.PowerUp.X = -this.PowerUp.Width;
                    break;
            }
        }

        #endregion
    }
}