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
            this.levelManager.GameWon += this.levelWon;
            this.levelManager.GameTimeUpdated += this.onGameTimeUpdated;

            this.currentLevel = 0;
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

        private void onGameWon()
        {
            this.GameWon?.Invoke();
        }

        private void onGameLost()
        {
            this.Collision?.Invoke();
        }

        private void levelWon()
        {
            this.InitializeGame();
        }

        #endregion
    }
}