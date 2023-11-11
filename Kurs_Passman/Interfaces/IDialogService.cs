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
        bool ConfirmDialog(string message,string caption);
        void MessageDialog(string message,string caption);
    }
    class DialogService : IDialogService
    {
        public bool ConfirmDialog(string message, string caption)
        {
            MessageBoxResult res = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return res == MessageBoxResult.Yes ? true : false;
        }
        public void MessageDialog(string message, string caption) 
        {
            MessageBox.Show(message, caption);
        }
    }
   
}
