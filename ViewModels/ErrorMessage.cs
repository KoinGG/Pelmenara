
using Avalonia.Controls;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

namespace Pelmenara_AUI_RUI.ViewModels
{
    public class ErrorMessage
    {
        public static void ShowErrorMessage(Window window, string errorText, string errorTitle = "Ошибка")
        {
            MessageBoxManager.GetMessageBoxStandardWindow($"{errorTitle}", $"{errorText}", ButtonEnum.Ok, Icon.Warning).ShowDialog(window);
        }
    }
}
