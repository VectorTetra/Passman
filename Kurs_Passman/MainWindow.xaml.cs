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

            MainViewModel mvmod = this.Resources["mvmod"] as MainViewModel;
            this.DataContext = mvmod;
            this.mvmod = mvmod;
            this.CommandBindings.Add(new CommandBinding(mvmod.AddAccountCommand, mvmod.Add_Account, mvmod.CanAdd_Account));
            this.CommandBindings.Add(new CommandBinding(mvmod.SearchAccountsCommand, mvmod.Search_Accounts, mvmod.CanSearch_Accounts));
            this.CommandBindings.Add(new CommandBinding(mvmod.UpdAccountCommand, mvmod.Upd_Account, mvmod.CanUpd_Account));

            // Заборонити вставку у поле перегляду пароля на сторінці "Інформація"
            InfoSitePassword.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (sender, e) => { }));
            // Заборонити вставку у поле перегляду пароля на сторінці "Шифрування"
            CryptSitePassword.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (sender, e) => { }));
            CryptSitePasswordSecretKey.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, (sender, e) => { }));
        }
        // Виникає під час зміни вкладки
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabStatistics.IsSelected)
            {
                mvmod.UpdateStatistics();
            }
            if (TabUpdDelAcc.IsSelected) mvmod?.LoadSelectedAccountForUpdate();
        }
        // Виникає під час натискання кнопки видалення
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (mvmod?.SelectedAccount != null)
            {
                var res = MessageBox.Show("Ви точно хочете видалити цей акаунт?", "Видалення акаунту", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    mvmod?.DeleteSelectedAccount();
                }
            }
        }

        private void Crypt_Click_1(object sender, RoutedEventArgs e)
        {
            if (mvmod?.SelectedAccount != null && mvmod?.SecretKey.Length > 0)
            {
                var res = MessageBox.Show("Ви точно хочете зашифрувати цей пароль?", "Шифрування / Розшифрування пароля", MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                {
                    mvmod?.CryptPassword();
                }
            }
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            mvmod?.LoadData("accounts.json");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mvmod?.SaveData("accounts.json");
        }

        // Заборонити введення пароля на сторінці "Інформація"
        private void InfoSitePassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }
        //// Перевірка на відкриття контекстного меню при натисканні Mouse2(заборонити).
        //private void InfoSitePassword_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    e.Handled = true;
        //}
        //// Перевірка на використання комбінацій Ctrl+C, Ctrl+V (заборонити).
        //private void InfoSitePassword_PreviewKeyUp(object sender, KeyEventArgs e)
        //{
        //    if (((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V) 
        //      ||((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.C))
        //    {
        //        e.Handled = true;
        //    }
        //}
    }
}
