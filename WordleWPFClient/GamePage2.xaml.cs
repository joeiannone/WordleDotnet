using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wordle;

namespace WordleWPFClient
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class GamePage2 : Page
    {
        public Game game;
        public GamePage2()
        {
            InitializeComponent();
            //game = new Game(App.WordsDbPath);
        }
    }
}
