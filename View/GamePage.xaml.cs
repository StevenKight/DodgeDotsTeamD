using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using DodgeDots.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DodgeDots.View
{
    /// <summary>
    ///     The main page for the game.
    /// </summary>
    public sealed partial class GamePage
    {
        #region Data members

        private const int Milliseconds = 1000;

        private PlayerDotManager playerManager;
        private GameManager gameManager;
        private HighScoreManager scoreBoard;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePage" /> class.
        /// </summary>
        public GamePage()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Invoked when Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event arguments passed to the method.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                this.scoreBoard = (HighScoreManager)e.Parameter;
            }

            this.buildView();
        }

        private void buildView()
        {
            this.adjustColors();

            this.gameOverTextBlock.Visibility = Visibility.Collapsed;

            this.lives.Text += GameSettings.PlayerLives;

            this.playerManager = new PlayerDotManager(this.canvas);

            this.runGame();
        }

        private void adjustColors()
        {
            this.countdown.Foreground = new SolidColorBrush(Colors.LawnGreen);
            this.lives.Foreground = new SolidColorBrush(Colors.LawnGreen);
        }

        private void runGame()
        {
            this.gameManager = new GameManager(this.canvas, this.playerManager, this.scoreBoard);

            this.gameManager.GameTimeUpdated += this.GameManager_GameTimeUpdated;
            this.gameManager.GameOver += this.gameManager_GameOver;
            this.gameManager.GameScoreUpdated += this.GameManager_GameScoreUpdated;
            this.gameManager.LevelCompleted += this.GameManager_LevelCompleted;
            this.gameManager.LevelUpdated += this.GameManager_LevelIncrementAsync;
            this.gameManager.LifeUpdated += this.GameManager_LifeUpdated;

            _ = this.gameManager.InitializeGameAsync();
        }

        private void GameManager_LifeUpdated(int lives)
        {
            this.lives.Text = $"Lives: {lives}";
        }

        private void GameManager_GameScoreUpdated(int gameScore)
        {
            this.score.Text = $"Score: {gameScore}";
        }

        private async void gameManager_GameOver(string text)
        {
            this.setGameOverResultText(text);

            await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);

            var gameOverList = new Collection<object> { this.gameManager, this.scoreBoard };
            Frame.Navigate(typeof(GameOverView), gameOverList);
        }

        private async void GameManager_LevelCompleted()
        {
            this.setGameOverResultText("LEVEL COMPLETE");

            await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);

            this.gameOverTextBlock.Visibility = Visibility.Collapsed;
        }

        private async void GameManager_LevelIncrementAsync(string title)
        {
            this.setGameOverResultText(title);

            await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);

            this.gameOverTextBlock.Visibility = Visibility.Collapsed;
        }

        private void setGameOverResultText(string message)
        {
            this.gameOverTextBlock.Text = message;
            this.gameOverTextBlock.Visibility = Visibility.Visible;
        }

        private void GameManager_GameTimeUpdated(int countDownNumber)
        {
            this.setCountdownColorByRemainingTime(countDownNumber);
            this.countdown.Text = countDownNumber.ToString();
        }

        private void setCountdownColorByRemainingTime(int countDownNumber)
        {
            const int yellowColorTimeRemaining = 10;
            const int redColorTimeRemaining = 5;

            if (countDownNumber > yellowColorTimeRemaining)
            {
                this.countdown.Foreground = new SolidColorBrush(Colors.LawnGreen);
            }
            else if (countDownNumber > redColorTimeRemaining)
            {
                this.countdown.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            else
            {
                this.countdown.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        #endregion
    }
}