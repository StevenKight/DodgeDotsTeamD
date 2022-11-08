using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Manages the different levels and level settings inside the game.
    /// </summary>
    public class LevelManager
    {
        #region Types and Delegates

        /// <summary>
        ///     A delegate for losing the game.
        /// </summary>
        public delegate void GameLostHandler();

        /// <summary>
        ///     A delegate for the countdown
        /// </summary>
        /// <param name="countDownNumber">The count down number.</param>
        public delegate void GameTimeHandler(int countDownNumber);

        /// <summary>
        ///     A delegate for winning the game.
        /// </summary>
        public delegate void LevelWonHandler();

        /// <summary>
        ///     A delegate for hitting a point object
        /// </summary>
        /// <param name="pointAmount">The point amount.</param>
        public delegate void PointHitHandler(int pointAmount);

        #endregion

        #region Data members

        private const int Milliseconds = 1000;
        private const int MillisecondsPerTick = 20;

        private readonly Player playerObject;
        private PointObject lastHitPoint;
        private Level levelInformation;

        private WaveManager waveManager;
        private Collection<PointManager> pointManagers;
        private PowerUpManager powerUpManager;
        private readonly Canvas canvas;
        private readonly AudioPlayer audioPlayer;

        private int survivalTime;
        private int currentWave;
        private bool powerUpActive;
        private int powerUpTicks;

        private DispatcherTimer timer;

        private DispatcherTimer waveTimer;
        private int waveTimerCount;

        #endregion

        #region Properties

        /// <summary>
        ///     The time left in the level.
        /// </summary>
        public int CountdownCount { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a instance of <see cref="LevelManager" /> class.
        /// </summary>
        /// <param name="background">The canvas to display on.</param>
        /// <param name="player">The player to manipulate.</param>
        public LevelManager(Canvas background, Player player)
        {
            this.canvas = background;
            this.playerObject = player;
            this.audioPlayer = new AudioPlayer();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Initializes the game placing player in the game
        ///     Precondition: background != null
        ///     Post-condition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="level">The level data to run the level/game with</param>
        public void InitializeGame(Level level)
        {
            this.formatLevelInformation(level);

            this.waveManager = new WaveManager(this.canvas, this.playerObject);
            this.powerUpActive = false;

            this.timer = new DispatcherTimer();
            this.timer.Tick += this.Timer_Tick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, MillisecondsPerTick);

            this.waveTimer = new DispatcherTimer();
            this.waveTimer.Tick += this.WaveTimer_Tick;
            this.waveTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);

            this.timer.Start();
            this.waveTimer.Start();
        }

        private void formatLevelInformation(Level level)
        {
            this.levelInformation = level;
            this.survivalTime = level.GameSurvivalTime;
            this.pointManagers = new Collection<PointManager>();

            if (level.LevelNumber == 2 || level.LevelNumber == 3 || level.LevelNumber == 1)
            {
                this.powerUpManager = new PowerUpManager(this.canvas);
            }

            foreach (var pointType in level.PointTypes)
            {
                this.pointManagers.Add(new PointManager(this.canvas, pointType));
            }

            this.currentWave = 0;
            this.CountdownCount = this.survivalTime;
        }

        private void randomlyRemovePoints()
        {
            foreach (var pointManager in this.pointManagers)
            {
                this.iterateThroughPointManagers(pointManager);
            }
        }

        private void iterateThroughPointManagers(PointManager pointManager)
        {
            foreach (var point in pointManager)
            {
                this.removePointsBasedOnLevel(pointManager, point);
            }
        }

        private async void removePointsBasedOnLevel(PointManager pointManager, PointObject point)
        {
            if (this.levelInformation.GetType() == typeof(LevelTwo))
            {
                var randomDuration = GameSettings.rnd.Next(6, 10 + 1);
                await Task.Delay(randomDuration * Milliseconds);
                this.canvas.Children.Remove(point.Sprite);
                pointManager.Remove(point);
            }

            if (this.levelInformation.GetType() == typeof(LevelThree))
            {
                var randomDuration = GameSettings.rnd.Next(3, 5 + 1);
                await Task.Delay(randomDuration * Milliseconds);
                this.canvas.Children.Remove(point.Sprite);
                pointManager.Remove(point);
            }
        }

        /// <summary>
        ///     Occurs when [game time updated].
        /// </summary>
        public event GameTimeHandler GameTimeUpdated;

        /// <summary>
        ///     Occurs when [game won].
        /// </summary>
        public event LevelWonHandler LevelWon;

        /// <summary>
        ///     Occurs when [game lost].
        /// </summary>
        public event GameLostHandler GameLost;

        /// <summary>
        ///     Occurs when [life lost].
        /// </summary>
        public event GameLostHandler LifeUpdate;

        /// <summary>
        ///     Occurs when Player hits a Point.
        /// </summary>
        public event PointHitHandler PointHit;

        private void onGameTimeUpdated()
        {
            this.GameTimeUpdated?.Invoke(this.CountdownCount);
        }

        private void onPointHit(int pointAmount)
        {
            this.PointHit?.Invoke(pointAmount);
        }

        private void onLevelWon()
        {
            this.stopGame();
            this.waveTimerCount = 0;
            this.LevelWon?.Invoke();
        }

        private void onGameLost()
        {
            this.GameLost?.Invoke();
        }

        private void onLifeLost()
        {
            this.LifeUpdate?.Invoke();
        }

        private void WaveTimer_Tick(object sender, object e)
        {
            if (this.waveTimerCount % this.levelInformation.WaveInterval == 0 &&
                !this.waveManager.HasPlayerHitADot())
            {
                this.startNextWave();
            }

            this.waveTimerCount++;
            this.CountdownCount--;
            this.onGameTimeUpdated();
        }

        private bool hasPlayerHitAPoint()
        {
            foreach (var points in this.pointManagers)
            {
                foreach (var point in points)
                {
                    if (this.playerObject.IsCircleOverlapPlayer(point))
                    {
                        this.lastHitPoint = point;
                        this.onPointHit(point.PointAmount);
                        points.Remove(point);

                        return true;
                    }
                }
            }

            return false;
        }

        private void startNextWave()
        {
            this.currentWave++;

            for (var i = 0; i <= this.levelInformation.Waves.Count; i++)
            {
                if (i == this.currentWave)
                {
                    var wave = this.levelInformation.Waves[i - 1];

                    var color = this.getWaveColor(i);

                    this.waveManager.StartWave(wave, color);
                    _ = this.playNewWaveSound();
                }
            }
        }

        private async Task playNewWaveSound()
        {
            var file = await this.audioPlayer.AudioFolder.Result.GetFileAsync("NewWave.wav");
            this.audioPlayer.PlayAudio(file);
        }

        private Color getWaveColor(int i)
        {
            if (this.levelInformation.FinalColor != new Color() &&
                this.currentWave == this.levelInformation.Waves.Count)
            {
                return this.levelInformation.FinalColor;
            }

            var colorLength = this.levelInformation.WaveColors.Count;
            var colorIndex = (i - 1) % colorLength;
            var color = this.levelInformation.WaveColors[colorIndex];
            return color;
        }

        private void Timer_Tick(object sender, object e)
        {
            if (this.powerUpActive)
            {
                _ = this.powerUp();
            }
            else if (this.waveManager.HasPlayerHitADot())
            {
                this.stopGame();
                this.lifeLost();
            }

            if (this.hasPlayerHitAPoint())
            {
                this.removePointObject(this.lastHitPoint);
            }
            else if (this.hasPlayerHitAPowerUp() && !this.powerUpActive)
            {
                _ = this.collectedPowerUp();
            }
            else if (this.waveTimerCount == this.survivalTime)
            {
                this.stopGame();
                this.onLevelWon();
            }

            this.randomlyRemovePoints();
        }

        private async Task powerUp()
        {
            _ = this.waveManager.RemoveHitDotsAsync();
            this.powerUpTicks++;

            if (this.powerUpTicks >= GameSettings.PowerUpDuration * Milliseconds / MillisecondsPerTick)
            {
                this.powerUpActive = false;

                var file = await this.audioPlayer.AudioFolder.Result.GetFileAsync("PoweredDown.wav");
                this.audioPlayer.PlayAudio(file);
            }
        }

        private async Task collectedPowerUp()
        {
            this.powerUpManager.StopPowerUpManager();
            this.powerUpActive = true;
            this.powerUpTicks = 0;

            this.playerObject.PowerUpAnimation();

            var file = await this.audioPlayer.AudioFolder.Result.GetFileAsync("PoweredUp.wav");
            this.audioPlayer.PlayAudio(file);
        }

        private bool hasPlayerHitAPowerUp()
        {
            return this.powerUpManager?.PowerUp != null &&
                   this.playerObject.IsCircleOverlapPlayer(this.powerUpManager.PowerUp);
        }

        private void lifeLost()
        {
            this.playerObject.DyingAnimation();
            this.stopGame();

            this.playerObject.PlayerLives--;
            this.onLifeLost();

            if (this.playerObject.PlayerLives <= 0)
            {
                this.onGameLost();
            }
        }

        private void stopGame()
        {
            this.timer.Stop();
            this.waveTimer.Stop();

            this.waveTimerCount = 0;
            this.waveManager.RemoveAllWaves();

            foreach (var points in this.pointManagers)
            {
                points.StopPointManager();
            }

            this.powerUpManager?.StopPowerUpManager();
        }

        private void removePointObject(PointObject point)
        {
            if (point != null)
            {
                foreach (var points in this.pointManagers)
                {
                    if (!points.Contains(point))
                    {
                        points.Remove(point);
                    }
                }

                this.canvas.Children.Remove(point.Sprite);
            }
        }

        #endregion
    }
}