using Avalonia.Controls;
using Pelmenara_AUI_RUI.Sourses;
using Pelmenara_AUI_RUI.Views;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class AuthVM : ViewModelBase
    {
        #region [Private Fields]

        private string _login;
        private string _password;

        #endregion

        public AuthVM()
        {
            SignInAcceptCommand = ReactiveCommand.Create<Window>(SignInAcceptCommandImpl);
            SignUpCommand = ReactiveCommand.Create<Window>(SignUpCommandImpl);
        }

        #region [Properties]

        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _login, value);
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _password, value);
            }
        }

        #region [Commands Declaration]

        public ReactiveCommand<Window, Unit> SignInAcceptCommand { get; }
        public ReactiveCommand<Window, Unit> SignUpCommand { get; }

        #endregion

        #endregion

        #region [Methods]

        private void SignInAcceptCommandImpl(Window window)
        {
            User? user = DbContextProvider.GetContext().Users.FirstOrDefault(x => x.Login == Login && x.Password == Password);

            if(user != null)
            {
                MainWindowVM.CurrentUser = user;
                window.Close();
            }
            else
            {
                ErrorMessage.ShowErrorMessage(window, "Пользователь устранён");
            }
        }
        private async void SignUpCommandImpl(Window window)
        {
            var registrationWindow = new RegistrationWindow();

            try
            {
                await registrationWindow.ShowDialog(window).WaitAsync(TimeSpan.FromMinutes(60));
            }
            catch
            {
                ErrorMessage.ShowErrorMessage(window, "Время регистрации истекло");
                registrationWindow.Close();
            }
        }

        #endregion
    }
}
