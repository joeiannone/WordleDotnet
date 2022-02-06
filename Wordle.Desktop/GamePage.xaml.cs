using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wordle;
using Wordle.Models;
using static Wordle.Models.Word;

namespace Wordle.Desktop
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private Game game;
        private Grid GameGrid;
        private Dictionary<string, Label> TextBoxes;
        private int ActiveColumn = 0;
        private BrushConverter BC = new BrushConverter();

        public GamePage()
        {
            InitializeComponent();
            InitGame();
            this.Loaded += GamePage_Loaded;
        }

        private void InitGame()
        {
            WordleFactory wordle = new WordleFactory();
            game = (Game)wordle.GetWordleComponent("Game");

            if (GameGrid != null)
            {
                // clear anything created dynamically
                GameGrid.Children.Clear();
                GameGrid.RowDefinitions.Clear();
                GameGrid.ColumnDefinitions.Clear();
            }
                
            BuildWordleGrid();
            UserMessageTextBlock.Content = "";
            ActiveColumn = 0;

            // make sure to remove before recreating key up listener
            this.KeyUp -= Page_KeyUp;
            this.KeyUp += Page_KeyUp;

            // Important
            FakeButton.Focus();
        }

        private void IncrementActiveColumn()
        {
            if (ActiveColumn < game.COLUMNS - 1)
                ActiveColumn++;
        }

        private void DecrementActiveColumn()
        {
            if (ActiveColumn > 0)
                ActiveColumn--;
        }


        private void BuildWordleGrid()
        {
            GameGrid = new Grid();
            TextBoxes = new Dictionary<string, Label>();

            // add column definitions
            for (int i = 0; i < game.COLUMNS; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                GameGrid.ColumnDefinitions.Add(col);
            }

            // add row definitions
            for (int i = 0; i < game.Rows; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                GameGrid.RowDefinitions.Add(row);
            }

            GameGrid.VerticalAlignment = VerticalAlignment.Top;

            List<TextBox> textBoxesList = new List<TextBox>();
            // row
            for (int i = 0; i < game.Rows; i++)
            {
                // col
                for (int j = 0; j < game.COLUMNS; j++)
                {
                    Label textBox = new Label();
                    textBox.FontSize = 24;
                    textBox.Height = 50;
                    textBox.Margin = new Thickness(2);
                    textBox.Padding = new Thickness(2);
                    //textBox.Background = Brushes.AliceBlue;
                    textBox.Name = $"x{i}x{j}";
                    textBox.BorderThickness = new Thickness(1);
                    textBox.BorderBrush = (Brush)BC.ConvertFrom("#D3D6dA");

                    if (j > 0)
                        textBox.SetValue(Grid.ColumnProperty, j);
                    if (i > 0)
                        textBox.SetValue(Grid.RowProperty, i);

                    if (i != game.CurrentRowPosition)
                    {
                        textBox.IsEnabled = false;
                        //textBox.Background = (Brush)BC.ConvertFrom("#D3D6dA");
                    }

                    textBox.HorizontalContentAlignment = HorizontalAlignment.Center;
                    textBox.VerticalContentAlignment = VerticalAlignment.Center;
                    TextBoxes.Add($"x{i}x{j}", textBox);
                    GameGrid.Children.Add(textBox);

                }

            }

            GamePanel.Children.Add(GameGrid);
        }

        private void ReplayButton_Click(object sender, RoutedEventArgs e)
        {
            InitGame();
        }

        private void SubmitGuess()
        {
            UserMessageTextBlock.Content = "";

            if (game.wordFound || game.CurrentRowPosition == game.COLUMNS + 1)
                return;

            string guessStr = "";
            for (int col = 0; col < game.COLUMNS; col++)
            {
                guessStr += TextBoxes[$"x{game.CurrentRowPosition}x{col}"].Content;
            }

            try
            {
                ValidatedWord guess = game.ValidateWord(guessStr);
                ValidatedWord guessResult = game.Guess(guess);
                UpdateGameGridState(guessResult);
                ActiveColumn = 0;
            }
            catch (InvalidOperationException ex)
            {
                UserMessageTextBlock.Content = ex.Message;
            }

        }

        private void UpdateGameGridState(ValidatedWord guessResult)
        {
            if (guessResult.ValidationMessages.Count != 0)
                return;

            for (int col = 0; col < game.COLUMNS; col++)
            {
                LetterState letterState = guessResult.LetterStates[$"{game.CurrentRowPosition}{col}"];
                Label textBox = TextBoxes[$"x{game.CurrentRowPosition}x{col}"];
                textBox.IsEnabled = false;
                textBox.Foreground = Brushes.White;
                textBox.BorderThickness = new Thickness(0);

                switch (letterState)
                {
                    case LetterState.isCorrect:
                        textBox.Background = (Brush)BC.ConvertFrom("#6AAA64");
                        break;

                    case LetterState.inWord:
                        textBox.Background = (Brush)BC.ConvertFrom("#C9B458");
                        break;

                    case LetterState.notInWord:
                        textBox.Background = (Brush)BC.ConvertFrom("#787C7E");
                        break;
                }
            }

            game.IncrementRowPosition();

            if (game.wordFound)
            {
                UserMessageTextBlock.Content = $"Congrats! You got it.\nYou found the word in {game.GetTimespanDisplayString()}";
                return;
            }
            else if (game.CurrentRowPosition == game.Rows)
            {
                UserMessageTextBlock.Content = $"You missed the mark :( \nThe word was {game.CurrentSecretWord.ToString()}. ";
                return;
            }

            for (int col = 0; col < game.COLUMNS; col++)
            {
                Label textBox = TextBoxes[$"x{game.CurrentRowPosition}x{col}"];
                //textBox.Background = Brushes.AliceBlue;
                textBox.BorderBrush = (Brush)BC.ConvertFrom("#D3D6dA");
                textBox.IsEnabled = true;
            }
        }

        private void Page_KeyUp(object sender, KeyEventArgs e)
        {
            // is a string with length 1 and is a letter
            if (e.Key.ToString().Length == 1 && char.IsLetter(char.Parse(e.Key.ToString())))
            {
                string letter = e.Key.ToString().ToUpper().Trim();
                Label activeTextBox = TextBoxes[$"x{game.CurrentRowPosition}x{ActiveColumn}"];
                activeTextBox.Content = letter;
                activeTextBox.BorderThickness = new Thickness(1);
                activeTextBox.BorderBrush = Brushes.Black;
                IncrementActiveColumn();
            }
            else if (e.Key == Key.Enter)
            {
                SubmitGuess();
            }
            else if (e.Key == Key.Back)
            {
                Label activeTextBox = TextBoxes[$"x{game.CurrentRowPosition}x{ActiveColumn}"];
                if (activeTextBox.Content == null)
                {
                    DecrementActiveColumn();
                    activeTextBox = TextBoxes[$"x{game.CurrentRowPosition}x{ActiveColumn}"];
                }
                activeTextBox.Content = null;
                //activeTextBox.BorderThickness = new Thickness(1);
                activeTextBox.BorderBrush = (Brush)BC.ConvertFrom("#D3D6dA");
            }


        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            SubmitGuess();
        }

        private void GamePage_Loaded(object sender, RoutedEventArgs e)
        {
            // focus this buttonfor the sake of key listener on page
            FakeButton.Focus();
        }
    }
}
