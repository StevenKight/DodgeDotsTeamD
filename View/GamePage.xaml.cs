using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using DodgeDots.Model;
using DodgeDots.View.Sprites;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DodgeDots.View
{
    /// <summary>
    ///     The main page for the game.
    /// </summary>
    public sealed partial class GamePage
    {
        #region Data members

        private const int ElementsInStartView = 4;

        private PlayerDotManager playerManager;

        private readonly Collection<UIElement> gameViewElements;
        private readonly Canvas background;

        #endregion

        #region Properties

        private Player Player => this.playerManager.PlayerDot;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GamePage" /> class.
        /// </summary>
        public GamePage(Canvas background)
        {
            this.InitializeComponent();

            this.countdown.Width = GameSettings.ApplicationWidth - 5;
            this.gameOverTextBlock.Width = GameSettings.ApplicationWidth;

            this.countdown.Foreground = new SolidColorBrush(Colors.LawnGreen);
            this.countdown.Text = GameSettings.GameSurvivalTime.ToString();
            this.gameOverTextBlock.Visibility = Visibility.Collapsed;
            this.canvas.Height = GameSettings.ApplicationHeight;

            this.gameViewElements = this.getChildren();
            this.background = background;
            this.showGameView();
        }

        #endregion

        #region Methods

        private void showGameView()
        {
            for (var i = this.background.Children.Count; i > ElementsInStartView; i--)
            {
                this.background.Children[i].Visibility = Visibility.Collapsed;
            }

            foreach (var gameViewElement in this.gameViewElements)
            {
                if (this.background.Children.Contains(gameViewElement))
                {
                    gameViewElement.Visibility = Visibility.Visible;
                }
                else
                {
                    this.background.Children.Add(gameViewElement);
                }
            }

            var player = this.displayPlayer();

            this.runGameDisplay(player);
        }

        private void runGameDisplay(Player player)
        {
            var gameManager = new GameManager(player);
            gameManager.InitializeGame(this.background);
            gameManager.GameTimeUpdated += this.GameManager_GameTimeUpdated;
            gameManager.GameLost += this.GameManager_GameLostAsync;
            gameManager.GameWon += this.GameManager_GameWon;
        }

        private Player displayPlayer()
        {
            this.playerManager = new PlayerDotManager(this.background);
            var player = this.playerManager.PlayerDot;

            this.Player.SetOuterColor(GameSettings.PrimaryDotColor);
            this.Player.SetInnerColor(GameSettings.SecondaryDotColor);
            return player;
        }

        private Collection<UIElement> getChildren()
        {
            var children = new Collection<UIElement>();

            foreach (var child in this.canvas.Children)
            {
                children.Add(child);
            }

            this.canvas.Children.Clear();
            return children;
        }

        /// <summary>
        ///     A delegate for clicking the back button.
        /// </summary>
        public delegate void GameEndHandler();

        /// <summary>
        ///     Fires an event when the back button is clicked.
        /// </summary>
        public event GameEndHandler GameEnd;

        private void GameManager_GameWon()
        {
            this.setGameOverResultText("YOU WIN");
        }

        private async void GameManager_GameLostAsync()
        {
            this.setGameOverResultText("GAME OVER");

            await Task.Delay(2 * 1000);

            this.removeGameDisplay();

            this.GameEnd?.Invoke();
        }

        private void removeGameDisplay()
        {
            var remove = new Collection<UIElement>();

            foreach (var child in this.background.Children)
            {
                if (this.removeCheck(child))
                {
                    remove.Add(child);
                }
            }

            foreach (var child in remove)
            {
                this.background.Children.Remove(child);
            }
        }

        private bool removeCheck(UIElement child)
        {
            return this.dotCheck(child) || this.playerCheck(child) || this.gameViewCheck(child);
        }

        private bool gameViewCheck(UIElement child) => this.gameViewElements.Contains(child);

        // TODO Ask about below warning
        private bool playerCheck(UIElement child) => child.GetType() == typeof(PlayerSprite);

        // TODO Ask about below warning
        private bool dotCheck(UIElement child) => child.GetType() == typeof(DotSprite);

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