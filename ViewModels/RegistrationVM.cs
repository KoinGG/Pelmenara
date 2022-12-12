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

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class RegistrationVM : ViewModelBase
    {
        public ReactiveCommand<Window, Unit> SignUpAcceptCommand { get; }
        public ReactiveCommand<Window, Unit> CancelCommand { get; }

        private User _user = new User();
        private string _password;
        private string _email;

        private void SignUpAcceptCommandImpl(Window window)
        {

            if (User.Login != null && User.Email != null && User.Password != null)
            {
                if(User.Login.Length >= 3 && User.Email.Contains("@") && User.Password.Length >= 6)
                {
                    if(User.Password != Password)
                    {
                        MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Пароли не совпадают", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                        return;
                    }
                    else if(Helper.GetContext().Users.FirstOrDefault(x => x.Login == User.Login || x.Email == User.Email) != null)
                    {
                        MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Пользователь с таким Login или Email уже существует", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
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
                else
                {
                    MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Хуита", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                }
            }
            else
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Не заполнены все поля", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
            }
        }

        private void CancelCommandImpl(Window window)
        {
            window.Close();
        }

        public string Password
        {
            get { return _password; }
            set 
            { this.RaiseAndSetIfChanged(ref _password, value); }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                {
                    throw new DataValidationException("Invalid email");
                }
                this.RaiseAndSetIfChanged(ref _email, value);
            }
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
