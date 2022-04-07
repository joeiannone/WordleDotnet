using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Wordle.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            VersionLabel.Text = $"Version {Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
            MainTabControl.SelectedIndex = 0;

            if ((bool)Application.Current.Properties["dark_mode"])
            {
                this.Style = (Style)this.FindResource("darkModeWindowStyle");
                MainTabControl.Style = (Style)this.FindResource("darkModeTabStyle");
            }
                
        }
    }
}
