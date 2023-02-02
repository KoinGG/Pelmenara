
using Avalonia.Controls;
using Pelmenara_AUI_RUI.Sourses;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class AddRecipeVM : ViewModelBase
    {
        #region [Private Fields]

        private Recipe _newRecipe = new Recipe();

        #endregion

        public AddRecipeVM()
        {
            AddRecipeAcceptCommand = ReactiveCommand.Create<Window>(AddRecipeAcceptCommandImpl);
            CancelCommand = ReactiveCommand.Create<Window>(CancelCommandImpl);
        }

        #region [Properties]

        public Recipe NewRecipe
        {
            get
            {
                return _newRecipe;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _newRecipe, value);
            }
        }

        #region [Commands Declaration]

        public ReactiveCommand<Window, Unit> AddRecipeAcceptCommand { get; }
        public ReactiveCommand<Window, Unit> CancelCommand { get; }

        #endregion

        #endregion

        #region [Methods]

        private void AddRecipeAcceptCommandImpl(Window window)
        {
            AddRecipeValidation(window);
            
            try
            {
                NewRecipe.CreationDate = DateTime.UtcNow;
                NewRecipe.OwnerId = MainWindowVM.CurrentUser.UserId;

                DbContextProvider.GetContext().Recipes.Add(NewRecipe);
                DbContextProvider.GetContext().SaveChanges();
            }
            catch
            {
                ErrorMessage.ShowErrorMessage(window, "Не удалось добавить рецепт");
                return;
            }

            window.Close();
            
        }

        private void CancelCommandImpl(Window window)
        {
            window.Close();
        }

        private void AddRecipeValidation(Window window)
        {
            if (NewRecipe.Title == null || NewRecipe.Description == null || NewRecipe.Ingredients == null || NewRecipe.CookingTime == null
             || NewRecipe.Title == ""   || NewRecipe.Description == ""   || NewRecipe.Ingredients == ""   || NewRecipe.CookingTime == ""
             || NewRecipe.Title == " "  || NewRecipe.Description == " "  || NewRecipe.Ingredients == " "  || NewRecipe.CookingTime == " ")
            {
                ErrorMessage.ShowErrorMessage(window, "Поля не заполнены");                
                return;
            }
            if (NewRecipe.Title.Length > 30)
            {
                ErrorMessage.ShowErrorMessage(window, "Длина заголовка не должна превышать 30 символов");
                return;
            }
            if (NewRecipe.Ingredients.Length > 200)
            {
                ErrorMessage.ShowErrorMessage(window, "Число символов в поле 'Ингрeдиенты' превышает допустимое значение (200)");
                return;
            }
            if (NewRecipe.CookingTime.Length > 15)
            {
                ErrorMessage.ShowErrorMessage(window, "Превышено допустимое количество символов для поля 'Время Готовки'");
                return;
            }

            var ifRecipeWithSameTitleExist = DbContextProvider.GetContext().Recipes.FirstOrDefault(x => x.Title == NewRecipe.Title); // Если null значит не существует

            if (ifRecipeWithSameTitleExist != null)
            {
                ErrorMessage.ShowErrorMessage(window, "Рецепт с таким названием уже существует");
                return;
            }
        }

        #endregion
    }
}
