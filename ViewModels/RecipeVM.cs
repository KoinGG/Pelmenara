
using Avalonia.Controls;
using Pelmenara_AUI_RUI.Sourses;
using Pelmenara_AUI_RUI.Views;
using ReactiveUI;
using System.Linq;
using System.Reactive;
using System.Threading;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class RecipeVM : ViewModelBase
    {
        #region [Private Fields]

        private FavoriteRecipe _favoriteRecipe = new FavoriteRecipe();
        private Recipe _recipe;

        private string _title;

        private bool _isChangeAndDeleteButtonVisibleAndEnabled;
        private bool _isAddFavoriteRecipeButtonVisible;
        private bool _isAddFavoriteRecipeButtonEnabled;        

        #endregion

        public RecipeVM(Recipe recipe)
        {
            _recipe = recipe;
            _title = $"{Recipe.Title}";

            _isChangeAndDeleteButtonVisibleAndEnabled = false;
            _isAddFavoriteRecipeButtonVisible = false;
            _isAddFavoriteRecipeButtonEnabled = false;

            ChangeRecipeCommand = ReactiveCommand.Create<Window>(ChangeRecipeCommandImpl);
            DeleteRecipeCommand = ReactiveCommand.Create<Window>(DeleteRecipeCommandImpl);
            AddFavoriteRecipeCommand = ReactiveCommand.Create<RecipeWindow>(AddFavoriteRecipeCommandImpl);
        }

        #region [Properties]

        public Recipe Recipe
        {
            get
            {
                return _recipe;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _recipe, value);
            }
        }
        public FavoriteRecipe FavoriteRecipe
        {
            get
            {
                return _favoriteRecipe;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _favoriteRecipe, value);
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

        public bool IsChangeAndDeleteButtonVisibleAndEnabled
        {
            get
            {
                return _isChangeAndDeleteButtonVisibleAndEnabled;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _isChangeAndDeleteButtonVisibleAndEnabled, value);
            }
        }        

        public bool IsAddFavoriteRecipeButtonVisible
        {
            get
            {
                return _isAddFavoriteRecipeButtonVisible;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _isAddFavoriteRecipeButtonVisible, value);
            }
        }
        public bool IsAddFavoriteRecipeButtonEnabled
        {
            get
            {
                return _isAddFavoriteRecipeButtonEnabled;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _isAddFavoriteRecipeButtonEnabled, value);
            }
        }

        #region [Commands Declaration]

        public ReactiveCommand<Window, Unit> ChangeRecipeCommand { get; }
        public ReactiveCommand<Window, Unit> DeleteRecipeCommand { get; }
        public ReactiveCommand<RecipeWindow, Unit> AddFavoriteRecipeCommand { get; }

        #endregion

        #endregion

        #region [Methods]

        private async void ChangeRecipeCommandImpl(Window window)
        {
            var changeRecipeWindow = new ChangeRecipeWindow(_recipe);

            try
            {
                await changeRecipeWindow.ShowDialog(window).WaitAsync(CancellationToken.None);
            }
            catch
            {
                ErrorMessage.ShowErrorMessage(window, "Непредвиденная ошибка");
            }
        }

        private void DeleteRecipeCommandImpl(Window window)
        {
            try
            {
                DbContextProvider.GetContext().Recipes.Remove(Recipe);
                DbContextProvider.GetContext().SaveChanges();
            }            
            catch
            {
                ErrorMessage.ShowErrorMessage(window, "Не удалось удалить рецепт");
                return;
            }

            window.Close();
        }

        private void AddFavoriteRecipeCommandImpl(RecipeWindow window)
        {
            AddFavoriteRecipeValidation(window);

            FavoriteRecipe.UserId = MainWindowVM.CurrentUser.UserId;
            FavoriteRecipe.RecipeId = Recipe.RecipeId;

            try
            {
                DbContextProvider.GetContext().FavoriteRecipes.Add(FavoriteRecipe);
                DbContextProvider.GetContext().SaveChanges();
            }
            catch
            {
                ErrorMessage.ShowErrorMessage(window, "Не удалось добавить рецепт в избранное");
                return;
            }
        }

        public void IfUserIsRecipeOwner()
        {
            if (Recipe.OwnerId == MainWindowVM.CurrentUser.UserId)
            {
                IsChangeAndDeleteButtonVisibleAndEnabled = true;
            }

            var ifFavoriteRecipeIsExist = DbContextProvider.GetContext().FavoriteRecipes.FirstOrDefault(x => x.UserId == MainWindowVM.CurrentUser.UserId && x.RecipeId == Recipe.RecipeId);
            // Если null значит не существует

            if (ifFavoriteRecipeIsExist == null)
            {
                IsAddFavoriteRecipeButtonEnabled = true;
            }

            IsAddFavoriteRecipeButtonVisible = true;
        }

        private void AddFavoriteRecipeValidation(Window window)
        {
            var ifFavoriteRecipeIsExist = DbContextProvider.GetContext().FavoriteRecipes.FirstOrDefault(x => x.UserId == MainWindowVM.CurrentUser.UserId && x.RecipeId == Recipe.RecipeId);
            // Если null значит не существует

            if (ifFavoriteRecipeIsExist != null)
            {
                ErrorMessage.ShowErrorMessage(window, "Не удалось добавить рецепт в избранное");
                return;
            }
        }

        #endregion
    }
}
