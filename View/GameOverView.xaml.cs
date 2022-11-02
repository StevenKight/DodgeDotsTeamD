using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DodgeDots.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DodgeDots.View
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GameOverView : Page
    {
        #region Types and Delegates

        /// <summary>
        ///     A delegate for losing the game.
        /// </summary>
        public delegate void PlayAgainHandler();

        /// <summary>
        ///     A delegate for losing the game.
        /// </summary>
        public delegate void SubmitHandler(bool startScreen);

        #endregion

        #region Data members

        private readonly bool startScreen;

        #endregion

        #region Constructors

        public GameOverView(bool lose)
        {
            this.InitializeComponent();

            this.newScoreText.Width = GameSettings.ApplicationWidth;
            this.scoreText.Width = GameSettings.ApplicationWidth;
            this.buttonGrid.Width = GameSettings.ApplicationWidth;
            this.buttonGrid.Height = GameSettings.ApplicationHeight;

            if (lose)
            {
                this.gameOver();

                this.startScreen = true;
            }
            else
            {
                this.startScreen = false;
            }
        }

        #endregion

        #region Methods

        public Collection<UIElement> getChildren()
        {
            var children = new Collection<UIElement>();

            foreach (var child in this.canvas.Children)
            {
                children.Add(child);
            }

            this.canvas.Children.Clear();
            return children;
        }

        private void gameOver()
        {
            this.newScoreText.Text = "Try Again?";
            this.submitButton.Content = "Start Menu";
            this.userName.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     Occurs when [game time updated].
        /// </summary>
        public event SubmitHandler ScoreSubmitted;

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            this.ScoreSubmitted?.Invoke(this.startScreen);
        }

        /// <summary>
        ///     Occurs when [game time updated].
        /// </summary>
        public event PlayAgainHandler PlayAgain;

        private void playAgain_Click(object sender, RoutedEventArgs e)
        {
            this.PlayAgain?.Invoke();
        }

        #endregion
    }
}