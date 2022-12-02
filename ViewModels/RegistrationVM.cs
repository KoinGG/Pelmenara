using Avalonia.Controls;
using MessageBox.Avalonia;
using Pelmenara_AUI_RUI.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using MessageBox.Avalonia.Enums;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class RegistrationVM : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> SignUpAcceptCommand { get; }
        public ReactiveCommand<Window, Unit> SignInCommand { get; }

        private void SignUpAcceptCommandImpl()
        {

        }
        private void SignInCommandImpl(Window window)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            window.Close();
        }

        public RegistrationVM()
        {
            //SignUpAcceptCommand = ReactiveCommand.Create<>(SignUpAcceptCommand);
            SignInCommand = ReactiveCommand.Create<Window>(SignInCommandImpl);
        }
    }
}
