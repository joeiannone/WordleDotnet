using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wordle.Models;

namespace Wordle.Desktop
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class UserStatisticsPage : Page
    {
        public UserStatisticsPage()
        {
            InitializeComponent();
            _ = LoadUserStats();
        }

        public async Task LoadUserStats()
        {
            Game game = new Game();
            //UserStatisticsDataGrid.ItemsSource = await game.GetUserStatsAsync(20);
            List<UserStats> userStatsList = await game.GetUserStatsAsync(20);
            for (int i = 0; i < userStatsList.Count; i++)
            {
                RowDefinition rd = new RowDefinition();
                UserStatsGrid.RowDefinitions.Add(rd);
                Label word = new Label();
                Label guesses = new Label();
                Label ts = new Label();

                word.Content = userStatsList[i].Word;
                word.SetValue(Grid.ColumnProperty, 0);
                word.SetValue(Grid.RowProperty, i);

                guesses.Content = userStatsList[i].GuessCount;
                guesses.SetValue(Grid.ColumnProperty, 1);
                guesses.SetValue(Grid.RowProperty, i);

                ts.Content = userStatsList[i].TimeSpan;
                ts.SetValue(Grid.ColumnProperty, 2);
                ts.SetValue(Grid.RowProperty, i);

                UserStatsGrid.Children.Add(word);
                UserStatsGrid.Children.Add(guesses);
                UserStatsGrid.Children.Add(ts);



            }
        }


    }
}
