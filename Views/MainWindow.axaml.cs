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
    }
}
