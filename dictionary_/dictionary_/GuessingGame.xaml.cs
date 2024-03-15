using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace dictionary_
{
    public partial class GuessingGame : Window
    {
        private XmlHandler handler = new XmlHandler();
        private List<Word> words { get; set; }

        private string guess;

        private int score = 0;

        public GuessingGame()
        {
            InitializeComponent();
            InitializeXmlData();
        }

        private void InitializeXmlData()
        {
            DataContext = new
            {
                Words = handler.GetWords(),
                Categories = handler.GetCategories()
            };
            words = handler.WordsToGuess();
        }

        private void UpdateScoreKeeper(int score)
        {
            ScoreKeeper.Text = $"{score}/5";
        }


        private void GameStart(object sender, RoutedEventArgs e)
        {
            StartButton.Visibility = Visibility.Collapsed;
            RestartButton.Visibility = Visibility.Collapsed;
            ScoreKeeper.Visibility = Visibility.Visible;
            UserControls.Visibility = Visibility.Visible;
            Game();
        }
        private void Guess(object sender, RoutedEventArgs e)
        {
            guess = UserGuess.Text;
        }

        private void RestartGame(object sender, RoutedEventArgs e)
        {
            RestartButton.Visibility = Visibility.Collapsed;
            ScoreKeeper.Visibility = Visibility.Visible;
            UserControls.Visibility = Visibility.Visible;
            words = handler.WordsToGuess();
            score = 0;
            UpdateScoreKeeper(score);
            Game();
        }

        private async Task Game()
        {
            var rand = new List<int> { 0, 1 };
            Random rnd = new Random();
            ScoreKeeper.Text = $"{score}/5";

            foreach (Word word in words)
            {
                int index = rnd.Next(rand.Count);
                if (rand[index] == 0 && word.ImagePath != "Resources/000.jpg")
                {
                    WordDetails.Visibility = Visibility.Collapsed;
                    WordPhoto.Source = new BitmapImage(new Uri(word.ImagePath, UriKind.RelativeOrAbsolute));
                    WordPhoto.Visibility = Visibility.Visible;
                }
                if (rand[index] == 1)
                {
                    WordPhoto.Visibility = Visibility.Collapsed;
                    WordDetails.Text = $"{word.Definition}";
                    WordDetails.Visibility = Visibility.Visible;
                }

                UserGuess.Text = ""; // clear previous guess

                // wait for user input
                await WaitForUserInput();

                if (guess.ToLower() == word.WordValue.ToLower())
                {
                    MessageBox.Show($"Congrats! You guessed correctly!", "Correct Guess!", MessageBoxButton.OK);
                    score += 1;
                    UpdateScoreKeeper(score);
                }
                else
                {
                    MessageBox.Show($"Sorry, it seems you didn't get that right.", "Wrong Guess!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (score == 5)
            {
                WordDetails.Text = "CONGRATULATIONS, YOU WON! :D";
            }
            else
            {
                WordDetails.Text = "Sorry, you lost.\nRestart the game and give it another go!";
            }

            UserControls.Visibility = Visibility.Collapsed;
            ScoreKeeper.Visibility = Visibility.Collapsed;
            WordDetails.Visibility = Visibility.Visible;
            WordPhoto.Visibility = Visibility.Collapsed;
            RestartButton.Visibility = Visibility.Visible;
        }

        private Task WaitForUserInput()
        {
            var tcs = new TaskCompletionSource<bool>();
            RoutedEventHandler ehandler = null;
            ehandler = (sender, e) =>
            {
                GuessButton.Click -= ehandler;
                tcs.SetResult(true);
            };
            GuessButton.Click += ehandler;

            return tcs.Task;
        }
    }
}
