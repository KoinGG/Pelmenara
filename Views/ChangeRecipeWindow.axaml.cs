using Avalonia.Controls;
using Pelmenara_AUI_RUI.ViewModels;

namespace Pelmenara_AUI_RUI.Views
{
    public partial class ChangeRecipeWindow : Window
    {
        public ChangeRecipeWindow()
        {
            InitializeComponent();
        }

        public ChangeRecipeWindow(Recipe recipe) : this()
        {
            DataContext = new ChangeRecipeVM(recipe);
        }
    }
}
