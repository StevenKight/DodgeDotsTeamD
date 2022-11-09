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

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePage" /> class.
        /// </summary>
        public GamePage()
        {
            this.InitializeComponent();
            this.canvas.Width = GameSettings.ApplicationWidth;
            this.canvas.Height = GameSettings.ApplicationHeight;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Invoked when Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event arguments passed to the method.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.buildView();
        }

        private void buildView()
        {
            this.adjustColors();
            this.adjustText();

            this.gameOverTextBlock.Visibility = Visibility.Collapsed;

            this.lives.Text += GameSettings.PlayerLives;

            this.runGame();
        }

        private void adjustColors()
        {
            this.countdown.Foreground = new SolidColorBrush(Colors.LawnGreen);
            this.lives.Foreground = new SolidColorBrush(Colors.LawnGreen);
        }

        private void adjustText()
        {
            this.gameOverTextBlock.Width = this.canvas.Width;
            this.countdown.Width = this.canvas.Width;
            this.score.Width = this.canvas.Width;
            this.lives.Width = this.canvas.Width;
        }

        private void runGame()
        {
            GameSettings.GameManager.GameTimeUpdated += this.GameManager_GameTimeUpdated;
            GameSettings.GameManager.GameOver += this.gameManager_GameOver;
            GameSettings.GameManager.GameScoreUpdated += this.GameManager_GameScoreUpdated;
            GameSettings.GameManager.LevelCompleted += this.GameManager_LevelCompleted;
            GameSettings.GameManager.LevelUpdated += this.GameManager_LevelIncrementAsync;
            GameSettings.GameManager.LifeUpdated += this.GameManager_LifeUpdated;

            GameSettings.GameManager.SetupGame(this.canvas);
            _ = GameSettings.GameManager.InitializeGameAsync(GameSettings.GameLoadMode.Reset);
        }

        private void GameManager_LifeUpdated(int playerLives)
        {
            this.lives.Text = $"Lives: {playerLives}";
        }

        private void GameManager_GameScoreUpdated(int gameScore)
        {
            this.score.Text = $"Score: {gameScore}";
        }

        private async void gameManager_GameOver(string text)
        {
            this.setGameOverResultText(text);

            await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);

            Frame.Navigate(typeof(GameOverView));
        }

        private async void GameManager_LevelCompleted()
        {
            this.setGameOverResultText("LEVEL COMPLETE");

            await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);
        }

        private async void GameManager_LevelIncrementAsync(string title)
        {
            this.setGameOverResultText(title);

            await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);

            this.gameOverTextBlock.Visibility = Visibility.Collapsed;
        }

        private void setGameOverResultText(string message)
        {
            this.gameOverTextBlock.Visibility = Visibility.Visible;
            this.gameOverTextBlock.Text = message;
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