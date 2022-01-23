using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WordleMacClient
{
    public partial class MainPage : ContentPage
    {
        public int ClickCount = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        void ReplayButton_Click(System.Object sender, System.EventArgs e)
        {
            ClickCount++;
            MainLabel.Text = $"Button clicked {ClickCount} times";
        }

        void SubmitButton_Click(System.Object sender, System.EventArgs e)
        {
            ClickCount++;
            MainLabel.Text = $"Button clicked {ClickCount} times";
        }
    }
}
