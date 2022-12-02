using Avalonia.Controls;
using Pelmenara_AUI_RUI.ViewModels;
using System;

namespace Pelmenara_AUI_RUI.Views
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            DataContext = new RegistrationVM();
            InitializeComponent();
        }
    }
}
