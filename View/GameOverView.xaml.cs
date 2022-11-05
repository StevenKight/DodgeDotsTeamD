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

        private readonly bool startScreen;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes the game over view.
        /// </summary>
        /// <param name="lose">
        ///     <value>True</value>
        ///     if the player lost,
        ///     <value>False</value>
        ///     otherwise
        /// </param>
        public GameOverView()
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
            this.newScoreText.Width = GameSettings.ApplicationWidth;
            this.scoreText.Width = GameSettings.ApplicationWidth;
            this.buttonGrid.Width = GameSettings.ApplicationWidth;
            this.buttonGrid.Height = GameSettings.ApplicationHeight;
        }

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