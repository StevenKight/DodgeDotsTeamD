using System.Collections.ObjectModel;
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
        #region Data members

        private GameManager gameManager;
        private HighScoreManager scoreBoard;

        #endregion

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
            if (e.Parameter != null)
            {
                var parameters = (Collection<object>)e.Parameter;

                this.gameManager = (GameManager)parameters[0];
                this.scoreBoard = (HighScoreManager)parameters[1];

                this.scoreText.Text = $"Score: {this.gameManager.GameScore} Level: {this.gameManager.CurrentLevel}";
            }
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.userName.Text != "")
            {
                this.gameManager.SaveGameScore(this.userName.Text);

                Frame.Navigate(typeof(HighScorePage), this.scoreBoard);
            }
            else
            {
                Frame.Navigate(typeof(MainPage), this.scoreBoard);
            }
        }

        private void playAgain_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage), this.scoreBoard);
        }

        #endregion
    }
}