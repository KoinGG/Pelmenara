
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Pelmenara_AUI_RUI.Views;
using Pelmenara_AUI_RUI.Sourses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string? _text00 = "00";
        private string? _text01 = "01";
        private string? _text11 = "11";

        public static User User { get; set; } = new User();
        public ReactiveCommand<MainWindow, Unit> SignInCommand { get; }
        public ReactiveCommand<MainWindow, Unit> SignOutCommand { get; }
        public ReactiveCommand<MainWindow, Unit> AddRecipeCommand { get; }
        public ReactiveCommand<Unit, Unit> IsUserAuthorisedChanges { get; }
        //public ReactiveCommand<MainWindow, Unit> SomeCommand { get; }

        public string? Text00
        {
            get => _text00;
            set
            {
                this.RaiseAndSetIfChanged(ref _text00, value);
            }
        }
        public string? Text01
        {
            get => _text01;
            set
            {
                this.RaiseAndSetIfChanged(ref _text01, value);
            }
        }
        public string? Text11
        {
            get => _text11;
            set
            {
                this.RaiseAndSetIfChanged(ref _text11, value);
            }
        }


        private async void SignInCommandImpl(MainWindow window)
        {
            AuthWindow authWindow = new AuthWindow();
            try
            {
                await authWindow.ShowDialog(window).WaitAsync(TimeSpan.FromMinutes(60));
            }
            catch
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ќшибќчка", "¬рем€ авторизации истекло", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                authWindow.Close();
            }
            finally
            {
                IfUserAuthorisedChangesImpl(window);
            }
        }
        private void IfUserAuthorisedChangesImpl(MainWindow window)
        {
            if (User.UserId != 0)
            {
                Text00 = "0000";
                Text01 = "0101";
                Text11 = "1111";
                window.Title = $"Pelmenara: {User.Login}";

                window.btn_SignIn.IsEnabled = false;
                window.btn_SignIn.IsVisible = false;

                window.btn_SignOut.IsEnabled = true;
                window.btn_SignOut.IsVisible = true;

                window.btn_AddRecipe.IsEnabled = true;
                window.btn_AddRecipe.IsVisible = true;

                window.btn_ChangeRecipe.IsEnabled = true;
                window.btn_ChangeRecipe.IsVisible = true;

                window.btn_DeleteRecipe.IsEnabled = true;
                window.btn_DeleteRecipe.IsVisible = true;
            }
        }

        private void SignOutCommandImpl(MainWindow window)
        {
            if (User.UserId != 0)
            {              
                User.UserId = -1;
                Text00 = "00";
                Text01 = "01";
                Text11 = "11";
                window.Title = "Pelmenara";
                window.btn_SignIn.IsEnabled = true;
                window.btn_SignIn.IsVisible = true;
                window.btn_SignOut.IsEnabled = false;
                window.btn_SignOut.IsVisible = false;
            }
            
        }

        private void AddRecipeCommandImpl(MainWindow window)
        {

        }

        //private void SomeCommandImpl(MainWindow window)
        //{
        //    Text00 = "000000";
        //    Text01 = "010101";
        //    Text11 = "111111";
        //}

        public MainWindowViewModel()
        {
            //SomeCommand = ReactiveCommand.Create<MainWindow>(SomeCommandImpl);
            SignInCommand = ReactiveCommand.Create<MainWindow>(SignInCommandImpl);
            SignOutCommand = ReactiveCommand.Create<MainWindow>(SignOutCommandImpl);
            AddRecipeCommand = ReactiveCommand.Create<MainWindow>(AddRecipeCommandImpl);
        }
    }
}
