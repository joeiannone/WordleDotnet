using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Wordle.Models;

namespace Wordle.Desktop
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class UserStatisticsPage : Page
    {
        private UserStatsService userStats;
        public UserStatisticsPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;

            WordleFactory wordle = new WordleFactory();
            userStats = (UserStatsService)wordle.GetWordleComponent("UserStats");
        }

        public async Task LoadUserStats()
        {

            UserStatsGrid.Children.Clear();
            List<UserStats> userStatsList = await userStats.GetUserStatsAsync(20);

            Label h1 = new Label();
            Label h2 = new Label();
            Label h3 = new Label();
            Label h4 = new Label();


            
            for (int i = 0; i < userStatsList.Count; i++)
            {
                int row = i + 1;
                RowDefinition rd = new RowDefinition();
                UserStatsGrid.RowDefinitions.Add(rd);
                Label word = new Label();
                Label guesses = new Label();
                Label ts = new Label();
                Label date = new Label();

                word.Content = userStatsList[i].Word;
                word.SetValue(Grid.ColumnProperty, 0);
                word.SetValue(Grid.RowProperty, row);

                guesses.Content = userStatsList[i].GuessCount;
                guesses.SetValue(Grid.ColumnProperty, 1);
                guesses.SetValue(Grid.RowProperty, row);

                ts.Content = GetTimespanDisplayString(userStatsList[i].TimeSpan);
                ts.SetValue(Grid.ColumnProperty, 2);
                ts.SetValue(Grid.RowProperty, row);

                date.Content = userStatsList[i].StartTime.ToString("d");
                date.SetValue(Grid.ColumnProperty, 3);
                date.SetValue(Grid.RowProperty, row);

                UserStatsGrid.Children.Add(word);
                UserStatsGrid.Children.Add(guesses);
                UserStatsGrid.Children.Add(ts);
                UserStatsGrid.Children.Add(date);
            }

            h1.Content = "Word";
            h2.Content = "Guesses";
            h3.Content = "Timer";
            h4.Content = "Date";
            h2.SetValue(Grid.ColumnProperty, 1);
            h3.SetValue(Grid.ColumnProperty, 2);
            h4.SetValue(Grid.ColumnProperty, 3);
            h1.FontWeight = FontWeight.FromOpenTypeWeight(700);
            h2.FontWeight = FontWeight.FromOpenTypeWeight(700);
            h3.FontWeight = FontWeight.FromOpenTypeWeight(700);
            h4.FontWeight = FontWeight.FromOpenTypeWeight(700);

            UserStatsGrid.RowDefinitions.Add(new RowDefinition());
            UserStatsGrid.Children.Add(h1);
            UserStatsGrid.Children.Add(h2);
            UserStatsGrid.Children.Add(h3);
            UserStatsGrid.Children.Add(h4);
        }

        public string GetTimespanDisplayString(TimeSpan timespan)
        {
            if (timespan.TotalSeconds >= 3600)
            {
                return string.Format("{0}h {1}m {2}s",
                    timespan.Hours,
                    timespan.Minutes,
                    timespan.Seconds
                );
            }
            else if (timespan.TotalSeconds >= 60)
            {
                return string.Format("{0}m {1}s",
                    timespan.Minutes,
                    timespan.Seconds
                );
            }
            else
            {
                return string.Format("{0}s", timespan.Seconds);
            }
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _ = LoadUserStats();
        }


    }
}
