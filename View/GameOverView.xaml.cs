using Windows.UI.Xaml;

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

        private void gameOver()
        {
            this.newScoreText.Text = "Try Again?";
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HighScorePage));
        }

        private void playAgain_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage));
        }

        #endregion
    }
}