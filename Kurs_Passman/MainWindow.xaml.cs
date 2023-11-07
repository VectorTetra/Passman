using Kurs_Passman.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            MainViewModel mvmod = this.Resources["mvmod"] as MainViewModel;
            this.DataContext = mvmod;
            this.mvmod = mvmod;
            this.CommandBindings.Add(new CommandBinding(mvmod.AddAccountCommand, mvmod.Add_Account, mvmod.CanAdd_Account));
            this.CommandBindings.Add(new CommandBinding(mvmod.SearchAccountsCommand, mvmod.Search_Accounts, mvmod.CanSearch_Accounts));
            this.CommandBindings.Add(new CommandBinding(mvmod.UpdAccountCommand, mvmod.Upd_Account, mvmod.CanUpd_Account));
            this.CommandBindings.Add(new CommandBinding(mvmod.SortBySimpleKeyCommand, mvmod.SortBySimpleKey, mvmod.CanSortBySimpleKey));
            this.CommandBindings.Add(new CommandBinding(mvmod.SortBySimpleValueCommand, mvmod.SortBySimpleValue, mvmod.CanSortBySimpleValue));
            this.CommandBindings.Add(new CommandBinding(mvmod.SortByDiffLoginCommand, mvmod.SortByDiffLogin, mvmod.CanSortByDiffLogin));
            this.CommandBindings.Add(new CommandBinding(mvmod.SortByDiffPasswordCommand, mvmod.SortByDiffPassword, mvmod.CanSortByDiffPassword));
            this.CommandBindings.Add(new CommandBinding(mvmod.SortByDiffQuantCommand, mvmod.SortByDiffQuant, mvmod.CanSortByDiffQuant));
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
    }
}
