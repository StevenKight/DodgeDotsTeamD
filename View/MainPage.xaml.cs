using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using DodgeDots.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DodgeDots.View
{
    /// <summary>
    ///     The main view that opens when running application.
    /// </summary>
    public sealed partial class MainPage
    {
        #region Constructors

        /// <summary>
        ///     The main view that opens when running application.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

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

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage));
        }

        private void High_Score_Click(object sender, RoutedEventArgs e)
        {
            this.displayHighScoreView();
        }

        private void displayHighScoreView()
        {
            Frame.Navigate(typeof(HighScorePage));
        }

        #endregion
    }
}