using System;
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

        private int survivalTime;

        private readonly Player playerObject;
        private Level levelInformation;

        private WaveManager waveManager;
        private readonly Canvas canvas;

        private int currentWave;

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
            this.levelInformation = level;
            this.survivalTime = level.GameSurvivalTime;

            this.playerObject.Colors = level.WaveColors;
            this.playerObject.SetColors();

            this.currentWave = 0;
            this.CountdownCount = this.survivalTime;

            this.waveManager = new WaveManager(this.canvas, this.playerObject);
            this.waveManager.PointHit += this.onPointHit;

            this.timer = new DispatcherTimer();
            this.timer.Tick += this.Timer_Tick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            this.waveTimer = new DispatcherTimer();
            this.waveTimer.Tick += this.WaveTimer_Tick;
            this.waveTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);

            this.timer.Start();
            this.waveTimer.Start();
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
        public event GameLostHandler Collision;

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
            this.waveManager.RemoveAllWaves();
            this.stopGame();
            this.waveTimerCount = 0;
            this.LevelWon?.Invoke();
        }

        private void onGameLost()
        {
            this.Collision?.Invoke();
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
                }
            }
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
            if (this.waveManager.HasPlayerHitADot())
            {
                this.playerObject.PlayerLives--;

                this.playerObject.DyingAnimation();

                this.stopGame();
                this.onGameLost();
            }
            else if (this.waveTimerCount == this.survivalTime)
            {
                this.stopGame();
                this.onLevelWon();
            }
        }

        private void stopGame()
        {
            this.timer.Stop();
            this.waveTimer.Stop();
            this.waveManager.RemoveAllWaves();
        }

        #endregion
    }
}