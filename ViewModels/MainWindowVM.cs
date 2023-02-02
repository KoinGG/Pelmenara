
using Avalonia.Controls;
using Pelmenara_AUI_RUI.Sourses;
using Pelmenara_AUI_RUI.Views;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        #region [Private Fields]

        #region [Constants]

        private const int FilterFromNewToOld = 0;
        private const int FilterFromOldToNew = 1;
        private const int NotSelectedItem = -1;

        #endregion

        private MainWindow _mainWindow;
        private Recipe _ifRecipeSelected;
        private ObservableCollection<Recipe> _recipes;
        private ObservableCollection<FavoriteRecipe> _favoriteRecipes;
        private int _selectedFilter;
        private string _searchQuery;

        private bool _isSignInButtonEnabled;
        private bool _isSignInButtonVisible;

        private bool _isLogOutButtonEnabled;
        private bool _isLogOutButtonVisible;

        private bool _isAddRecipeButtonEnabled;
        private bool _isFavoriteRecipesButtonEnabled;

        private string _title;

        private int _recipesListBoxSelectedIndex;

        #endregion

        public MainWindowVM(MainWindow window)
        {
            _mainWindow = window;
            _selectedFilter = FilterFromOldToNew;

            _isSignInButtonEnabled = true;
            _isSignInButtonVisible = true;

            _isLogOutButtonEnabled = false;
            _isLogOutButtonVisible = false;

            _isAddRecipeButtonEnabled = false;
            _isFavoriteRecipesButtonEnabled = false;

            _title = "Pelmenara";

            var listOfRecipes = DbContextProvider.GetContext().Recipes.OrderByDescending(x => x.CreationDate).ToList();
            Recipes = new ObservableCollection<Recipe>(listOfRecipes);

            SignInCommand = ReactiveCommand.Create<Window>(SignInCommandImpl);
            LogOutCommand = ReactiveCommand.Create<MainWindow>(LogOutCommandImpl);
            AddRecipeCommand = ReactiveCommand.Create<Window>(AddRecipeCommandImpl);
            FavoriteRecipesCommand = ReactiveCommand.Create<Window>(FavoriteRecipesCommandImpl);
            SearchCommand = ReactiveCommand.Create<Window>(SearchCommandImpl);
        }

        #region [Properties]

        public static User? CurrentUser { get; set; } = null;        
        public ObservableCollection<Recipe> Recipes
        {
            get
            {
                return _recipes;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _recipes, value);
            }
        }
        public ObservableCollection<FavoriteRecipe> FavoriteRecipes
        {
            get 
            { 
                return _favoriteRecipes; 
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _favoriteRecipes, value);
            }
        }
        public Recipe IfRecipeSelected
        {
            get { return _ifRecipeSelected; }
            set
            {
                this.RaiseAndSetIfChanged(ref _ifRecipeSelected, value);
                IfRecipeSelectedCommand(_mainWindow);
            }
        }
        public int SelectedFilter
        {
            get 
            { 
                return _selectedFilter; 
            }
            set
            {
                if (value == FilterFromOldToNew)
                {
                    Recipes = new ObservableCollection<Recipe>(DbContextProvider.GetContext().Recipes.OrderBy(x => x.CreationDate).ToList());
                }
                else if (value == FilterFromNewToOld)
                {
                    Recipes = new ObservableCollection<Recipe>(DbContextProvider.GetContext().Recipes.OrderByDescending(x => x.CreationDate).ToList());
                }

                this.RaiseAndSetIfChanged(ref _selectedFilter, value);
            }
        }
        public string SearchQuery
        {
            get 
            { 
                return _searchQuery; 
            }
            set 
            { 
                this.RaiseAndSetIfChanged(ref _searchQuery, value);
            }
        }

        public bool IsSignInButtonEnabled 
        { 
            get
            {
                return _isSignInButtonEnabled;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _isSignInButtonEnabled, value);
            }
        }
        public bool IsSignInButtonVisible 
        { 
            get
            {
                return _isSignInButtonVisible;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _isSignInButtonVisible, value);
            }
        }

        public bool IsLogOutButtonEnabled 
        {
            get
            {
                return _isLogOutButtonEnabled;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _isLogOutButtonEnabled, value);
            } 
        }
        public bool IsLogOutButtonVisible 
        { 
            get
            {
                return _isLogOutButtonVisible;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _isLogOutButtonVisible, value);
            }
        }

        public bool IsAddRecipeButtonEnabled 
        {
            get
            {
                return _isAddRecipeButtonEnabled;
            } 
            set
            {
                this.RaiseAndSetIfChanged(ref _isAddRecipeButtonEnabled, value);
            }
        }

        public bool IsFavoriteRecipesButtonEnabled 
        { 
            get
            {
                return _isFavoriteRecipesButtonEnabled;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _isFavoriteRecipesButtonEnabled, value);
            } 
        }

        public string Title 
        { 
            get 
            { 
                return _title; 
            } 
            set 
            { 
                this.RaiseAndSetIfChanged(ref _title, value);
            } 
        }

        public int RecipesListBoxSelectedIndex
        {
            get
            {
                return _recipesListBoxSelectedIndex;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _recipesListBoxSelectedIndex, value);
            }
        }

        #region [Commands Declaration]

        public ReactiveCommand<Window, Unit> SignInCommand { get; }
        public ReactiveCommand<MainWindow, Unit> LogOutCommand { get; }
        public ReactiveCommand<Window, Unit> AddRecipeCommand { get; }
        public ReactiveCommand<Window, Unit> FavoriteRecipesCommand { get; }
        public ReactiveCommand<Window, Unit> SearchCommand { get; }

        #endregion

        #endregion

        #region [Methods]

        private async void SignInCommandImpl(Window window)
        {
            var authWindow = new AuthWindow();
            try
            {
                await authWindow.ShowDialog(window).WaitAsync(TimeSpan.FromMinutes(60));
            }
            catch
            {
                ErrorMessage.ShowErrorMessage(window, "Время авторизации истекло");
                authWindow.Close();
            }
            finally
            {
                IfUserAuthorisedChanges(window);
            }
        }

        private void LogOutCommandImpl(MainWindow window)
        {
            if (CurrentUser != null)
            {
                CurrentUser = null;

                IfUserLogOutChanges(window);
            }
        }

        private async void AddRecipeCommandImpl(Window window)
        {
            var addRecipeWindow = new AddRecipeWindow();

            try
            {
                await addRecipeWindow.ShowDialog(window).WaitAsync(CancellationToken.None);
                Recipes = new ObservableCollection<Recipe>(DbContextProvider.GetContext().Recipes.OrderByDescending(x => x.CreationDate).ToList());
            }
            catch
            {
                ErrorMessage.ShowErrorMessage(window, "Непредвиденная ошибка");
            }
        }

        private async void FavoriteRecipesCommandImpl(Window window)
        {
            var favoriteRecipeWindow = new FavoriteRecipeWindow();
            try
            {
                await favoriteRecipeWindow.ShowDialog(window).WaitAsync(CancellationToken.None);
                Recipes = new ObservableCollection<Recipe>(DbContextProvider.GetContext().Recipes.OrderByDescending(x => x.CreationDate).ToList());
            }
            catch
            {
                ErrorMessage.ShowErrorMessage(window, "Непредвиденная ошибка");
            }            
        }

        private void SearchCommandImpl(Window window)
        {
            var search = DbContextProvider.GetContext().Recipes.Where(x => x.Title.Contains(_searchQuery) || x.Description.Contains(_searchQuery) || x.Ingredients.Contains(_searchQuery)).ToList();

            if (SelectedFilter == FilterFromNewToOld)
            {
                Recipes = new ObservableCollection<Recipe>(search.OrderByDescending(x => x.CreationDate));
            }
            else if (SelectedFilter == FilterFromOldToNew)
            {
                Recipes = new ObservableCollection<Recipe>(search.OrderBy(x => x.CreationDate));
            }
        }

        private void IfUserAuthorisedChanges(Window window)
        {
            if (CurrentUser != null)
            {
                Title = $"Pelmenara: {CurrentUser.Login}";

                IsSignInButtonEnabled = false;
                IsSignInButtonVisible = false;

                IsLogOutButtonEnabled = true;
                IsLogOutButtonVisible = true;

                IsAddRecipeButtonEnabled = true;

                IsFavoriteRecipesButtonEnabled = true;
            }
        }

        public void IfUserLogOutChanges(Window window)
        {
            Title = "Pelmenara";

            IsSignInButtonEnabled = true;
            IsSignInButtonVisible = true;

            IsLogOutButtonEnabled = false;
            IsLogOutButtonVisible = false;

            IsAddRecipeButtonEnabled = false;

            IsFavoriteRecipesButtonEnabled = false;
        }

        private async void IfRecipeSelectedCommand(Window window)
        {
            if (CurrentUser != null)
            {
                var recipeWindow = new RecipeWindow(_ifRecipeSelected);
                //RecipesListBoxSelectedIndex = NotSelectedItem;
                _mainWindow.listbox_Recipes.SelectedIndex = NotSelectedItem;
                try
                {
                    await recipeWindow.ShowDialog(window).WaitAsync(CancellationToken.None);
                }
                catch
                {
                    ErrorMessage.ShowErrorMessage(window, "Непредвиденная ошибка");
                }
            }
        }

        #endregion
    }
}
