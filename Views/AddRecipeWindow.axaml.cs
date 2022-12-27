using Avalonia.Controls;
using Pelmenara_AUI_RUI.ViewModels;

namespace Pelmenara_AUI_RUI.Views
{
    public partial class AddRecipeWindow : Window
    {
        public AddRecipeWindow()
        {
            DataContext = new AddRecipeVM();
            InitializeComponent();
        }
    }
}
