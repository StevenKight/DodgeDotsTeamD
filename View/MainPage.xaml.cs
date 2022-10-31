using Windows.Foundation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using DodgeDots.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DodgeDots.View
{
    /// <summary>
    ///     The main page for the game.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Data members

        /// <summary>
        ///     The application height
        /// </summary>
        public const double ApplicationHeight = 450;

        /// <summary>
        ///     The application width
        /// </summary>
        public const double ApplicationWidth = 450;

        private readonly PlayerDotManager playerManager;

        #endregion

        #region Properties

        private Player Player => this.playerManager.PlayerDot;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainPage" /> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            this.canvas.Width = ApplicationWidth;
            this.countdown.Width = ApplicationWidth - 5;
            this.gameOverTextBlock.Width = ApplicationWidth;

            ApplicationView.PreferredLaunchViewSize = new Size { Width = ApplicationWidth, Height = ApplicationHeight };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(ApplicationWidth, ApplicationHeight));
            this.countdown.Foreground = new SolidColorBrush(Colors.LawnGreen);
            this.countdown.Text = GameSettings.GameSurvivalTime.ToString();
            this.gameOverTextBlock.Visibility = Visibility.Collapsed;
            this.canvas.Height = ApplicationHeight;

            this.playerManager = new PlayerDotManager(this.canvas);
            this.Player.SetOuterColor(GameSettings.PrimaryDotColor);
            this.Player.SetInnerColor(GameSettings.SecondaryDotColor);
            var gameManager = new GameManager(this.Player);
            gameManager.InitializeGame(this.canvas);
            gameManager.GameTimeUpdated += this.GameManager_GameTimeUpdated;
            gameManager.GameLost += this.GameManager_GameLost;
            gameManager.GameWon += this.GameManager_GameWon;
        }

        #endregion

        #region Methods

        private void GameManager_GameWon()
        {
            this.setGameOverResultText("YOU WIN");
        }

        private void GameManager_GameLost()
        {
            this.setGameOverResultText("GAME OVER");
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