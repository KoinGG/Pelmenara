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
using Pelmenara_AUI_RUI.Sourses;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class AuthVM : ViewModelBase
    {
        private string _login;
        private string _password;
        public ReactiveCommand<Window, Unit> SignInAcceptCommand { get; }
        public ReactiveCommand<Window, Unit> SignUpCommand { get; }

        public string Login
        {
            get { return _login; }
            set 
            {
                this.RaiseAndSetIfChanged(ref _login, value);
            }
        }
        public string Password
        {
            get { return _password; }
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        private void SignInAcceptCommandImpl(Window window)
        {
            var user = Helper.GetContext().Users.FirstOrDefault(x => x.Login == Login && x.Password == Password);
            if(user != null)
            {
                MainWindowViewModel.User = user;
                window.Close();
            }
            else
            {
                MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Пользователь устранён", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
            }
        }

        private async void SignUpCommandImpl(Window window)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            await registrationWindow.ShowDialog(window).WaitAsync(TimeSpan.FromMinutes(60));
        }

        public AuthVM()
        {
            SignInAcceptCommand = ReactiveCommand.Create<Window>(SignInAcceptCommandImpl);
            SignUpCommand = ReactiveCommand.Create<Window>(SignUpCommandImpl);
        }
    }
}
