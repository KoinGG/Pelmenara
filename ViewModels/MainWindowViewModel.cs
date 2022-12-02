
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using System.Reactive;
using Avalonia.Controls;
using Pelmenara_AUI_RUI.Views;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactiveCommand<Window, Unit> SignInCommand { get; }
        private void SignInCommandImpl(Window window)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
        }
        public MainWindowViewModel()
        {
            SignInCommand = ReactiveCommand.Create<Window>(SignInCommandImpl);
        }
    }
}
