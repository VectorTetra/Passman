using Kurs_Passman.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kurs_Passman
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        MainWindow mainWindow; //Посилання на головне вікно програми
        public SettingsWindow(MainViewModel vm, MainWindow mainWindow)
        {
            InitializeComponent();
            this.DataContext = vm;
            this.mainWindow = mainWindow;
            var location = mainWindow.PointToScreen(new Point(0, 0));
            this.Left = location.X;
            this.Top = location.Y;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
