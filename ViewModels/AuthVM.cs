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
                //_login = value;
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
            // заглушка (пока нет БД)
            if(Login == "123" && Password == "456")
            {
                MainWindow mainWindow= new MainWindow();
                mainWindow.Show();
                window.Close();
            }
            else
            {
                MessageBoxManager.GetMessageBoxStandardWindow("Пользователь устранён", "ашибка ашибка", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
            }
        }
        private void SignUpCommandImpl(Window window)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            window.Close();
        }
        public AuthVM()
        {
            SignInAcceptCommand = ReactiveCommand.Create<Window>(SignInAcceptCommandImpl);
            SignUpCommand = ReactiveCommand.Create<Window>(SignUpCommandImpl);
        }
    }
}
