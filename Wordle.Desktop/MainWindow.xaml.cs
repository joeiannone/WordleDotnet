using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;


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
            MainTabControl.SelectedIndex = 0;
        }
    }
}
