using System.Windows;
using System.Windows.Controls;

namespace Desktop.Helpers
{
    public class Print
    {
        public static void PrintDocument(object toPrint)
        {
            var data = toPrint as DataGrid;
            if (data.Items.Count != 0)
            {
                DialogDisplayHelper.DisplayMessageBox("no printer found", "information",boxIcon: MessageBoxImage.Error);
                return;
            }
            DialogDisplayHelper.DisplayMessageBox("Nothing have selected to print","Information",boxIcon:MessageBoxImage.Warning);

        }
    }
}
