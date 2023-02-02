
using Avalonia.Controls;
using Pelmenara_AUI_RUI.Sourses;
using ReactiveUI;
using System;
using System.Reactive;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class ChangeRecipeVM : ViewModelBase
    {
        #region [Private Fields]

        private Recipe _editedRecipe = new Recipe();

        #endregion

        public ChangeRecipeVM(Recipe recipe)
        {
            EditedRecipe = recipe;

            ChangeRecipeAcceptCommand = ReactiveCommand.Create<Window>(ChangeRecipeAcceptCommandImpl);
            CancelCommand = ReactiveCommand.Create<Window>(CancelCommandImpl);
        }

        #region [Properties]

        public Recipe EditedRecipe
        {
            get
            {
                return _editedRecipe;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _editedRecipe, value);
            }
        }

        #region [Commands Declaration]

        public ReactiveCommand<Window, Unit> ChangeRecipeAcceptCommand { get; }
        public ReactiveCommand<Window, Unit> CancelCommand { get; }

        #endregion

        #endregion

        #region [Methods]

        private void ChangeRecipeAcceptCommandImpl(Window window)
        {
            ChangeRecipeValidation(window);

            try
            {
                EditedRecipe.CreationDate = DateTime.UtcNow;
                EditedRecipe.OwnerId = MainWindowVM.CurrentUser.UserId;

                DbContextProvider.GetContext().Recipes.Update(EditedRecipe);
                DbContextProvider.GetContext().SaveChanges();
            }
            catch
            {
                ErrorMessage.ShowErrorMessage(window, "Не удалось обновить рецепт");
                return;
            }

            window.Close();
        }

        private void CancelCommandImpl(Window window)
        {
            window.Close();
        }

        private void ChangeRecipeValidation(Window window)
        {
            if (EditedRecipe.Title == null || EditedRecipe.Description == null || EditedRecipe.Ingredients == null || EditedRecipe.CookingTime == null
             || EditedRecipe.Title == ""   || EditedRecipe.Description == ""   || EditedRecipe.Ingredients == ""   || EditedRecipe.CookingTime == ""
             || EditedRecipe.Title == " "  || EditedRecipe.Description == " "  || EditedRecipe.Ingredients == " "  || EditedRecipe.CookingTime == " ")
            {
                ErrorMessage.ShowErrorMessage(window, "Поля не заполнены");
                return;
            }
            if (EditedRecipe.Title.Length > 30)
            {
                ErrorMessage.ShowErrorMessage(window, "Длина заголовка не должна превышать 30 символов");
                return;
            }
            if (EditedRecipe.Ingredients.Length > 200)
            {
                ErrorMessage.ShowErrorMessage(window, "Число символов в поле 'Ингредиенты' превышает допустимое значение (200) ");
                return;
            }
            if (EditedRecipe.CookingTime.Length > 15)
            {
                ErrorMessage.ShowErrorMessage(window, "Превышено допустимое количество символов для поля 'Время Готовки'");
                return;
            }
        }

        #endregion
    }
}
