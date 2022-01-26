using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordleWPFClient.Classes;
using WordleWPFClient;
using Wordle;

namespace WordleWPFClient
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {

        public static int wordleColumns = 5;
        public static int wordleRowsMax = 6;
        public int currentRow = 0;
        private Grid WordleGameGrid = new Grid();
        private Letter[][] wordleLetters = new Letter[wordleRowsMax][];
        private Dictionary<string, int[]> wordleLettersReference = new Dictionary<string, int[]>();
        private WordleWPFClient.Classes.Game game;
        private string wordleRows;

        public GamePage()
        {
            // this is a test
            //Wordle.Game wordlegametest = new Wordle.Game(App.WordsDbPath);

            InitializeComponent();
            newGame();
        }


        private void newGame()
        {
            UserMessageTextBlock.Text = "";
            currentRow = 0;
            WordleGamePanel.Children.Remove(WordleGameGrid);
            WordleGameGrid.Children.Clear();
            WordleGameGrid = new Grid();
            wordleLetters = new Letter[wordleRowsMax][];
            wordleLettersReference = new Dictionary<string, int[]>();

            game = new Classes.Game(GetRandomWord(), wordleRowsMax);
            BuildWordleGrid();
            //WordleLabel.Content = game.secretWord;
        }

        private string GetRandomWord()
        {
            Models.Word randomWord;

            using (SQLiteConnection connection = new SQLiteConnection(App.DbConnectionString))
            {
                Random rand = new Random();
                int maxId = connection.Table<Models.Word>().Count();
                int randomIndex = rand.Next(1, maxId);

                List<Models.Word> randomWords = connection.Table<Models.Word>().Where(x => x.Id.Equals(randomIndex)).ToList<Models.Word>();
                randomWord = randomWords[0];
            }
            return randomWord.Text;
        }

        private void BuildWordleGrid()
        {

            for (int i = 0; i < wordleColumns; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);

                WordleGameGrid.ColumnDefinitions.Add(col);

            }


            for (int i = 0; i < wordleRowsMax; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                WordleGameGrid.RowDefinitions.Add(row);

                // init wordleletters state columns
                wordleLetters[i] = new Letter[wordleColumns];

            }



            WordleGameGrid.VerticalAlignment = VerticalAlignment.Top;

            List<TextBox> textBoxesList = new List<TextBox>();
            // row
            for (int i = 0; i < wordleRowsMax; i++)
            {
                // col
                for (int j = 0; j < wordleColumns; j++)
                {
                    TextBox textBox = new TextBox();

                    if (i == 0 && j == 0)
                    {
                        //textBox.Focus();
                    }

                    textBox.FontSize = 24;
                    textBox.MaxLength = 1;
                    textBox.Height = 40;
                    textBox.Margin = new Thickness(5);
                    textBox.Padding = new Thickness(5);
                    textBox.Background = Brushes.AliceBlue;
                    textBox.Name = $"letterText{i}_{j}";
                    textBox.CharacterCasing = CharacterCasing.Upper;
                    textBox.TextChanged += LetterInput_TextChanged;
                    textBox.KeyDown += LetterInpout_KeyDown;


                    if (j > 0)
                        textBox.SetValue(Grid.ColumnProperty, j);
                    if (i > 0)
                        textBox.SetValue(Grid.RowProperty, i);

                    if (i != game.guessPos)
                    {
                        textBox.IsEnabled = false;
                        textBox.Background = Brushes.DarkGray;
                    }

                    // init wordleletters state
                    wordleLetters[i][j] = new Letter();
                    wordleLetters[i][j].associatedTextBox = textBox;
                    wordleLetters[i][j].Row = i;
                    wordleLetters[i][j].Position = j;
                    wordleLettersReference.Add($"{textBox.Name.GetHashCode()}", new int[2] { i, j });

                    WordleGameGrid.Children.Add(textBox);

                }

            }

            WordleGamePanel.Children.Add(WordleGameGrid);

            wordleLetters[0][0].associatedTextBox.Focus();
        }

        private void LetterInpout_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SubmitButton_Click(sender, e);
            }
            else if (e.Key == Key.Back)
            {
                // TODO: backspace through inputs (like shift + tab)
            }
        }




        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {

            UserMessageTextBlock.Text = "";
            UserMessageTextBlock.Foreground = Brushes.Black;

            string wordGuess = letterArrayToWordString(wordleLetters[game.guessPos]).ToLower();

            if (wordGuess.Length != wordleColumns)
            {
                //MessageBox.Show($"Must submit a {wordleColumns} letter word", "Incomplete input", MessageBoxButton.OK, MessageBoxImage.Warning);
                UserMessageTextBlock.Text = $"Must submit a {wordleColumns} letter word";
                UserMessageTextBlock.Foreground = Brushes.Red;
                return;
            }

            // check if word guess is an actual word in database
            using (SQLiteConnection connection = new SQLiteConnection(App.DbConnectionString))
            {
                List<Models.Word> wordResult = connection.Table<Models.Word>().Where(x => x.Text.Equals(wordGuess.ToLower())).ToList<Models.Word>();
                if (wordResult.Count < 1)
                {
                    //MessageBox.Show($"Word not found in database. Must submit a valid word.", "Invalid word", MessageBoxButton.OK, MessageBoxImage.Warning);
                    UserMessageTextBlock.Text = $"Word not found in database. \nMust submit a valid word.";
                    UserMessageTextBlock.Foreground = Brushes.Red;
                    return;
                }
            }


            foreach (Letter letter in wordleLetters[game.guessPos])
            {
                letter.Character = letter.associatedTextBox.Text.ToLower()[0];
                letter.associatedTextBox.IsEnabled = false;


                if (checkIfLetterInWordInPosition(letter))
                {
                    letter.isCorrect = true;
                    letter.associatedTextBox.Background = Brushes.Green;
                    letter.associatedTextBox.Foreground = Brushes.White;
                }
                else if (checkifLetterInWord(letter.Character))
                {
                    letter.isInWord = true;
                    letter.associatedTextBox.Background = Brushes.Yellow;
                }
                else
                    letter.associatedTextBox.Background = Brushes.Gray;
            }


            if (game.secretWord == letterArrayToWordString(wordleLetters[game.guessPos]))
            {
                //MessageBox.Show($"You got it!", "Congrats!", MessageBoxButton.OK, MessageBoxImage.Information);
                UserMessageTextBlock.Text = $"You got it! Congrats!";
                UserMessageTextBlock.Foreground = Brushes.Green;
                return;
            }

            //MessageBox.Show($"The word is {wordGuess}", "Submitted", MessageBoxButton.OK, MessageBoxImage.Information);


            // setup next row (assuming everything was verified)
            if (wordleLetters[game.guessPos] != wordleLetters.Last())
            {
                foreach (Letter letter in wordleLetters[++game.guessPos])
                {
                    letter.associatedTextBox.IsEnabled = true;
                    letter.associatedTextBox.Background = Brushes.AliceBlue;
                    if (letter == wordleLetters[game.guessPos].First())
                        letter.associatedTextBox.Focus();
                }
            }
            else
            {
                //MessageBox.Show($"Better luck next time :/\nThe word is {game.secretWord}", "You lose", MessageBoxButton.OK, MessageBoxImage.Information);
                UserMessageTextBlock.Text = $"Better luck next time :/\nThe word is... {game.secretWord}";
                UserMessageTextBlock.Foreground = Brushes.Black;
                return;
            }

        }

        private bool checkifLetterInWord(char letter)
        {
            if (game.secretWord.Contains(letter))
                return true;
            return false;
        }

        private bool checkIfLetterInWordInPosition(Letter letter)
        {
            char[] secretLetters = game.secretWord.ToCharArray();
            if (secretLetters[letter.Position] == letter.Character)
                return true;
            return false;
        }

        private bool letterAlreadyFound()
        {

            return false;
        }

        private void LetterInput_TextChanged(object sender, RoutedEventArgs e)
        {
            DependencyObject senderObj = sender as DependencyObject;
            string senderName = senderObj.GetValue(FrameworkElement.NameProperty).ToString();
            string senderText = ((TextBox)sender).Text;
            if (senderName.Contains("letterText"))
            {
                int[] letterKeysTuple = wordleLettersReference[$"{senderName.GetHashCode()}"];

                if (letterKeysTuple[1] < wordleColumns - 1 && senderText.Length != 0)
                {
                    Letter letter = wordleLetters[letterKeysTuple[0]][letterKeysTuple[1] + 1];
                    letter.associatedTextBox.Focus();
                }

            }

        }

        private bool validateCurrentRowLength(Letter[] letters)
        {
            // check that all letters are in

            return true;
        }

        private string letterArrayToWordString(Letter[] letters)
        {

            string word = "";
            foreach (Letter letter in letters)
            {
                word += letter.associatedTextBox.Text.ToLower();
            }
            return word.Trim();
        }

        private Letter[] wordStringToLetterArray(string wordString)
        {
            Letter[] letters = new Letter[wordString.Length];
            char[] lettersArr = wordString.ToCharArray();

            int i = 0;
            foreach (char c in wordString.ToCharArray())
            {
                letters[i] = new Letter();
                letters[i].Character = c;
                letters[i].Position = i;
                i++;
            }
            return letters;
        }

        private void ReplayButton_Click(object sender, RoutedEventArgs e)
        {
            newGame();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show($"{e.U}", "", MessageBoxButton.OK, MessageBoxImage.Information);

            // Uri gameUri = new Uri("/Ava.xaml", UriKind.Relative);
            // this.NavigationService.Navigate(gameUri);
        }
    }
}
