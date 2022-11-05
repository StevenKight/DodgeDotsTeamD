using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using DodgeDots.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DodgeDots.View
{
    /// <summary>
    ///     The High Score Board page for displaying the top 10 high scores.
    /// </summary>
    public sealed partial class HighScorePage
    {
        #region Constructors

        /// <summary>
        ///     The High Score Board page for displaying the top 10 high scores.
        /// </summary>
        /// <param name="background">The canvas to put the High Score screen on.</param>
        public HighScorePage()
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
            this.highScoreboardText.Width = GameSettings.ApplicationWidth;
            this.buttonGrid.Width = GameSettings.ApplicationWidth;
            this.buttonGrid.Height = GameSettings.ApplicationHeight;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        #endregion
    }
}