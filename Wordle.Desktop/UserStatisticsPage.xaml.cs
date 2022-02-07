using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wordle.Models;

namespace Wordle.Desktop
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class UserStatisticsPage : Page
    {
        private UserStatsService userStatsService;
        public UserStatisticsPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;

            WordleFactory wordle = new WordleFactory();
            userStatsService = (UserStatsService)wordle.GetWordleComponent("UserStats");
        }

        public async Task LoadUserStats()
        {

            UserStatsGrid.Children.Clear();
            Played.Content = await userStatsService.GetGamesPlayedAsync();
            WinPercentage.Content = await userStatsService.GetWinPercentageAsync();
            CurrentStreak.Content = await userStatsService.GetCurrentStreakAsync();
            MaxStreak.Content = await userStatsService.GetMaxStreakAsync();

            Dictionary<int, int> guessDistributions = await userStatsService.GetGuessDistributionAsync();
            GuessDistributionGrid.Children.Clear();
            GuessDistributionGrid.RowDefinitions.Clear();

            Random rand = new Random();
            // making sure this outputs in order of key (number of guesses)
            for (int i = 1; i < guessDistributions.Count + 1; i++)
            {
                if (guessDistributions.ContainsKey(i))
                {
                    RowDefinition row = new RowDefinition();
                    Label kLabel = new Label();
                    Label vLabel = new Label();
                    kLabel.Content = $"{i}";
                    kLabel.SetValue(Grid.RowProperty, i-1);
                    kLabel.SetValue(Grid.ColumnProperty, 0);
                    kLabel.Margin = new Thickness { Bottom = 2, Left = 0, Top = 0, Right = 0 };
                    vLabel.Content = guessDistributions[i];
                    vLabel.SetValue(Grid.RowProperty, i - 1);
                    vLabel.SetValue(Grid.ColumnProperty, 1);
                    vLabel.Background = Brushes.DodgerBlue;
                    vLabel.Foreground = Brushes.White;
                    vLabel.Width = 2*UserStatsService.GetDistributionPercentage(guessDistributions[i], guessDistributions[guessDistributions.Count]);
                    vLabel.HorizontalAlignment = HorizontalAlignment.Left;
                    vLabel.HorizontalContentAlignment = HorizontalAlignment.Right;
                    vLabel.Margin = new Thickness { Bottom = 2, Left = 0, Top = 0, Right = 0 };
                    GuessDistributionGrid.RowDefinitions.Add(row);
                    GuessDistributionGrid.Children.Add(kLabel);
                    GuessDistributionGrid.Children.Add(vLabel);
                }
            }
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
