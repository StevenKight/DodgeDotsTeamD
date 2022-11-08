using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
        ///     A delegate for winning the game.
        /// </summary>
        public delegate void GameOverHandler(string displayText);

        /// <summary>
        ///     A delegate for the game score
        /// </summary>
        /// <param name="gameScore">The game score number.</param>
        public delegate void GameScoreHandler(int gameScore);

        /// <summary>
        ///     A delegate for the countdown
        /// </summary>
        /// <param name="countDownNumber">The count down number.</param>
        public delegate void GameTimeHandler(int countDownNumber);

        /// <summary>
        ///     A delegate for the winning a level
        /// </summary>
        public delegate void LevelCompletedHandler();

        /// <summary>
        ///     A delegate for the game level
        /// </summary>
        /// <param name="levelTitle">The title of the level.</param>
        public delegate void LevelHandler(string levelTitle);

        /// <summary>
        ///     A delegate for updating the lives
        /// </summary>
        /// <param name="lives">The lives.</param>
        public delegate void LifeHandler(int lives);

        #endregion

        #region Data members

        private const int Milliseconds = 1000;
        private PlayerDotManager playerManager;
        private LevelManager levelManager;
        private readonly Collection<Level> levelList;

        private bool isLevelComplete;
        private int playerDeathCount;
        private Canvas background;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the game score.
        /// </summary>
        /// <value>
        ///     The game score.
        /// </value>
        public int GameScore { get; private set; }

        /// <summary>
        ///     Gets the current level of the game.
        /// </summary>
        public int CurrentLevel { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        public GameManager()
        {
            this.levelList = new Collection<Level>
            {
                new LevelOne(),
                new LevelTwo(),
                new LevelThree()
            };

            this.CurrentLevel = 0;
            this.GameScore = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets up the game to be ran.
        /// </summary>
        /// <param name="canvas">The canvas to display the game on.</param>
        public void SetupGame(Canvas canvas)
        {
            this.background = canvas;
            this.playerManager = new PlayerDotManager(canvas);
        }

        private async void LevelManager_lifeLost()
        {
            this.playerManager.StopPlayer();
            this.playerDeathCount++;
            await this.gameOver("LifeLost.wav");

            if (this.playerDeathCount < GameSettings.PlayerLives)
            {
                await this.loadLevel();
            }
        }

        /// <summary>
        ///     Initialize and run the game.
        /// </summary>
        public async Task InitializeGameAsync()
        {
            this.setupLevelManager();

            this.CurrentLevel++;

            if (this.CurrentLevel <= this.levelList.Count)
            {
                await this.loadLevel();
            }
            else
            {
                this.onGameWon();
            }
        }

        private void setupLevelManager()
        {
            this.levelManager = new LevelManager(this.playerManager.PlayerDot);

            this.levelManager.GameLost += this.onGameLost;
            this.levelManager.LifeUpdate += this.LevelManager_lifeLost;
            this.levelManager.LevelWon += this.levelWon;
            this.levelManager.GameTimeUpdated += this.onGameTimeUpdated;
            this.levelManager.PointHit += this.WaveManager_PointHit;
        }

        private async Task loadLevel()
        {
            var level = this.levelList[this.CurrentLevel - 1];

            this.onGameLifeUpdated(this.playerManager.PlayerDot.PlayerLives);

            this.LevelUpdated?.Invoke(level.Title);

            this.onGameTimeUpdated(level.GameSurvivalTime);

            this.preparePlayer(level);

            this.playerManager.PlacePlayerCenteredInGameArena();
            await this.delayForNextLevel();

            this.runLevel(level);
        }

        private async Task delayForNextLevel()
        {
            if (this.isLevelComplete || this.playerDeathCount != GameSettings.PlayerLives)
            {
                await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);
                this.isLevelComplete = false;
            }
        }

        private void runLevel(Level level)
        {
            this.playerManager.RestartPlayer();
            this.levelManager.InitializeGame(level, this.background);
        }

        private void preparePlayer(Level level)
        {
            this.playerManager.StopPlayer();
            this.playerManager.PlayerDot.Colors = level.WaveColors;
            this.playerManager.PlayerDot.SetColors();
        }

        /// <summary>
        ///     Occurs when [game time updated].
        /// </summary>
        public event GameTimeHandler GameTimeUpdated;

        /// <summary>
        ///     Occurs when [game time updated].
        /// </summary>
        public event GameScoreHandler GameScoreUpdated;

        /// <summary>
        ///     Occurs when [game time updated].
        /// </summary>
        public event LevelHandler LevelUpdated;

        /// <summary>
        ///     Occurs when [life updated].
        /// </summary>
        public event LifeHandler LifeUpdated;

        /// <summary>
        ///     Occurs when level is won.
        /// </summary>
        public event LevelCompletedHandler LevelCompleted;

        /// <summary>
        ///     Occurs when [game won].
        /// </summary>
        public event GameOverHandler GameOver;

        private void onGameTimeUpdated(int countdownCount)
        {
            this.GameTimeUpdated?.Invoke(countdownCount);
        }

        private void onGameLifeUpdated(int lives)
        {
            this.LifeUpdated?.Invoke(lives);
        }

        private async void onGameWon()
        {
            await this.gameOver("YouWin.wav");
            this.GameOver?.Invoke("YOU WIN");
        }

        private async Task gameOver(string filename)
        {
            var file = await GameSettings.AudioManager.AudioFolder.Result.GetFileAsync(filename);
            GameSettings.AudioManager.PlayAudio(file);

            this.playerManager.StopPlayer();
        }

        private async void onGameLost()
        {
            await this.gameOver("GameOver.wav");
            this.GameOver?.Invoke("GAME OVER");
        }

        private void onGameScoreUpdated()
        {
            this.GameScoreUpdated?.Invoke(this.GameScore);
        }

        private void onLevelCompleted()
        {
            this.LevelCompleted?.Invoke();
        }

        private async void levelWon()
        {
            this.onLevelCompleted();
            this.isLevelComplete = true;
            await this.gameOver("LevelWon.wav");

            await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);

            _ = this.InitializeGameAsync();
        }

        private void WaveManager_PointHit(int pointAmount)
        {
            this.GameScore += pointAmount;
            this.onGameScoreUpdated();
        }

        #endregion
    }
}