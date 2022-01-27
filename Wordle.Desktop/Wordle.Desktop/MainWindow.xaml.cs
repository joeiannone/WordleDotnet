using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Wordle.Desktop
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        private Game game;
        private static int Columns = 5;
        private int Rows = 6;
        private Grid GameGrid;
        private Dictionary<string, TextBox> TextBoxes;

        public MainWindow()
        {
            this.InitializeComponent();
            InitGame();
            BuildWordleGrid();
        }

        private void InitGame()
        {
            //game = new Game(App.DbConnectionString, Rows);
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
                    textBox.Height = 40;
                    textBox.Margin = new Thickness(5);
                    textBox.Padding = new Thickness(5);
                    //textBox.Background = Brushes.AliceBlue;
                    textBox.Name = $"letterText{i}_{j}";
                    textBox.CharacterCasing = CharacterCasing.Upper;
                    //textBox.TextChanged += LetterInput_TextChanged;
                    //textBox.KeyDown += LetterInpout_KeyDown;


                    if (j > 0)
                        textBox.SetValue(Grid.ColumnProperty, j);
                    if (i > 0)
                        textBox.SetValue(Grid.RowProperty, i);

                    if (i != game.CurrentRowPosition)
                    {
                        textBox.IsEnabled = false;
                        //textBox.Background = Brushes.DarkGray;
                    }

                    // init wordleletters state
                    //wordleLetters[i][j] = new Letter();
                    //wordleLetters[i][j].associatedTextBox = textBox;
                    //wordleLetters[i][j].Row = i;
                    //wordleLetters[i][j].Position = j;
                    //wordleLettersReference.Add($"{textBox.Name.GetHashCode()}", new int[2] { i, j });

                    TextBoxes.Add($"{i}{j}", textBox);

                    GameGrid.Children.Add(textBox);

                }

            }

            GamePanel.Children.Add(GameGrid);

            //wordleLetters[0][0].associatedTextBox.Focus();
        }
    }
}
