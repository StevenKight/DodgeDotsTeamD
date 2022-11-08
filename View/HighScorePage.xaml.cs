using System.Collections.ObjectModel;
using System.Diagnostics;
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
            this.sort.SelectedItem = this.sortOne;
            this.getScoreBoard();
        }

        private void getScoreBoard()
        {
            var ordered = sort_Score_Name_Level();

            if (this.sort.SelectedItem == this.sortTwo)
            {
                ordered = sort_Name_Score_Level();
            }
            else if (this.sort.SelectedItem == this.sortThree)
            {
                ordered = sort_Level_Score_Name();
            }

            var topTen = new Collection<UserScore>(ordered.Take(10).ToArray());

            var listView = this.highScores;
            if (listView != null)
            {
                listView.ItemsSource = topTen;
            }

            if (topTen.Count == 0)
            {
                Debug.WriteLine("No scores to display");
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            await GameSettings.ScoreBoard.ResetHighScoresAsync();
            this.getScoreBoard();
        }

        private void sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.getScoreBoard();
        }

        private static IOrderedEnumerable<UserScore> sort_Score_Name_Level()
        {
            var firstOrdered = GameSettings.ScoreBoard.AllScores.OrderByDescending(i => i.Score);
            var secondOrdered = firstOrdered.ThenBy(i => i.Username);
            var ordered = secondOrdered.ThenByDescending(i => i.Level);

            return ordered;
        }

        private static IOrderedEnumerable<UserScore> sort_Name_Score_Level()
        {
            var firstOrdered = GameSettings.ScoreBoard.AllScores.OrderBy(i => i.Username);
            var secondOrdered = firstOrdered.ThenByDescending(i => i.Score);
            var ordered = secondOrdered.ThenByDescending(i => i.Level);

            return ordered;
        }

        private static IOrderedEnumerable<UserScore> sort_Level_Score_Name()
        {
            var firstOrdered = GameSettings.ScoreBoard.AllScores.OrderByDescending(i => i.Level);
            var secondOrdered = firstOrdered.ThenByDescending(i => i.Score);
            var ordered = secondOrdered.ThenBy(i => i.Username);

            return ordered;
        }

        #endregion
    }
}