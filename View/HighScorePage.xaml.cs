using System;
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
        #region Data members

        private HighScoreManager scoreBoard;

        #endregion

        #region Constructors

        /// <summary>
        ///     The High Score Board page for displaying the top 10 high scores.
        /// </summary>
        public HighScorePage()
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
                this.scoreBoard = (HighScoreManager)e.Parameter;
            }

            this.getScoreBoard();
        }

        // TODO Other sort functions and display in columns
        private void getScoreBoard()
        {
            this.scoreBoard.SortTopTen();
            this.listDisplay.Text = "";

            foreach (var score in this.scoreBoard.TopTen)
            {
                this.listDisplay.Text += score + Environment.NewLine;
            }

            if (this.scoreBoard.TopTen.Count == 0)
            {
                this.listDisplay.Text = "No scores to display";
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), this.scoreBoard);
        }

        private async void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            await this.scoreBoard.ResetHighScoresAsync();
            this.getScoreBoard();
        }

        #endregion
    }
}