using Kurs_Passman.Interfaces;
using Kurs_Passman.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Kurs_Passman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel? mvmod = null;
        public MainWindow()
        {
            InitializeComponent();
            //mvmod = this.Resources["mvmod"] as MainViewModel;
            Uri iconUri = new Uri("../../../Resources/Passman_Icon.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            MainViewModel mainViewModel = new MainViewModel(new DialogService());
            this.Resources.Add("mvmod", mainViewModel);
            this.mvmod = mainViewModel;
            this.DataContext = mvmod;

            // Заборонити вставку у поле перегляду пароля на сторінці "Інформація"
            InfoSitePassword.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (sender, e) => { }));
            // Заборонити вставку у поле перегляду пароля на сторінці "Шифрування"
            CryptSitePassword.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (sender, e) => { }));
            // Заборонити вставку у поле секретного ключа шифрування на сторінці "Шифрування"
            CryptSitePasswordSecretKey.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (sender, e) => { }));
        }
        // Виникає під час зміни вкладки
        private async void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabStatistics.IsSelected)
            {
                await mvmod?.UpdateStatistics();
            }
            if (TabUpdDelAcc.IsSelected) mvmod?.LoadSelectedAccountForUpdate();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mvmod?.SelectedAccount != null)
            {
                mvmod?.LoadSelectedAccountForUpdate();
                //MainTabControl.Visibility = Visibility.Visible;
                TabItemAccountInfo.IsEnabled = true;
                TabUpdDelAcc.IsEnabled = true;
                TabEncrypt.IsEnabled = true;
                TabStatistics.IsEnabled = true;
            }
            else
            {
                TabItemAccountInfo.IsEnabled = false;
                TabUpdDelAcc.IsEnabled = false;
                TabEncrypt.IsEnabled = false;
                TabStatistics.IsEnabled = false;
                MainTabControl.SelectedIndex = 1;
            }
        }

        // Заборонити введення пароля на сторінці "Інформація"
        private void InfoSitePassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }
    }
}
