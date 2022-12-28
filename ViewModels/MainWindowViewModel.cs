
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
        public static User User { get; set; } = new User();

        #region [Commands Declaration]

        public ReactiveCommand<MainWindow, Unit> SignInCommand { get; }
        public ReactiveCommand<MainWindow, Unit> LogOutCommand { get; }
        public ReactiveCommand<Window, Unit> AddRecipeCommand { get; }
        public ReactiveCommand<Window, Unit> FavoriteRecipesCommand { get; }
        public ReactiveCommand<Window, Unit> SearchCommand { get; }

        #endregion

        #region [Private Properties]

        private MainWindow _mainWindow;
        private Recipe _recipe;
        private ObservableCollection<Recipe> _recipes;
        private ObservableCollection<FavoriteRecipe> _favoriteRecipes;
        private int _selectedFilter;
        private string _searchQuery;

        #endregion

        public ObservableCollection<Recipe> Recipes
        {
            get { return _recipes; }
            set
            {
                this.RaiseAndSetIfChanged(ref _recipes, value);
            }
        }
        public ObservableCollection<FavoriteRecipe> FavoriteRecipes
        {
            get { return _favoriteRecipes; }
            set
            {
                this.RaiseAndSetIfChanged(ref _favoriteRecipes, value);
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
                MessageBoxManager.GetMessageBoxStandardWindow("Ошибка", "Время авторизации истекло", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                authWindow.Close();
            }
            finally
            {
                IfUserAuthorisedChangesImpl(window);
            }
        }

        private void LogOutCommandImpl(MainWindow window)
        {
            if (User.UserId != 0)
            {
                User.UserId = -1;

                window.Title = "Pelmenara";

                window.btn_SignIn.IsEnabled = true;
                window.btn_SignIn.IsVisible = true;

                window.btn_LogOut.IsEnabled = false;
                window.btn_LogOut.IsVisible = false;

                window.btn_AddRecipe.IsEnabled = false;

                window.btn_FavoriteRecipes.IsEnabled = false;
            }

        }

        private async void AddRecipeCommandImpl(Window window)
        {
            AddRecipeWindow addRecipeWindow = new AddRecipeWindow();
            await addRecipeWindow.ShowDialog(window).WaitAsync(CancellationToken.None);

            Recipes = new ObservableCollection<Recipe>(Helper.GetContext().Recipes.OrderByDescending(x => x.CreationDate).ToList());
        }

        private async void FavoriteRecipesCommandImpl(Window window)
        {
            FavoriteRecipeWindow favoriteRecipeWindow = new FavoriteRecipeWindow();
            await favoriteRecipeWindow.ShowDialog(window).WaitAsync(CancellationToken.None);

            Recipes = new ObservableCollection<Recipe>(Helper.GetContext().Recipes.OrderByDescending(x => x.CreationDate).ToList());
        }

        private void SearchCommandImpl(Window window)
        {
            var search = Helper.GetContext().Recipes.Where(x => x.Title.Contains(_searchQuery) || x.Description.Contains(_searchQuery) || x.Ingredients.Contains(_searchQuery)).ToList();

            if (_selectedFilter == 0)
            {
                Recipes = new ObservableCollection<Recipe>(search.OrderByDescending(x => x.CreationDate));
            }
            else if (_selectedFilter == 1)
            {
                Recipes = new ObservableCollection<Recipe>(search.OrderBy(x => x.CreationDate));
            }
        }

        private void IfUserAuthorisedChangesImpl(MainWindow window)
        {
            if (User.UserId != 0)
            {
                window.Title = $"Pelmenara: {User.Login}";

                window.btn_SignIn.IsEnabled = false;
                window.btn_SignIn.IsVisible = false;

                window.btn_LogOut.IsEnabled = true;
                window.btn_LogOut.IsVisible = true;

                window.btn_AddRecipe.IsEnabled = true;

                window.btn_FavoriteRecipes.IsEnabled = true;
            }
        }

        public Recipe IfRecipeSelected
        {
            get { return _recipe; }
            set
            {
                this.RaiseAndSetIfChanged(ref _recipe, value);
                IfRecipeSelectedCommandImpl(_mainWindow);
            }
        }

        private async void IfRecipeSelectedCommandImpl(Window window)
        {
            if (User.UserId > 0)
            {
                RecipeWindow recipeWindow = new RecipeWindow(_recipe);

                _mainWindow.listbox_Recipes.SelectedIndex = -1;
                await recipeWindow.ShowDialog(window);
            }
        }

        public int SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                if (value == 1)
                {
                    Recipes = new ObservableCollection<Recipe>(Helper.GetContext().Recipes.OrderBy(x => x.CreationDate).ToList());
                }
                else if (value == 0)
                {
                    Recipes = new ObservableCollection<Recipe>(Helper.GetContext().Recipes.OrderByDescending(x => x.CreationDate).ToList());
                }

                this.RaiseAndSetIfChanged(ref _selectedFilter, value);
            }
        }

        public string SearchQuery
        {
            get { return _searchQuery; }
            set { this.RaiseAndSetIfChanged(ref _searchQuery, value); }
        }

        public MainWindowViewModel(MainWindow window)
        {
            _mainWindow = window;
            _selectedFilter = 0;

            Recipes = new ObservableCollection<Recipe>(Helper.GetContext().Recipes.OrderByDescending(x => x.CreationDate).ToList());

            SignInCommand = ReactiveCommand.Create<MainWindow>(SignInCommandImpl);
            LogOutCommand = ReactiveCommand.Create<MainWindow>(LogOutCommandImpl);
            AddRecipeCommand = ReactiveCommand.Create<Window>(AddRecipeCommandImpl);
            FavoriteRecipesCommand = ReactiveCommand.Create<Window>(FavoriteRecipesCommandImpl);
            SearchCommand = ReactiveCommand.Create<Window>(SearchCommandImpl);
        }
    }
}
