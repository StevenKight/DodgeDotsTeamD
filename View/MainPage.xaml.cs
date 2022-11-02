using System.Collections.ObjectModel;
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
        #region Data members

        private readonly Collection<UIElement> mainPageElements;

        #endregion

        #region Constructors

        /// <summary>
        ///     The main view that opens when running application.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            this.mainPageElements = this.getChildren();

            this.startText.Width = GameSettings.ApplicationWidth;
            this.buttonGrid.Width = GameSettings.ApplicationWidth;
            this.buttonGrid.Height = GameSettings.ApplicationHeight;

            this.canvas.Width = GameSettings.ApplicationWidth;

            ApplicationView.PreferredLaunchViewSize = new Size
                { Width = GameSettings.ApplicationWidth, Height = GameSettings.ApplicationHeight };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView()
                .SetPreferredMinSize(new Size(GameSettings.ApplicationWidth, GameSettings.ApplicationHeight));

            this.displayStartView();
        }

        #endregion

        #region Method

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

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            this.Hide_Views();
            var gameView = new GamePage(this.canvas);

            gameView.ScoreSubmitted += this.displayView;
        }

        private void displayView(bool startScreen)
        {
            if (startScreen)
            {
                this.displayStartView();
            }
            else
            {
                this.displayHighScoreView();
            }
        }

        private void High_Score_Click(object sender, RoutedEventArgs e)
        {
            this.displayHighScoreView();
        }

        private void displayHighScoreView()
        {
            this.Hide_Views();
            var highScoreView = new HighScorePage(this.canvas);
            highScoreView.BackButtonClick += this.displayStartView;
        }

        private void displayStartView()
        {
            this.canvas.Children.Clear();

            foreach (var child in this.mainPageElements)
            {
                child.Visibility = Visibility.Visible;
                this.canvas.Children.Add(child);
            }
        }

        private void Hide_Views()
        {
            foreach (var child in this.canvas.Children)
            {
                child.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}