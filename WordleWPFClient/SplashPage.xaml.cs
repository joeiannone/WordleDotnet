using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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

namespace WordleWPFClient
{
    /// <summary>
    /// Interaction logic for SplashPage.xaml
    /// </summary>
    public partial class SplashPage : Page
    {
        public SplashPage()
        {
            InitializeComponent();
            
            Loaded += SplashPage_Loaded;

        }

        private async void SplashPage_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(2000);
            Uri gameUri = new Uri("/GamePage.xaml", UriKind.Relative);
            this.NavigationService.Navigate(gameUri);
        }



    }
}
