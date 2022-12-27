using Avalonia.Controls;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using Pelmenara_AUI_RUI.Sourses;
using Pelmenara_AUI_RUI.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class RecipeVM : ViewModelBase
    {
        private Recipe _recipe;
        private FavoriteRecipe _favoriteRecipe = new FavoriteRecipe();

        public ReactiveCommand<Window, Unit> ChangeRecipeCommand { get; }
        public ReactiveCommand<Window, Unit> DeleteRecipeCommand { get; }
        public ReactiveCommand<RecipeWindow, Unit> AddFavoriteRecipeCommand { get; }

        public Recipe Recipe
        {
            get { return _recipe; }
            set { this.RaiseAndSetIfChanged(ref _recipe, value); }
        }
        public FavoriteRecipe FavoriteRecipe
        {
            get { return _favoriteRecipe; }
            set { this.RaiseAndSetIfChanged(ref _favoriteRecipe, value); }
        }

        private async void ChangeRecipeCommandImpl(Window window)
        {
            ChangeRecipeWindow changeRecipeWindow = new ChangeRecipeWindow(_recipe);
            await changeRecipeWindow.ShowDialog(window).WaitAsync(CancellationToken.None);
        }

        private void DeleteRecipeCommandImpl(Window window)
        {
            try
            {
                Helper.GetContext().Recipes.Remove(Recipe);
                Helper.GetContext().SaveChanges();
            }            
            catch
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Не удалось удалить рецепт", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }


            window.Close();
        }

        private void AddFavoriteRecipeCommandImpl(RecipeWindow window)
        {
            FavoriteRecipe.UserId = MainWindowViewModel.User.UserId;
            FavoriteRecipe.RecipeId = Recipe.RecipeId;
            try
            {
                Helper.GetContext().FavoriteRecipes.Add(FavoriteRecipe);
                Helper.GetContext().SaveChanges();
            }
            catch
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Не удалось добавить рецепт в избранное", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }
        }

        public bool AddFavoriteRecipeCommandImpl(int UserId, int RecipeId)
        {
            if(Helper.GetContext().FavoriteRecipes.FirstOrDefault(x => x.UserId == UserId && x.RecipeId == RecipeId) == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void SomeMethod(RecipeWindow window)
        {
            if (Recipe.OwnerId == MainWindowViewModel.User.UserId)
            {
                window.btn_ChangeRecipe.IsVisible = true;
                window.btn_ChangeRecipe.IsEnabled = true;

                window.btn_DeleteRecipe.IsVisible = true;
                window.btn_DeleteRecipe.IsEnabled = true;
            }

            if (Helper.GetContext().FavoriteRecipes.FirstOrDefault(x => x.UserId == MainWindowViewModel.User.UserId && x.RecipeId == Recipe.RecipeId) == null)
            {
                window.btn_AddFavoriteRecipe.IsEnabled = true;
            }            

            window.btn_AddFavoriteRecipe.IsVisible = true;
        }

        public RecipeVM(Recipe recipe)
        {
            _recipe = recipe;

            ChangeRecipeCommand = ReactiveCommand.Create<Window>(ChangeRecipeCommandImpl);
            DeleteRecipeCommand = ReactiveCommand.Create<Window>(DeleteRecipeCommandImpl);
            AddFavoriteRecipeCommand = ReactiveCommand.Create<RecipeWindow>(AddFavoriteRecipeCommandImpl);
        }
    }
}
