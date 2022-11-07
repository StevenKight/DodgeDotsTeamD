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
        ///     A delegate for losing the game.
        /// </summary>
        public delegate void GameLostHandler();

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
        ///     A delegate for winning the game.
        /// </summary>
        public delegate void GameWonHandler();

        /// <summary>
        ///     A delegate for the game level
        /// </summary>
        /// <param name="levelTitle">The title of the level.</param>
        public delegate void LevelHandler(string levelTitle);

        /// <summary>
        ///     A delegate for the winning a level
        /// </summary>
        public delegate void LevelCompletedHandler();

        /// <summary>
        ///  A delegate for updating the lives
        /// </summary>
        /// <param name="lives">The lives.</param>
        public delegate void LifeHandler(int lives);

        #endregion

        #region Data members

        private const int Milliseconds = 1000;

        private readonly PlayerDotManager playerManager;
        private readonly LevelManager levelManager;
        private readonly Collection<Level> levelList;

        private int currentLevel;
        private int pointTotal;
        private bool isLevelComplete;
        private bool hasPlayerDied;

        private readonly AudioPlayer audioPlayer;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the game score.
        /// </summary>
        /// <value>
        ///     The game score.
        /// </value>
        public int GameScore { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameManager" /> class.
        /// </summary>
        /// <param name="background">The canvas to display the game on.</param>
        /// <param name="player">The player object within the game.</param>
        public GameManager(Canvas background, PlayerDotManager player)
        {
            this.playerManager = player;
            this.levelManager = new LevelManager(background, player.PlayerDot);

            this.levelList = new Collection<Level>
            {
                new LevelOne(),
                new LevelTwo(),
                new LevelThree()
            };

            this.levelManager.Collision += this.onGameLost;
            this.levelManager.lifeUpdate += this.LevelManager_lifeLost;
            this.levelManager.LevelWon += this.levelWon;
            this.levelManager.GameTimeUpdated += this.onGameTimeUpdated;

            this.currentLevel = 0;
            this.pointTotal = 0;

            this.levelManager.PointHit += this.WaveManager_PointHit;

            this.audioPlayer = new AudioPlayer();
        }

        private async void LevelManager_lifeLost()
        {
            this.playerManager.StopPlayer();
            this.hasPlayerDied = true;
            await this.gameOver("LifeLost.wav");
            await this.loadLevel();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Initialize and run the game.
        /// </summary>
        public async Task InitializeGameAsync()
        {
            this.currentLevel++;

            if (this.currentLevel <= this.levelList.Count)
            {
                await this.loadLevel();
            }
            else
            {
                this.onGameWon();
            }
        }

        private async Task loadLevel()
        {
            var level = this.levelList[this.currentLevel - 1];

            await this.delayForNextLevel();

            this.onGameLifeUpdated(this.playerManager.PlayerDot.PlayerLives);

            this.LevelUpdated?.Invoke(level.Title);

            this.onGameTimeUpdated(level.GameSurvivalTime);

            this.preparePlayer(level);

            await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);

            this.runLevel(level);
        }

        private async Task delayForNextLevel()
        {
            if (this.isLevelComplete || this.hasPlayerDied)
            {
                await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);
                this.isLevelComplete = false;
            }
        }

        private void runLevel(Level level)
        {
            this.playerManager.RestartPlayer();
            this.levelManager.InitializeGame(level);
        }

        private void preparePlayer(Level level)
        {
            this.playerManager.StopPlayer();
            this.playerManager.PlacePlayerCenteredInGameArena();
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
        public event GameWonHandler GameWon;

        /// <summary>
        ///     Occurs when [game lost].
        /// </summary>
        public event GameLostHandler Collision;

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
            this.GameWon?.Invoke();
        }

        private async Task gameOver(string filename)
        {
            var file = await this.audioPlayer.AudioFolder.Result.GetFileAsync(filename);
            this.audioPlayer.PlayAudio(file);

            this.playerManager.StopPlayer();
        }

        private async void onGameLost()
        {
            await this.gameOver("GameOver.wav");
            this.Collision?.Invoke();
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