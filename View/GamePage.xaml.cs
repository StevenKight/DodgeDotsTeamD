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
        ///     Runs when the page is navigated to from another page
        /// </summary>
        /// <param name="e">Navigation event arguments</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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
            var gameManager = new GameManager(this.canvas, this.playerManager);

            gameManager.GameTimeUpdated += this.GameManager_GameTimeUpdated;
            gameManager.Collision += this.GameManager_GameLostAsync;
            gameManager.GameWon += this.GameManager_GameWon;
            gameManager.GameScoreUpdated += this.GameManager_GameScoreUpdated;
            gameManager.LevelCompleted += this.GameManager_LevelCompleted;
            gameManager.LevelUpdated += this.GameManager_LevelIncrementAsync;
            gameManager.LifeUpdated += this.GameManager_LifeUpdated;

            _ = gameManager.InitializeGameAsync();
        }

        private void GameManager_LifeUpdated(int lives)
        {
            this.lives.Text = $"Lives: {lives}";
        }

        private void GameManager_GameScoreUpdated(int gameScore)
        {
            this.score.Text = $"Score: {gameScore}";
        }

        private async void GameManager_GameWon()
        {
            // TODO Data binding win message
            this.setGameOverResultText("YOU WIN");

            await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);

            this.gameOver();
        }

        private void gameOver()
        {
            Frame.Navigate(typeof(GameOverView));
        }

        private async void GameManager_GameLostAsync()
        {
            // TODO Data binding lose message
            this.setGameOverResultText("GAME OVER");

            await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);

            this.gameOver();
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