using Avalonia.Controls;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;
using Pelmenara_AUI_RUI.Sourses;
using Pelmenara_AUI_RUI.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class ChangeRecipeVM : ViewModelBase
    {
        public ReactiveCommand<Window, Unit> ChangeRecipeAcceptCommand { get; }
        public ReactiveCommand<Window, Unit> CancelCommand { get; }

        private Recipe _recipe = new Recipe();

        private void ChangeRecipeAcceptCommandImpl(Window window)
        {
            if (Recipe.Title == null || Recipe.Description == null || Recipe.Ingredients == null || Recipe.CookingTime == null
             || Recipe.Title == ""   || Recipe.Description == ""   || Recipe.Ingredients == ""   || Recipe.CookingTime == ""
             || Recipe.Title == " "  || Recipe.Description == " "  || Recipe.Ingredients == " "  || Recipe.CookingTime == " ")
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Поля не заполнены", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }
            if (Recipe.Title.Length > 30)
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Длина заголовка не должна превышать 30 символов", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }
            if (Recipe.Ingredients.Length > 200)
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Число символов в поле 'Игридиенты' превышает допустимое значение (200)  ", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }
            if (Recipe.CookingTime.Length > 15)
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Превышено допустимое количество символов для поля 'Время Готовки'", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }

            try
            {
                Recipe.CreationDate = DateTime.UtcNow;
                Recipe.OwnerId = MainWindowViewModel.User.UserId;
                Helper.GetContext().Recipes.Update(Recipe);
                Helper.GetContext().SaveChanges();
            }
            catch
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Не удалось обновить рецепт", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                return;
            }

            window.Close();
        }

        private void CancelCommandImpl(Window window)
        {
            window.Close();
        }

        public Recipe Recipe
        {
            get { return _recipe; }
            set { this.RaiseAndSetIfChanged(ref _recipe, value); }
        }

        public ChangeRecipeVM(Recipe recipe)
        {
            Recipe = recipe;

            ChangeRecipeAcceptCommand = ReactiveCommand.Create<Window>(ChangeRecipeAcceptCommandImpl);
            CancelCommand = ReactiveCommand.Create<Window>(CancelCommandImpl);
        }
    }
}
