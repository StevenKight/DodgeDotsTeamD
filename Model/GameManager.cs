using System;
using System.Collections.ObjectModel;
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

        #endregion

        #region Data members

        private readonly LevelManager levelManager;
        private readonly Collection<Level> levelList;

        private int currentLevel;
        private int pointTotal;

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
        public GameManager(Canvas background, Player player)
        {
            this.levelManager = new LevelManager(background, player);

            this.levelList = new Collection<Level>
            {
                new LevelOne(),
                new LevelTwo(),
                new LevelThree()
            };

            this.levelManager.Collision += this.onGameLost;
            this.levelManager.LevelWon += this.levelWon;
            this.levelManager.GameTimeUpdated += this.onGameTimeUpdated;

            this.currentLevel = 0;
            this.pointTotal = 0;

            this.levelManager.PointHit += this.WaveManager_PointHit;

            this.audioPlayer = new AudioPlayer();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Initialize and run the game.
        /// </summary>
        public void InitializeGame()
        {
            this.currentLevel++;

            if (this.currentLevel <= this.levelList.Count)
            {
                this.levelManager.InitializeGame(this.levelList[this.currentLevel - 1]);
            }
            else
            {
                this.onGameWon();
            }
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

        private async void onGameWon()
        {
            var file = await this.audioPlayer.AudioFolder.Result.GetFileAsync("YouWin.wav");
            this.audioPlayer.PlayAudio(file);
            this.GameWon?.Invoke();
        }

        private async void onGameLost()
        {
            var file = await this.audioPlayer.AudioFolder.Result.GetFileAsync("GameOver.wav");
            this.audioPlayer.PlayAudio(file);
            this.Collision?.Invoke();
        }

        private void onGameScoreUpdated()
        {
            this.GameScoreUpdated?.Invoke(this.GameScore);
        }

        private void levelWon()
        {
            this.InitializeGame();
        }

        private void WaveManager_PointHit(int pointAmount)
        {
            this.GameScore += pointAmount;
            this.onGameScoreUpdated();
        }

        #endregion
    }
}