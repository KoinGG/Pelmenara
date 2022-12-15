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
using System.Threading.Tasks;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class AddRecipeVM : ViewModelBase
    {
        public ReactiveCommand<Window, Unit> AddRecipeAcceptCommand { get; }
        public ReactiveCommand<Window, Unit> CancelCommand { get; }

        private Recipe _recipe = new Recipe();

        private void AddRecipeAcceptCommandImpl(Window window)
        {
            if(Recipe.Title != null && Recipe.Description != null
               && Recipe.Ingredients != null && Recipe.CookingTime != null)
            {
                if(Recipe.Title.Length > 30)
                {
                    if(Recipe.Ingredients.Length > 200)
                    {
                        if(Recipe.CookingTime.Length > 15)
                        {
                            if(Helper.GetContext().Recipes.FirstOrDefault(x => x.Title == Recipe.Title) != null)
                            {
                                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Рецепт с таким названием уже существует", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                                return;
                            }
                            try
                            {
                                Recipe.CreationDate = DateTime.Now;
                                Recipe.OwnerId = MainWindowViewModel.User.UserId;
                                Helper.GetContext().Recipes.Add(Recipe);
                                Helper.GetContext().SaveChanges();
                            }
                            catch
                            {
                                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Не удалось добавить рецепт", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                                return;
                            }

                            window.Close();
                        }
                        else
                        {
                            MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Превышено допустимое количество символов для поля 'Время Готовки'", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                        }
                    }
                    else
                    {
                        MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Число символов в поле 'Игридиенты' превышает допустимое значение (200)  ", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                    }
                }
                else
                {
                    MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Длина заголовка не должна превышать 30 символов", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
                }
            }
            else
            {
                MessageBoxManager.GetMessageBoxStandardWindow("ОшибОчка", "Поля не заполнены", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
            }
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
        public AddRecipeVM()
        {
            AddRecipeAcceptCommand = ReactiveCommand.Create<Window>(AddRecipeAcceptCommandImpl);
            CancelCommand = ReactiveCommand.Create<Window>(CancelCommandImpl);
        }
    }
}
