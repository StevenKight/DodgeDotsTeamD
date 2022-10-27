using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeDots.Model
{
    /// <summary>
    ///     Manages the entire game.
    /// </summary>
    public class GameManager
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
        public delegate void GameWonHandler();

        #endregion

        #region Data members

        private const int MaxWaves = 5;
        private const int WaveInterval = 5;
        private const int SurvivalTime = GameSettings.GameSurvivalTime;

        private readonly Player playerObject;

        private WaveManager waveManager;
        private Canvas canvas;

        private int currentWave;

        private DispatcherTimer timer;

        private DispatcherTimer waveTimer;
        private int waveTimerCount;
        private int countdownCount;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        ///     Precondition: backgroundHeight > 0 AND backgroundWidth > 0
        /// </summary>
        /// <param name="player">The player dot in the game</param>
        public GameManager(Player player)
        {
            this.playerObject = player;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when [game time updated].
        /// </summary>
        public event GameTimeHandler GameTimeUpdated;

        /// <summary>
        ///     Occurs when [game won].
        /// </summary>
        public event GameWonHandler GameWon;

        /// <summary>
        ///     Occurs when [game lost].
        /// </summary>
        public event GameLostHandler GameLost;

        private void onGameTimeUpdated()
        {
            this.GameTimeUpdated?.Invoke(this.countdownCount);
        }

        private void onGameWon()
        {
            this.GameWon?.Invoke();
        }

        private void onGameLost()
        {
            this.GameLost?.Invoke();
        }

        /// <summary>
        ///     Initializes the game placing player in the game
        ///     Precondition: background != null
        ///     Post-condition: Game is initialized and ready for play.
        /// </summary>
        /// <param name="background">The background canvas.</param>
        public void InitializeGame(Canvas background)
        {
            this.canvas = background ?? throw new ArgumentNullException(nameof(background));

            this.currentWave = 1;
            this.countdownCount = SurvivalTime;

            this.waveManager = new WaveManager(this.canvas, this.playerObject);

            this.timer = new DispatcherTimer();
            this.timer.Tick += this.Timer_Tick;
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            this.waveTimer = new DispatcherTimer();
            this.waveTimer.Tick += this.WaveTimer_Tick;
            this.waveTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);

            this.timer.Start();
            this.waveTimer.Start();
            this.waveManager.StartWaveDownMoveDirection();
        }

        private void WaveTimer_Tick(object sender, object e)
        {
            if (this.waveTimerCount % WaveInterval == 0 && this.waveTimerCount != 0 &&
                this.currentWave < MaxWaves &&
                !this.waveManager.HasPlayerHitADot())
            {
                this.startNextWave();
            }

            this.waveTimerCount++;
            this.countdownCount--;
            this.onGameTimeUpdated();
        }

        private void startNextWave()
        {
            this.currentWave++;
            switch (this.currentWave)
            {
                case 2:
                {
                    this.waveManager.StartWaveRightMoveDirection();
                    break;
                }
                case 3:
                {
                    this.waveManager.StartWaveUpMoveDirection();
                    break;
                }
                case 4:
                {
                    this.waveManager.StartWaveLeftMoveDirection();
                    break;
                }
                case 5:
                {
                    this.waveManager.StartFinalBlitzWave();
                    break;
                }
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            if (this.waveManager.HasPlayerHitADot())
            {
                this.stopGame();
                this.onGameLost();
            }
            else if (this.waveTimerCount == SurvivalTime)
            {
                this.stopGame();
                this.onGameWon();
            }
        }

        private void stopGame()
        {
            this.timer.Stop();
            this.waveTimer.Stop();
            this.waveManager.StopAllActiveDotManagers();
        }

        #endregion
    }
}