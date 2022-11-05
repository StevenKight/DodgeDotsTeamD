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
        private GameOverView gameOverView;

        #endregion

        #region Properties

        private Player Player => this.playerManager.PlayerDot;

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.buildView();
        }

        private void buildView()
        {
            this.countdown.Width = GameSettings.ApplicationWidth - this.countdown.FontSize;
            this.lives.Width = GameSettings.ApplicationWidth - this.countdown.FontSize;

            this.gameOverTextBlock.Width = GameSettings.ApplicationWidth;
            this.canvas.Height = GameSettings.ApplicationHeight;

            this.countdown.Foreground = new SolidColorBrush(Colors.LawnGreen);
            this.lives.Foreground = new SolidColorBrush(Colors.LawnGreen);

            this.gameOverTextBlock.Visibility = Visibility.Collapsed;

            this.lives.Text += GameSettings.PlayerLives;

            this.runGameView();
        }

        private void runGameView()
        {
            this.countdown.Text = GameSettings.GameSurvivalTime.ToString();

            var player = this.displayPlayer();

            this.runGame(player);
        }

        private void runGame(Player player)
        {
            var gameManager = new GameManager(player);
            gameManager.InitializeGame(this.canvas);
            gameManager.GameTimeUpdated += this.GameManager_GameTimeUpdated;
            gameManager.Collision += this.GameManager_GameLostAsync;
            gameManager.GameWon += this.GameManager_GameWon;
        }

        private Player displayPlayer()
        {
            this.playerManager = new PlayerDotManager(this.canvas);
            var player = this.playerManager.PlayerDot;

            this.Player.SetOuterColor(GameSettings.PrimaryDotColor);
            this.Player.SetInnerColor(GameSettings.SecondaryDotColor);
            return player;
        }

        private async void GameManager_GameWon()
        {
            // TODO Data binding
            this.setGameOverResultText("YOU WIN");

            await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);

            this.gameOver(false);
        }

        private void gameOver(bool lose)
        {
            this.dimBackgroundElements();

            Frame.Navigate(typeof(GameOverView));
        }

        private void dimBackgroundElements()
        {
            foreach (var child in this.canvas.Children)
            {
                child.Opacity = 0.25;
            }
        }

        private async void GameManager_GameLostAsync()
        {
            // TODO Data binding
            this.setGameOverResultText("GAME OVER");

            await Task.Delay(GameSettings.DyingAnimationLength * Milliseconds);

            this.gameOver(true);
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