using Kurs_Passman.Interfaces;
using Kurs_Passman.ViewModels;
using MaterialDesignThemes.Wpf;
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
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        private bool isDark = true;
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

            ToggleBaseColour();
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

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mvmod?.SelectedAccount != null)
            {
                await mvmod?.LoadSelectedAccountForUpdate();
                //MainTabControl.Visibility = Visibility.Visible;
                TabItemAccountInfo.Visibility = Visibility.Visible;
                TabUpdDelAcc.Visibility = Visibility.Visible;
                TabEncrypt.Visibility = Visibility.Visible;
                TabStatistics.Visibility = Visibility.Visible;
            }
            else
            {
                TabItemAccountInfo.Visibility = Visibility.Collapsed;
                TabUpdDelAcc.Visibility = Visibility.Collapsed;
                TabEncrypt.Visibility = Visibility.Collapsed;
                TabStatistics.Visibility = Visibility.Collapsed;
                MainTabControl.SelectedIndex = 1;
            }
        }

        // Заборонити введення пароля на сторінці "Інформація"
        private void InfoSitePassword_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void InfoSitePassword_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow wf = new SettingsWindow(mvmod, this);
            wf.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ToggleBaseColour();
        }
        private void ToggleBaseColour()
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme = isDark ? new MaterialDesignDarkTheme() : (IBaseTheme)new MaterialDesignLightTheme();
            theme.SetBaseTheme(baseTheme);

            if (isDark)
            {
                theme.Background = Color.FromArgb(255, 0, 0, 0);                        // Темна тема - чорний колір                - задній фон
                theme.Paper = Color.FromArgb(255, 255, 255, 255);                       // Темна тема - білий колір                 - шрифт
                theme.SnackbarRipple = Color.FromArgb(255, 4, 32, 44);                     // Темна тема - темно- зелений колір        
                theme.SnackbarBackground = Color.FromArgb(255, 48, 64, 63);                      // Темна тема - помірно-зелений колір
                theme.ValidationError = Color.FromArgb(255, 90, 112, 100);                         // Темна тема - сірий колір
                theme.CardBackground = Color.FromArgb(255, 98, 126, 93);                // Темна тема - темно-сірий колір
                theme.Divider = Color.FromArgb(255, 202, 209, 201);                     // Темна тема - темно-сірий колір
                theme.ColumnHeader = Color.FromArgb(255, 255, 255, 255);
                
                theme.SetPrimaryColor(Color.FromArgb(255, 255, 255, 0));
            }
            else
            {
                theme.Background = Color.FromArgb(255, 255, 255, 195);                  // Світла тема - світло-жовтий колір    - задній фон
                theme.Paper = Color.FromArgb(255, 0, 0, 0);                             // Світла тема - чорний колір           - шрифт
                theme.SnackbarRipple = Color.FromArgb(255, 255, 255, 240);                 // Світла тема -      
                theme.SnackbarBackground = Color.FromArgb(255, 207, 202, 198);                   // Світла тема - 
                theme.ValidationError = Color.FromArgb(255, 203, 184, 171);                        // Світла тема -
                theme.CardBackground = Color.FromArgb(255, 162, 140, 125);              // Світла тема - 
                theme.Divider = Color.FromArgb(255, 128, 117, 111);                     // Світла тема - 
                theme.ColumnHeader = Color.FromArgb(255, 0, 0, 0);
                theme.SetPrimaryColor(Color.FromArgb(255, 0, 100, 255));
            }
            _paletteHelper.SetTheme(theme);
            isDark = !isDark;
        }
    }
}
