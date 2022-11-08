using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using DodgeDots.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DodgeDots.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameOverView
    {
        #region Constructors

        /// <summary>
        ///     Initializes the game over view.
        /// </summary>
        public GameOverView()
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
            this.scoreText.Text =
                $"Score: {GameSettings.GameManager.GameScore} Level: {GameSettings.GameManager.CurrentLevel}";
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.userName.Text != "")
            {
                this.saveGameScore();

                Frame.Navigate(typeof(HighScorePage));
            }
            else
            {
                Frame.Navigate(typeof(MainPage));
            }
        }

        private void saveGameScore()
        {
            GameSettings.ScoreBoard.AddScore(GameSettings.GameManager.GameScore,
                GameSettings.GameManager.CurrentLevel,
                this.userName.Text);
        }

        private void playAgain_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage));
        }

        #endregion
    }
}