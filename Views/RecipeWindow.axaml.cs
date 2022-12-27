using Avalonia.Controls;
using Pelmenara_AUI_RUI.Sourses;
using Pelmenara_AUI_RUI.ViewModels;

namespace Pelmenara_AUI_RUI.Views
{
    public partial class RecipeWindow : Window
    {
        public RecipeWindow(Recipe recipe)
        {
            DataContext = new RecipeVM(recipe);
            (DataContext as RecipeVM).SomeMethod(this);            
            InitializeComponent();
        }
    }
}
