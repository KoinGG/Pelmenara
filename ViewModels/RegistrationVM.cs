
using Avalonia.Controls;
using Pelmenara_AUI_RUI.Sourses;
using ReactiveUI;
using System.Linq;
using System.Reactive;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class RegistrationVM : ViewModelBase
    {
        #region [Private Fields]

        private User _user = new User();
        private string _password;

        #endregion

        public RegistrationVM()
        {
            SignUpAcceptCommand = ReactiveCommand.Create<Window>(SignUpAcceptCommandImpl);
            CancelCommand = ReactiveCommand.Create<Window>(CancelCommandImpl);
        }

        #region [Properties]

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
        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _user, value);
            }
        }

        #region [Commands Declaration]

        public ReactiveCommand<Window, Unit> SignUpAcceptCommand { get; }
        public ReactiveCommand<Window, Unit> CancelCommand { get; }

        #endregion

        #endregion

        #region [Methods]

        private void SignUpAcceptCommandImpl(Window window)
        {
            SignUpValidation(window);

            try
            {
                DbContextProvider.GetContext().Users.Add(User);
                DbContextProvider.GetContext().SaveChanges();
            }
            catch
            {
                ErrorMessage.ShowErrorMessage(window, "Не удалось создать пользователя");
            }

            window.Close();
        }

        private void CancelCommandImpl(Window window)
        {
            window.Close();
        }

        private void SignUpValidation(Window window)
        {
            if (User.Login == null || User.Email == null || User.Password == null
             || User.Login == ""   || User.Email == ""   || User.Password == ""
             || User.Login == " "  || User.Email == " "  || User.Password == " ")
            {
                ErrorMessage.ShowErrorMessage(window, "Не заполнены все поля");                
                return;
            }
            if (User.Login.Length < 4)
            {
                ErrorMessage.ShowErrorMessage(window, "Логин должен иметь больше трёх символов");
                return;
            }
            if (User.Password.Length < 6)
            {
                ErrorMessage.ShowErrorMessage(window, "Пароль должен иметь не меньше 6 символов");
                return;
            }
            if (User.Password != Password)
            {
                ErrorMessage.ShowErrorMessage(window, "Пароли не совпадают");
                return;
            }

            var ifUserExist = DbContextProvider.GetContext().Users.FirstOrDefault(x => x.Login == User.Login || x.Email == User.Email); // Если null значит не существует

            if (ifUserExist != null)
            {
                ErrorMessage.ShowErrorMessage(window, "Пользователь с таким Login или Email уже существует");
                return;
            }
            if (!User.Email.Contains("@") || !User.Email.Contains("."))
            {
                ErrorMessage.ShowErrorMessage(window, "Не корректный Email");
                return;
            }
        }

        #endregion
    }
}
