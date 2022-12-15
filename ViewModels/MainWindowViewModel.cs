
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
using System.Collections.ObjectModel;
using System.IO;
namespace Pelmenara_AUI_RUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string? _text00 = "00";
        private string? _text01 = "01";
        private string? _text11 = "11";

        private MainWindow _mainWindow;
        private Recipe _recipe;
        public static User User { get; set; } = new User();

        public ReactiveCommand<MainWindow, Unit> SignInCommand { get; }
        public ReactiveCommand<MainWindow, Unit> LogOutCommand { get; }
        public ReactiveCommand<Window, Unit> AddRecipeCommand { get; }
        public ReactiveCommand<Unit, Unit> IsUserAuthorisedChanges { get; }
        public ReactiveCommand<MainWindow, Unit> SomeCommand { get; }

        private ObservableCollection<Recipe> _recipes;

        public ObservableCollection<Recipe> Recipes
        {
            get { return _recipes; }
            set 
            {
                this.RaiseAndSetIfChanged(ref _recipes, value);
            }
        }

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

                window.btn_LogOut.IsEnabled = true;
                window.btn_LogOut.IsVisible = true;

                window.btn_AddRecipe.IsEnabled = true;
                window.btn_AddRecipe.IsVisible = true;

                window.btn_ChangeRecipe.IsEnabled = true;
                window.btn_ChangeRecipe.IsVisible = true;

                window.btn_DeleteRecipe.IsEnabled = true;
                window.btn_DeleteRecipe.IsVisible = true;
            }
        }

        private void LogOutCommandImpl(MainWindow window)
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

                window.btn_LogOut.IsEnabled = false;
                window.btn_LogOut.IsVisible = false;
            }
            
        }

        public Recipe IfRecipeSelected
        {
            get { return _recipe; }
            set 
            {
                this.RaiseAndSetIfChanged(ref _recipe, value);
                SomeCommandImpl(_mainWindow);
            }
        }

        private async void AddRecipeCommandImpl(Window window)
        {
            AddRecipeWindow addRecipeWindow = new AddRecipeWindow();
            await addRecipeWindow.ShowDialog(window).WaitAsync(TimeSpan.FromMinutes(60));
        }

        private void SomeCommandImpl(MainWindow window)
        {
            Text00 = "000000";
            Text01 = "010101";
            Text11 = "111111";
        }

        public MainWindowViewModel(MainWindow window)
        {
            _mainWindow = window;
            Recipes = new ObservableCollection<Recipe>(Helper.GetContext().Recipes.ToList());

            SomeCommand = ReactiveCommand.Create<MainWindow>(SomeCommandImpl);
            SignInCommand = ReactiveCommand.Create<MainWindow>(SignInCommandImpl);
            LogOutCommand = ReactiveCommand.Create<MainWindow>(LogOutCommandImpl);
            AddRecipeCommand = ReactiveCommand.Create<Window>(AddRecipeCommandImpl);
        }
    }
}
