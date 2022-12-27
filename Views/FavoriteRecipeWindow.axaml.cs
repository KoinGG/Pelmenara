using Avalonia.Controls;
using Pelmenara_AUI_RUI.ViewModels;

namespace Pelmenara_AUI_RUI.Views
{
    public partial class FavoriteRecipeWindow : Window
    {
        public FavoriteRecipeWindow()
        {
            DataContext = new FavoriteRecipeVM(this);
            InitializeComponent();
        }
    }
}
