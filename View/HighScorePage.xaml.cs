using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DodgeDots.View
{
    /// <summary>
    ///     The High Score Board page for displaying the top 10 high scores.
    /// </summary>
    public sealed partial class HighScorePage
    {
        #region Data members

        private const int ElementsInStartView = 4;

        private readonly Collection<UIElement> highScoreElements;
        private readonly Canvas background;

        #endregion

        #region Constructors

        /// <summary>
        ///     The High Score Board page for displaying the top 10 high scores.
        /// </summary>
        /// <param name="background">The canvas to put the High Score screen on.</param>
        public HighScorePage(Canvas background)
        {
            this.InitializeComponent();
            this.highScoreElements = this.getChildren();

            this.background = background;

            this.showHighScoreView();
        }

        #endregion

        #region Methods

        private void showHighScoreView()
        {
            for (var i = this.canvas.Children.Count; i > ElementsInStartView; i--)
            {
                this.canvas.Children[i].Visibility = Visibility.Collapsed;
            }

            foreach (var highScoreElement in this.highScoreElements)
            {
                if (this.background.Children.Contains(highScoreElement))
                {
                    highScoreElement.Visibility = Visibility.Visible;
                }
                else
                {
                    this.background.Children.Add(highScoreElement);
                }
            }
        }

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

        /// <summary>
        ///     A delegate for clicking the back button.
        /// </summary>
        public delegate void BackClickHandler();

        /// <summary>
        ///     Fires an event when the back button is clicked.
        /// </summary>
        public event BackClickHandler BackButtonClick;

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.BackButtonClick?.Invoke();
        }

        #endregion
    }
}