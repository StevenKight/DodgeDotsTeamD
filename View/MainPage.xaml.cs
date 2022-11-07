using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using DodgeDots.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DodgeDots.View
{
    /// <summary>
    ///     The main view that opens when running application.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Data members

        private HighScoreManager scoreBoard;

        #endregion

        #region Constructors

        /// <summary>
        ///     The main view that opens when running application.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            this.pageSetup();
        }

        #endregion

        #region Methods

        private void pageSetup()
        {
            this.startText.Width = GameSettings.ApplicationWidth;
            this.buttonGrid.Width = GameSettings.ApplicationWidth;
            this.buttonGrid.Height = GameSettings.ApplicationHeight;

            this.canvas.Width = GameSettings.ApplicationWidth;

            ApplicationView.PreferredLaunchViewSize = new Size
                { Width = GameSettings.ApplicationWidth, Height = GameSettings.ApplicationHeight };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView()
                .SetPreferredMinSize(new Size(GameSettings.ApplicationWidth, GameSettings.ApplicationHeight));
        }

        #endregion

        #region Method

        /// <summary>
        ///     Invoked when Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event arguments passed to the method.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                this.scoreBoard = (HighScoreManager)e.Parameter;
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage), this.scoreBoard);
        }

        private void High_Score_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HighScorePage), this.scoreBoard);
        }

        private async void Reset_Score_Click(object sender, RoutedEventArgs e)
        {
            await this.scoreBoard.ResetHighScoresAsync();
        }

        #endregion
    }
}