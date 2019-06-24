using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace Desktop
{ 
    public static class DialogDisplayHelper
    {
        public static void DisplayMessageBox(string message, string header, MessageBoxButton boxButton = MessageBoxButton.OK,MessageBoxImage boxIcon=MessageBoxImage.Information)
        {
            MessageBox.Show(message, header, boxButton,boxIcon);
        }
    }
}
