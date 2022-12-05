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
    public class RegistrationVM : ViewModelBase
    {
        public ReactiveCommand<Window, Unit> SignUpAcceptCommand { get; }
        public ReactiveCommand<Window, Unit> SignInCommand { get; }

        private User _user = new User();
        private string _password;

        private void SignUpAcceptCommandImpl(Window window)
        {

            if (User.Login != null && User.Email != null && User.Password != null)
            {
                if(User.Password != Password)
                {
                    MessageBoxManager.GetMessageBoxStandardWindow("Пароли не совпадают", "ашибка ашибка", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                    return;
                }
                else if(Helper.GetContext().Users.FirstOrDefault(x => x.Login == User.Login || x.Email == User.Email) != null)
                {
                    MessageBoxManager.GetMessageBoxStandardWindow("Пользователь с таким Login или Email уже существует", "ашибка ашибка", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                }

                try
                {
                    Helper.GetContext().Users.Add(User);
                    Helper.GetContext().SaveChanges();
                }
                catch
                {
                    MessageBoxManager.GetMessageBoxStandardWindow("Не удалось создать пользователя", "ашибка ашибка", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                }

                AuthWindow authWindow = new AuthWindow();
                authWindow.Show();
                window.Close();
            }
            else
            {
                MessageBoxManager.GetMessageBoxStandardWindow("Не заполнены все поля", "ашибка ашибка", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
            }
        }
        private void SignInCommandImpl(Window window)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            window.Close();
        }

        public string Password
        {
            get { return _password; }
            set { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        public User User
        {
            get { return _user; }
            set { this.RaiseAndSetIfChanged(ref _user, value); }
        }


        public RegistrationVM()
        {
            SignUpAcceptCommand = ReactiveCommand.Create<Window>(SignUpAcceptCommandImpl);
            SignInCommand = ReactiveCommand.Create<Window>(SignInCommandImpl);
        }
    }
}
