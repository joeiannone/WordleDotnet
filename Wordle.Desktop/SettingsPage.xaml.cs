using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Wordle.Models;

namespace Wordle.Desktop
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {

        private readonly int FormID = 1;
        private SettingsService settingsService;
        private List<Settings> formData;

        public SettingsPage()
        {
            InitializeComponent();
            Loaded += Page_Loaded;

            WordleFactory wordle = new WordleFactory();
            settingsService = (SettingsService)wordle.GetWordleComponent("Settings");

        }

        private void PopulateForm()
        {
            formData = settingsService.GetSettingsForm(FormID);
            foreach (Settings _s in formData)
            {   
                switch (_s.Name)
                {
                    case "hard_mode":
                        HardMode.IsChecked = _s.BooleanValue;
                        break;
                    case "high_contrast_mode":
                        HighContrastMode.IsChecked = _s.BooleanValue;
                        break;
                }
            }
        }

        private void Setting_Clicked(object sender, RoutedEventArgs e)
        {
            CheckBox setting = sender as CheckBox;
            MessageBoxResult? restartResult = null;
            switch (setting.Name)
            {
                case "HardMode":
                    Settings hard_mode = settingsService.GetSetting(FormID, "hard_mode");
                    hard_mode.BooleanValue = setting.IsChecked ?? false;
                    settingsService.SaveSettings(hard_mode);
                    break;
                case "HighContrastMode":
                    Settings high_contrast = settingsService.GetSetting(FormID, "high_contrast_mode");
                    high_contrast.BooleanValue = setting.IsChecked ?? false;
                    settingsService.SaveSettings(high_contrast);
                    restartResult = MessageBox.Show("\"High Contrast Mode\" requires an app restart to take effect.\n\nRestart now?", "Restart", MessageBoxButton.YesNoCancel, MessageBoxImage.Information);
                    break;
            }

            if (restartResult == MessageBoxResult.Yes)
            {
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
        }

        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateForm();
        }
    }
}
