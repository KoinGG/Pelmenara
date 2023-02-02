
using Avalonia.Controls;
using Pelmenara_AUI_RUI.Sourses;
using Pelmenara_AUI_RUI.Views;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class FavoriteRecipeVM : ViewModelBase
    {
        #region [Private Fields]

        private const int NotSelectedItem = -1;

        private FavoriteRecipeWindow _favoriteRecipeWindow;
        private FavoriteRecipe _ifRecipeSelected;
        private ObservableCollection<FavoriteRecipe> _favoriteRecipes;
        private int _favoriteRecipesListBoxSelectedIndex;

        #endregion

        public FavoriteRecipeVM(FavoriteRecipeWindow window)
        {
            _favoriteRecipeWindow = window;

            var listOfFavoriteRecipes = DbContextProvider.GetContext().FavoriteRecipes.Where(x => x.UserId == MainWindowVM.CurrentUser.UserId).ToList();
            FavoriteRecipes = new ObservableCollection<FavoriteRecipe>(listOfFavoriteRecipes);
        }

        #region [Properties]

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
        public FavoriteRecipe IfRecipeSelected
        {
            get
            {
                return _ifRecipeSelected;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _ifRecipeSelected, value);
                IfRecipeSelectedCommandImpl(_favoriteRecipeWindow);
            }
        }
        public int FavoriteRecipesListBoxSelectedIndex
        {
            get
            {
                return _favoriteRecipesListBoxSelectedIndex;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _favoriteRecipesListBoxSelectedIndex, value);
            }
        }

        #endregion

        #region [Methods]

        private async void IfRecipeSelectedCommandImpl(Window window)
        {
            var recipeWindow = new RecipeWindow(_ifRecipeSelected.Recipe);
            //FavoriteRecipesListBoxSelectedIndex = NotSelectedItem;
            _favoriteRecipeWindow.listbox_FavoriteRecipes.SelectedIndex = NotSelectedItem;

            try
            {
                await recipeWindow.ShowDialog(window);
            }
            catch
            {
                ErrorMessage.ShowErrorMessage(window, "Непредвиденная ошибка");
            }            
        }

        #endregion
    }
}
