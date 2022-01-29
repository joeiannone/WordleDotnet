﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wordle;
using Wordle.Models;

namespace Wordle.Desktop
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private Game game;
        private Grid GameGrid;
        private Dictionary<string, TextBox> TextBoxes;

        public GamePage()
        {
            InitializeComponent();
            InitGame();
        }

        private void InitGame()
        {
            game = new Game();
            if (GameGrid != null)
                GameGrid.Children.Clear();
            BuildWordleGrid();
            UserMessageTextBlock.Text = game.CurrentSecretWord.ToString();
        }


        private void BuildWordleGrid()
        {
            GameGrid = new Grid();
            TextBoxes = new Dictionary<string, TextBox>();

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
                    TextBox textBox = new TextBox();

                    textBox.FontSize = 24;
                    textBox.MaxLength = 1;
                    textBox.Height = 50;
                    textBox.Margin = new Thickness(2);
                    textBox.Padding = new Thickness(2);
                    textBox.Background = Brushes.AliceBlue;
                    textBox.Name = $"x{i}x{j}";
                    textBox.CharacterCasing = CharacterCasing.Upper;
                    textBox.TextChanged += LetterInput_TextChanged;
                    textBox.KeyUp += LetterInput_KeyUp;


                    if (j > 0)
                        textBox.SetValue(Grid.ColumnProperty, j);
                    if (i > 0)
                        textBox.SetValue(Grid.RowProperty, i);

                    if (i != game.CurrentRowPosition)
                    {
                        textBox.IsEnabled = false;
                        textBox.Background = Brushes.DarkGray;
                    }

                    textBox.HorizontalContentAlignment = HorizontalAlignment.Center;
                    textBox.VerticalContentAlignment = VerticalAlignment.Center;
                    TextBoxes.Add($"x{i}x{j}", textBox);
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

        private void SubmitGuess()
        {
            string guessStr = "";
            for (int col = 0; col < game.COLUMNS; col++)
            {
                guessStr += $"{TextBoxes[$"x{game.CurrentRowPosition}x{col}"].Text}";
            }
            

            try
            {
                ValidatedWord guess = game.ValidateWord(guessStr);
                ValidatedWord guessResult = game.Guess(guess);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
            }
           
            

        }

        private void UpdateGridState(ValidatedWord guessResult)
        {
            // TODO
        }



        private string IncrementTextBoxColumnKey(string key)
        {
            string[] keyArr = key.Split(new[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
            
            if (keyArr.Length != 2)
                throw new InvalidOperationException("Invalid key");
            string type = keyArr[0];
            int row = Int32.Parse(keyArr[0]);
            int col = Int32.Parse(keyArr[1]);

            if (col < (game.COLUMNS - 1))
                col++;

            return $"x{row}x{col}";
        }

        private string DecrementTextBoxColumnKey(string key)
        {
            string[] keyArr = key.Split(new[] { "x" }, StringSplitOptions.RemoveEmptyEntries);

            if (keyArr.Length != 2)
                throw new InvalidOperationException("Invalid key");
            string type = keyArr[0];
            int row = Int32.Parse(keyArr[0]);
            int col = Int32.Parse(keyArr[1]);

            if (col > 0)
                col--;

            return $"x{row}x{col}";
        }

        /**
         * 
         */
        private void LetterInput_TextChanged(object sender, RoutedEventArgs e)
        {
            DependencyObject senderObj = sender as DependencyObject;
            string senderName = senderObj.GetValue(FrameworkElement.NameProperty).ToString();
            string senderText = ((TextBox)sender).Text;

            if (senderText.Length == 0)
                return;

            try
            {
                string incrementedKey = IncrementTextBoxColumnKey(senderName);

                if (incrementedKey != senderName)
                    TextBoxes[incrementedKey].Focus();
                
            } 
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /**
         * 
         */
        private void LetterInput_KeyUp(object sender, KeyEventArgs e)
        {
            DependencyObject senderObj = sender as DependencyObject;
            string senderName = senderObj.GetValue(FrameworkElement.NameProperty).ToString();
            string senderText = ((TextBox)sender).Text;

            Debug.WriteLine(e.Key.GetType().Name);
            if (e.Key == Key.Return)
            {
                SubmitGuess();
            }
            else if (e.Key == Key.Back)
            {
                // TODO: backspace through inputs (like shift + tab)
                try
                {
                    if (senderText.Length == 0)
                    {
                        string decrementedKey = DecrementTextBoxColumnKey(senderName);
                        TextBoxes[decrementedKey].Focus();
                        TextBoxes[decrementedKey].Text = "";
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            SubmitGuess();
        }
    }
}