using Avalonia.Controls;
using Pelmenara_AUI_RUI.Sourses;
using Pelmenara_AUI_RUI.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class FavoriteRecipeVM : ViewModelBase
    {
        private FavoriteRecipeWindow _favoriteRecipeWindow;
        private FavoriteRecipe _favoriteRecipe;        
        private ObservableCollection<FavoriteRecipe> _favoriteRecipes;

        public ObservableCollection<FavoriteRecipe> FavoriteRecipes
        {
            get { return _favoriteRecipes; }
            set
            {
                this.RaiseAndSetIfChanged(ref _favoriteRecipes, value);
            }
        }
        public FavoriteRecipe IfRecipeSelected
        {
            get { return _favoriteRecipe; }
            set
            {
                this.RaiseAndSetIfChanged(ref _favoriteRecipe, value);
                IfRecipeSelectedCommandImpl(_favoriteRecipeWindow);
            }
        }
        private async void IfRecipeSelectedCommandImpl(Window window)
        {
            RecipeWindow recipeWindow = new RecipeWindow(_favoriteRecipe.Recipe);
            _favoriteRecipeWindow.listbox_FavoriteRecipes.SelectedIndex = -1;
            await recipeWindow.ShowDialog(window);
        }

        public FavoriteRecipeVM(FavoriteRecipeWindow window)
        {
            _favoriteRecipeWindow = window;

            FavoriteRecipes = new ObservableCollection<FavoriteRecipe>(Helper.GetContext().FavoriteRecipes.Where(x => x.UserId == MainWindowViewModel.User.UserId).ToList());
        }
    }
}
