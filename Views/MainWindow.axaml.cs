using Avalonia.Controls;
using Pelmenara_AUI_RUI.ViewModels;
using System;

namespace Pelmenara_AUI_RUI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
        }
        public void aboba()
        {

        }
        //private void IsUserAuthorisedChangesImpl(object? sender, SelectionChangedEventArgs e)
        //{
        //    if (MainWindowViewModel.User != null)
        //    {
        //        text00.Text = "0000";
        //        text01.Text = "0101";
        //        text11.Text = "1111";
        //    }
        //}

    }
}
