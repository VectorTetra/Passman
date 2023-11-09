using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kurs_Passman.Interfaces
{
    public interface IDialogService
    {
        bool ConfirmDialog(string message);
        void MessageDialog(string message);
    }
    class DialogService : IDialogService
    {
        public bool ConfirmDialog(string message)
        {
            MessageBoxResult res = MessageBox.Show(message, "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return res == MessageBoxResult.Yes ? true : false;
        }
        public void MessageDialog(string message) 
        {
            MessageBox.Show(message);
        }
    }
   
}
