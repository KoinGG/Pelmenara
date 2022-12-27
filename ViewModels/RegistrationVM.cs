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
using System.Text.RegularExpressions;
using Avalonia.Data;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class RegistrationVM : ViewModelBase
    {
        public ReactiveCommand<Window, Unit> SignUpAcceptCommand { get; }
        public ReactiveCommand<Window, Unit> CancelCommand { get; }

        private User _user = new User();
        private string _password;

        private void SignUpAcceptCommandImpl(Window window)
        {
            if(User.Login == null || User.Email == null || User.Password == null
            || User.Login == ""   || User.Email == ""   || User.Password == ""
            || User.Login == " "  || User.Email == " "  || User.Password == " ")
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Не заполнены все поля", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }
            if(User.Login.Length < 4)
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Логин должен иметь больше трёх символов", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }
            if(User.Password.Length < 6)
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Пароль должен иметь не меньше 6 символов", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }
            if(User.Password != Password)
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Пароли не совпадают", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }
            if(Helper.GetContext().Users.FirstOrDefault(x => x.Login == User.Login || x.Email == User.Email) != null)
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Пользователь с таким Login или Email уже существует", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }
            if(!User.Email.Contains("@") || !User.Email.Contains("."))
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Не корректный Email", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }

            try
            {
                Helper.GetContext().Users.Add(User);
                Helper.GetContext().SaveChanges();
            }
            catch
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Не удалось создать пользователя", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }

            window.Close();
        }

        public bool SignUpAcceptCommandImpl(string Login, string Password, string Password2, string Email)
        {
            if (Login == null || Email == null || Password == null
             || Login == ""   || Email == ""   || Password == ""
             || Login == " "  || Email == " "  || Password == " ")
            {
                return false;
            }
            else if (Login.Length < 4)
            {                
                return false;
            }
            else if (Password.Length < 6)
            {
                return false;
            }
            else if (Password != Password2)
            {
                return false;
            }
            else if (Helper.GetContext().Users.FirstOrDefault(x => x.Login == Login || x.Email == Email) != null)
            {
                return false;
            }
            else if (!Email.Contains("@") || !Email.Contains("."))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CancelCommandImpl(Window window)
        {
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
            CancelCommand = ReactiveCommand.Create<Window>(CancelCommandImpl);
        }
    }
}
