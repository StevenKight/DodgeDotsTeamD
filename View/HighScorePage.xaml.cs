using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

        private ComboBoxItem selectedItem;

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

            this.selectedItem = this.sortOne;
            this.getScoreBoard();
        }

        // TODO Other sort functions and display in columns
        private void getScoreBoard()
        {
            var ordered = this.sort_Score_Name_Level();

            if (this.selectedItem == this.sortTwo)
            {
                ordered = this.sort_Name_Score_Level();
            }
            else if (this.selectedItem == this.sortThree)
            {
                ordered = this.sort_Level_Score_Name();
            }

            var topTen = new Collection<UserScore>(ordered.Take(10).ToArray());

            this.listDisplay.Text = "";

            foreach (var score in topTen)
            {
                this.listDisplay.Text += score + Environment.NewLine;
            }

            if (topTen.Count == 0)
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

        private void sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.selectedItem = (ComboBoxItem)this.sort.SelectedItem;

            this.getScoreBoard();
        }

        private IOrderedEnumerable<UserScore> sort_Score_Name_Level()
        {
            var firstOrdered = this.scoreBoard.AllScores.OrderByDescending(i => i.Score);
            var secondOrdered = firstOrdered.ThenBy(i => i.Username);
            var ordered = secondOrdered.ThenByDescending(i => i.Level);

            return ordered;
        }

        private IOrderedEnumerable<UserScore> sort_Name_Score_Level()
        {
            var firstOrdered = this.scoreBoard.AllScores.OrderBy(i => i.Username);
            var secondOrdered = firstOrdered.ThenByDescending(i => i.Score);
            var ordered = secondOrdered.ThenByDescending(i => i.Level);

            return ordered;
        }

        private IOrderedEnumerable<UserScore> sort_Level_Score_Name()
        {
            var firstOrdered = this.scoreBoard.AllScores.OrderByDescending(i => i.Level);
            var secondOrdered = firstOrdered.ThenByDescending(i => i.Score);
            var ordered = secondOrdered.ThenBy(i => i.Username);

            return ordered;
        }

        #endregion
    }
}