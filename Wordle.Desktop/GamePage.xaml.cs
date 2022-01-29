using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wordle;

namespace Wordle.Desktop
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private Game game;
        private static int Columns = 5;
        private int Rows = 6;
        private Grid GameGrid;
        private Dictionary<string, TextBox> TextBoxes;

        public GamePage()
        {
            InitializeComponent();
            InitGame();
            BuildWordleGrid();
        }

        private void InitGame()
        {
            game = new Game(Rows);
            UserMessageTextBlock.Text = game.CurrentSecretWord.ToString();
        }


        private void BuildWordleGrid()
        {
            GameGrid = new Grid();
            TextBoxes = new Dictionary<string, TextBox>();

            // add column definitions
            for (int i = 0; i < Columns; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);

                GameGrid.ColumnDefinitions.Add(col);

            }

            // add row definitions
            for (int i = 0; i < Rows; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                GameGrid.RowDefinitions.Add(row);
            }



            GameGrid.VerticalAlignment = VerticalAlignment.Top;

            List<TextBox> textBoxesList = new List<TextBox>();
            // row
            for (int i = 0; i < Rows; i++)
            {
                // col
                for (int j = 0; j < Columns; j++)
                {
                    TextBox textBox = new TextBox();

                    textBox.FontSize = 24;
                    textBox.MaxLength = 1;
                    textBox.Height = 50;
                    //textBox.Width = 40;
                    textBox.Margin = new Thickness(2);
                    textBox.Padding = new Thickness(2);
                    textBox.Background = Brushes.AliceBlue;
                    textBox.Name = $"letterText{i}_{j}";
                    textBox.CharacterCasing = CharacterCasing.Upper;
                    textBox.TextChanged += LetterInput_TextChanged;
                    //textBox.KeyDown += LetterInput_KeyDown;


                    if (j > 0)
                        textBox.SetValue(Grid.ColumnProperty, j);
                    if (i > 0)
                        textBox.SetValue(Grid.RowProperty, i);

                    if (i != game.CurrentRowPosition)
                    {
                        textBox.IsEnabled = false;
                        textBox.Background = Brushes.DarkGray;
                    }

                    // init wordleletters state
                    //wordleLetters[i][j] = new Letter();
                    //wordleLetters[i][j].associatedTextBox = textBox;
                    //wordleLetters[i][j].Row = i;
                    //wordleLetters[i][j].Position = j;
                    //wordleLettersReference.Add($"{textBox.Name.GetHashCode()}", new int[2] { i, j });
                    textBox.HorizontalContentAlignment = HorizontalAlignment.Center;
                    textBox.VerticalContentAlignment = VerticalAlignment.Center;
                    TextBoxes.Add($"{i}{j}", textBox);
                    GameGrid.Children.Add(textBox);

                }

            }

            GamePanel.Children.Add(GameGrid);

            //wordleLetters[0][0].associatedTextBox.Focus();
        }

        private void ReplayButton_Click(object sender, RoutedEventArgs e)
        {
            InitGame();
        }

        private void LetterInput_TextChanged(object sender, RoutedEventArgs e)
        {
            DependencyObject senderObj = sender as DependencyObject;
            string senderName = senderObj.GetValue(FrameworkElement.NameProperty).ToString();
            string senderText = ((TextBox)sender).Text;

            /*
            if (senderName.Contains("letterText"))
            {
                int[] letterKeysTuple = wordleLettersReference[$"{senderName.GetHashCode()}"];

                if (letterKeysTuple[1] < wordleColumns - 1 && senderText.Length != 0)
                {
                    Letter letter = wordleLetters[letterKeysTuple[0]][letterKeysTuple[1] + 1];
                    letter.associatedTextBox.Focus();
                }

            }
            */

        }
    }
}
