//using JB.Collections.Reactive;
using Kurs_Passman.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Kurs_Passman.Commands;
using Microsoft.EntityFrameworkCore;
using System.Windows.Documents;
using Kurs_Passman.Interfaces;
using System.Threading;
using System.Windows;

namespace Kurs_Passman.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            db = new();
            db.Database.EnsureCreated();
            db.Accounts.Load();
            Accounts = db.Accounts.Local.ToObservableCollection();
            SearchedAccounts = db.Accounts.Local.ToObservableCollection();
        }
        // Database - context для взаємодії з БД
        PassManContext db;
        // Контекст синхронізації
        SynchronizationContext _uiContext = SynchronizationContext.Current;
        // Accounts - повна колекція всіх акаунтів
        #region Accounts
        private ObservableCollection<Account> _accounts;
        public ObservableCollection<Account> Accounts
        {
            get { return _accounts; }
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }
        #endregion Accounts
        // SelectedAccount - посилання на обраний акаунт в списку акаунтів 
        #region SelectedAccount
        private Account _selectedAccount = null;
        public Account SelectedAccount
        {
            get { return _selectedAccount; }
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
                if (SelectedAccount != null)
                { CryptButtonCaption = (SelectedAccount.Encrypted == 1 ? "Розшифрувати" : "Зашифрувати"); }

            }
        }
        #endregion SelectedAccount
        // SearchedAccounts - список всіх акаунтів, які задовольняють критерії пошуку
        #region SearchedAccounts
        private ObservableCollection<Account> _searchedaccounts;
        public ObservableCollection<Account> SearchedAccounts
        {
            get { return _searchedaccounts; }
            set
            {
                _searchedaccounts = value;
                OnPropertyChanged(nameof(SearchedAccounts));
            }
        }
        #endregion SearchedAccounts
        // StatDictionaries - масиви-словники, які показують статистику
        // По логіну - (логін, кількість)
        // По паролю - (пароль, кількість)
        // По логіну і паролю - (пара, кількість)
        #region StatDictionaries

        // Login_Stat - Cтатистика По логіну - (логін, кількість)
        #region Login_Stat
        private Dictionary<string, int> _login_stat = new();
        public Dictionary<string, int> Login_Stat
        {
            get { return _login_stat; }
            set
            {
                _login_stat = value;
                OnPropertyChanged(nameof(Login_Stat));
            }
        }
        #endregion Login_Stat

        // Password_Stat - Cтатистика По паролю - (пароль, кількість)
        #region Password_Stat
        private Dictionary<string, int> _password_stat = new();
        public Dictionary<string, int> Password_Stat
        {
            get { return _password_stat; }
            set
            {
                _password_stat = value;
                OnPropertyChanged(nameof(Password_Stat));
            }
        }
        #endregion Password_Stat

        // Login_Password_Stat - Cтатистика По логіну і паролю - (пара, кількість)
        #region Login_Password_Stat
        private Dictionary<List<string>, int> _login_password_stat = new();
        public Dictionary<List<string>, int> Login_Password_Stat
        {
            get { return _login_password_stat; }
            set
            {
                _login_password_stat = value;
                OnPropertyChanged(nameof(Login_Password_Stat));
            }
        }
        #endregion Login_Password_Stat

        #endregion StatDictionaries

        // Filters - властивості - прив'язки до критеріїв пошуку
        #region Filters

        #region IsSearchByLogin
        private bool _IsSearchByLogin = false;
        public bool IsSearchByLogin
        {
            get { return _IsSearchByLogin; }
            set
            {
                _IsSearchByLogin = value;
                OnPropertyChanged(nameof(IsSearchByLogin));
            }
        }
        #endregion IsSearchByLogin

        #region IsSearchBySiteName
        private bool _IsSearchBySiteName = false;
        public bool IsSearchBySiteName
        {
            get { return _IsSearchBySiteName; }
            set
            {
                _IsSearchBySiteName = value;
                OnPropertyChanged(nameof(IsSearchBySiteName));
            }
        }
        #endregion IsSearchBySiteName

        #region IsSearchBySiteDescription
        private bool _IsSearchBySiteDescription = false;
        public bool IsSearchBySiteDescription
        {
            get { return _IsSearchBySiteDescription; }
            set
            {
                _IsSearchBySiteDescription = value;
                OnPropertyChanged(nameof(IsSearchBySiteDescription));
            }
        }
        #endregion IsSearchBySiteDescription

        #region IsSearchBySiteAddress
        private bool _IsSearchBySiteAddress = false;
        public bool IsSearchBySiteAddress
        {
            get { return _IsSearchBySiteAddress; }
            set
            {
                _IsSearchBySiteAddress = value;
                OnPropertyChanged(nameof(IsSearchBySiteAddress));
            }
        }
        #endregion IsSearchBySiteAddress

        #region IsOrderAscending
        private bool _IsOrderAscending = true;
        public bool IsOrderAscending
        {
            get { return _IsOrderAscending; }
            set
            {
                _IsOrderAscending = value;
                OnPropertyChanged(nameof(IsOrderAscending));
            }
        }
        #endregion IsOrderAscending
        #endregion Filters

        // _dialogService - сервіс діалогових вікон для відображення сповіщень
        #region DialogService
        private readonly IDialogService _dialogService;
        #endregion DialogService

        #region BindingStrings

        #region AddingAccount

        #region AddAccName
        private string _AddAccName = string.Empty;
        public string AddAccName
        {
            get { return _AddAccName; }
            set
            {
                _AddAccName = value;
                OnPropertyChanged(nameof(AddAccName));
            }
        }
        #endregion AddAccName
        #region AddAccDescription
        private string _AddAccDescription = string.Empty;
        public string AddAccDescription
        {
            get { return _AddAccDescription; }
            set
            {
                _AddAccDescription = value;
                OnPropertyChanged(nameof(AddAccDescription));
            }
        }
        #endregion AddAccDescription
        #region AddAccAddress
        private string _AddAccAddress = string.Empty;
        public string AddAccAddress
        {
            get { return _AddAccAddress; }
            set
            {
                _AddAccAddress = value;
                OnPropertyChanged(nameof(AddAccAddress));
            }
        }
        #endregion AddAccAddress
        #region AddAccLogin
        private string _AddAccLogin = string.Empty;
        public string AddAccLogin
        {
            get { return _AddAccLogin; }
            set
            {
                _AddAccLogin = value;
                OnPropertyChanged(nameof(AddAccLogin));
            }
        }
        #endregion AddAccLogin
        #region AddAccPassword
        private string _AddAccPassword = string.Empty;
        public string AddAccPassword
        {
            get { return _AddAccPassword; }
            set
            {
                _AddAccPassword = value;
                //if (IsAddingPasswordMin8) { AddingPasswordMin8Color = new SolidColorBrush(Color.FromArgb(0,0,100,0)); }
                OnPropertyChanged(nameof(AddAccPassword));
            }
        }
        #endregion AddAccPassword
        #endregion AddingAccount

        #region UpdatingAccount
        #region UpdAccName
        private string _UpdAccName = string.Empty;
        public string UpdAccName
        {
            get { return _UpdAccName; }
            set
            {
                _UpdAccName = value;
                OnPropertyChanged(nameof(UpdAccName));
            }
        }
        #endregion UpdAccName
        #region UpdAccDescription
        private string _UpdAccDescription = string.Empty;
        public string UpdAccDescription
        {
            get { return _UpdAccDescription; }
            set
            {
                _UpdAccDescription = value;
                OnPropertyChanged(nameof(UpdAccDescription));
            }
        }
        #endregion UpdAccDescription
        #region UpdAccAddress
        private string _UpdAccAddress = string.Empty;
        public string UpdAccAddress
        {
            get { return _UpdAccAddress; }
            set
            {
                _UpdAccAddress = value;
                OnPropertyChanged(nameof(UpdAccAddress));
            }
        }
        #endregion UpdAccUpdress
        #region UpdAccLogin
        private string _UpdAccLogin = string.Empty;
        public string UpdAccLogin
        {
            get { return _UpdAccLogin; }
            set
            {
                _UpdAccLogin = value;
                OnPropertyChanged(nameof(UpdAccLogin));
            }
        }
        #endregion UpdAccLogin
        #region UpdAccPassword
        private string _UpdAccPassword = string.Empty;
        public string UpdAccPassword
        {
            get { return _UpdAccPassword; }
            set
            {
                _UpdAccPassword = value;
                OnPropertyChanged(nameof(UpdAccPassword));
            }
        }
        #endregion UpdAccPassword
        #endregion UpdatingAccount

        #region SecretKey
        private string _SecretKey = string.Empty;
        public string SecretKey
        {
            get { return _SecretKey; }
            set
            {
                _SecretKey = value;
                OnPropertyChanged(nameof(SecretKey));
            }
        }
        #endregion SecretKey

        #region CryptButtonCaption
        private string _CryptButtonCaption = string.Empty;
        public string CryptButtonCaption
        {
            get { return _CryptButtonCaption; }
            set
            {
                _CryptButtonCaption = value;
                OnPropertyChanged(nameof(CryptButtonCaption));
            }
        }
        #endregion CryptButtonCaption

        #region SearchingExpression
        private string _SearchingExpression = string.Empty;
        public string SearchingExpression
        {
            get { return _SearchingExpression; }
            set
            {
                _SearchingExpression = value;
                OnPropertyChanged(nameof(SearchingExpression));
            }
        }
        #endregion SearchingExpression
        #endregion BindingStrings

        #region Password_Recommendations

        public bool IsAddingPasswordRequirementsMet 
        { 
            get {
                return IsAddingPasswordMin8 &
                        IsAddingPasswordHasUppercase &
                        IsAddingPasswordHasLowercase &
                        IsAddingPasswordHasDigits &
                        IsAddingPasswordHasSpecialChars;
            } 
        }
        public bool IsUpdingPasswordRequirementsMet
        {
            get
            {
                return IsUpdingPasswordMin8 &
                        IsUpdingPasswordHasUppercase &
                        IsUpdingPasswordHasLowercase &
                        IsUpdingPasswordHasDigits &
                        IsUpdingPasswordHasSpecialChars;
            }
        }
        #region IsPasswordRecommendationsEnabled
        private bool _IsPasswordRecommendationsEnabled = false;

        public bool IsPasswordRecommendationsEnabled
        {
            get { return _IsPasswordRecommendationsEnabled; }
            set
            {
                _IsPasswordRecommendationsEnabled = value;
                OnPropertyChanged(nameof(IsPasswordRecommendationsEnabled));
            }
        }

        #endregion IsPasswordRecommendationsEnabled

        #region AddingPasswordRecommendations

        #region IsAddingPasswordMin8
        public bool IsAddingPasswordMin8
        {
            get { return AddAccPassword.Length >= 8; }
            
        }
        #endregion IsAddingPasswordMin8

        #region IsAddingPasswordHasUppercase
        public bool IsAddingPasswordHasUppercase
        {
            get { return AddAccPassword.Any(key => char.IsUpper(key)); }

        }
        #endregion IsAddingPasswordHasUppercase

        #region IsAddingPasswordHasLowercase
        public bool IsAddingPasswordHasLowercase
        {
            get { return AddAccPassword.Any(key => char.IsLower(key)); }

        }
        #endregion IsAddingPasswordHasLowercase

        #region IsAddingPasswordHasDigits
        public bool IsAddingPasswordHasDigits
        {
            get { return AddAccPassword.Any(key => char.IsDigit(key)); }

        }
        #endregion IsAddingPasswordHasDigits

        #region IsAddingPasswordHasSpecialChars
        public bool IsAddingPasswordHasSpecialChars
        {
            get { return AddAccPassword.Any(key => !char.IsLetterOrDigit(key)); }

        }
        #endregion IsAddingPasswordHasSpecialChars

        #endregion AddingPasswordRecommendations

        #region UpdingPasswordRecommendations

        #region IsUpdingPasswordMin8
        public bool IsUpdingPasswordMin8
        {
            get { return UpdAccPassword.Length >= 8; }

        }
        #endregion IsUpdingPasswordMin8

        #region IsUpdingPasswordHasUppercase
        public bool IsUpdingPasswordHasUppercase
        {
            get { return UpdAccPassword.Any(key => char.IsUpper(key)); }

        }
        #endregion IsUpdingPasswordHasUppercase

        #region IsUpdingPasswordHasLowercase
        public bool IsUpdingPasswordHasLowercase
        {
            get { return UpdAccPassword.Any(key => char.IsLower(key)); }

        }
        #endregion IsUpdingPasswordHasLowercase

        #region IsUpdingPasswordHasDigits
        public bool IsUpdingPasswordHasDigits
        {
            get { return UpdAccPassword.Any(key => char.IsDigit(key)); }

        }
        #endregion IsUpdingPasswordHasDigits

        #region IsUpdingPasswordHasSpecialChars
        public bool IsUpdingPasswordHasSpecialChars
        {
            get { return UpdAccPassword.Any(key => !char.IsLetterOrDigit(key)); }

        }
        #endregion IsUpdingPasswordHasSpecialChars

        #endregion UpdingPasswordRecommendations

        #region AddingPasswordColors

        #region AddingPasswordMin8Color
        public Brush AddingPasswordMin8Color 
        {
            get {
                if (IsAddingPasswordMin8)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
                }
                else return new SolidColorBrush(Color.FromArgb(255, 139, 0, 0));
                
            }
        }
        #endregion AddingPasswordMin8Color

        #region AddingPasswordHasUppercaseColor
        public Brush AddingPasswordHasUppercaseColor
        {
            get
            {
                if (IsAddingPasswordHasUppercase)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
                }
                else return new SolidColorBrush(Color.FromArgb(255, 139, 0, 0));

            }
        }
        #endregion AddingPasswordHasUppercaseColor

        #region AddingPasswordHasLowercaseColor
        public Brush AddingPasswordHasLowercaseColor
        {
            get
            {
                if (IsAddingPasswordHasLowercase)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
                }
                else return new SolidColorBrush(Color.FromArgb(255, 139, 0, 0));

            }
        }
        #endregion AddingPasswordHasLowercaseColor

        #region AddingPasswordHasDigitsColor
        public Brush AddingPasswordHasDigitsColor
        {
            get
            {
                if (IsAddingPasswordHasDigits)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
                }
                else return new SolidColorBrush(Color.FromArgb(255, 139, 0, 0));

            }
        }
        #endregion AddingPasswordHasDigitsColor

        #region AddingPasswordHasSpecialCharsColor
        public Brush AddingPasswordHasSpecialCharsColor
        {
            get
            {
                if (IsAddingPasswordHasSpecialChars)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
                }
                else return new SolidColorBrush(Color.FromArgb(255, 139, 0, 0));

            }
        }
        #endregion AddingPasswordHasSpecialCharsColor

        #endregion AddingPasswordColors

        #region UpdingPasswordColors

        #region UpdingPasswordMin8Color
        public Brush UpdingPasswordMin8Color
        {
            get
            {
                if (IsUpdingPasswordMin8)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
                }
                else return new SolidColorBrush(Color.FromArgb(255, 139, 0, 0));

            }
        }
        #endregion UpdingPasswordMin8Color

        #region UpdingPasswordHasUppercaseColor
        public Brush UpdingPasswordHasUppercaseColor
        {
            get
            {
                if (IsUpdingPasswordHasUppercase)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
                }
                else return new SolidColorBrush(Color.FromArgb(255, 139, 0, 0));

            }
        }
        #endregion UpdingPasswordHasUppercaseColor

        #region UpdingPasswordHasLowercaseColor
        public Brush UpdingPasswordHasLowercaseColor
        {
            get
            {
                if (IsUpdingPasswordHasLowercase)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
                }
                else return new SolidColorBrush(Color.FromArgb(255, 139, 0, 0));

            }
        }
        #endregion UpdingPasswordHasLowercaseColor

        #region UpdingPasswordHasDigitsColor
        public Brush UpdingPasswordHasDigitsColor
        {
            get
            {
                if (IsUpdingPasswordHasDigits)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
                }
                else return new SolidColorBrush(Color.FromArgb(255, 139, 0, 0));

            }
        }
        #endregion UpdingPasswordHasDigitsColor

        #region UpdingPasswordHasSpecialCharsColor
        public Brush UpdingPasswordHasSpecialCharsColor
        {
            get
            {
                if (IsUpdingPasswordHasSpecialChars)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 0, 100, 0));
                }
                else return new SolidColorBrush(Color.FromArgb(255, 139, 0, 0));

            }
        }
        #endregion UpdingPasswordHasSpecialCharsColor

        #endregion UpdingPasswordColors

        #endregion Password_Recommendations

        #region PasswordGeneration
        #region IsOfferPasswordGeneration
        private bool _IsOfferPasswordGeneration = false;

        public bool IsOfferPasswordGeneration
        {
            get { return _IsOfferPasswordGeneration; }
            set
            {
                _IsOfferPasswordGeneration = value;
                OnPropertyChanged(nameof(IsOfferPasswordGeneration));
                if (IsOfferPasswordGeneration)
                {
                    IsPasswordGenerationButtonsVisible = Visibility.Visible;
                }
                else IsPasswordGenerationButtonsVisible = Visibility.Collapsed;
            }
        }

        #endregion IsOfferPasswordGeneration

        #region IsPasswordGenerationButtonsVisible
        public Visibility _IsPasswordGenerationButtonsVisible = Visibility.Collapsed;
        public Visibility IsPasswordGenerationButtonsVisible
        {
            get { return _IsPasswordGenerationButtonsVisible; }
            set 
            {
                _IsPasswordGenerationButtonsVisible = value;
                OnPropertyChanged(nameof(IsPasswordGenerationButtonsVisible));
            }
            
        }
        #endregion IsPasswordGenerationButtonsVisible

        #endregion PasswordGeneration

        #region Commands

        #region Command_AddAccount
        private DelegateCommand add_account_command = null;
        public ICommand AddAccountCommand
        {
            get
            {
                if (add_account_command == null)
                {
                    add_account_command = new DelegateCommand(
                        async d =>
                        {

                            await Task.Run(() =>
                            {
                                try
                                {
                                    _uiContext.Send(d => { db.Accounts.Add(new Account { Login = AddAccLogin, SiteName = AddAccName, SiteAddress = AddAccAddress, Password = AddAccPassword, SiteDescription = AddAccDescription }); }, null);
                                    _uiContext.Send(d => { db.SaveChangesAsync(); }, null);
                                    AddAccAddress = string.Empty;
                                    AddAccLogin = string.Empty;
                                    AddAccDescription = string.Empty;
                                    AddAccName = string.Empty;
                                    AddAccPassword = string.Empty;
                                    if (SearchingExpression == string.Empty)
                                    {
                                        SearchedAccounts = Accounts;
                                    }
                                    if (SelectedAccount == null && SearchedAccounts.Count > 0)
                                    {
                                        SelectedAccount = SearchedAccounts[SearchedAccounts.Count - 1];
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _uiContext.Send(d => { _dialogService.MessageDialog(ex.Message, ex.GetType().ToString()); }, null);
                                }
                            });
                        },
                        d =>
                        {
                            if (IsPasswordRecommendationsEnabled)
                            {
                                return (AddAccAddress.Length > 0 &
                                AddAccLogin.Length > 0 &
                                AddAccName.Length > 0 &
                                IsAddingPasswordRequirementsMet);
                            }
                            else return (AddAccAddress.Length > 0 &&
                                AddAccLogin.Length > 0 &&
                                AddAccName.Length > 0 &&
                                AddAccPassword.Length > 0);
                        });
                }
                return add_account_command;
            }
        }
        #endregion Command_AddAccount

        #region Command_SearchAccounts
        private DelegateCommand search_accounts_command = null;
        public ICommand SearchAccountsCommand
        {
            get
            {
                if (search_accounts_command == null)
                {
                    search_accounts_command = new DelegateCommand(
                        async d =>
                        {
                            try
                            {
                                await Task.Run(() =>
                                {
                                    if (SearchingExpression == string.Empty)
                                    {
                                        SearchedAccounts = Accounts;
                                    }
                                    else
                                    {
                                        SearchedAccounts = new ObservableCollection<Account>();
                                        Regex regex = new Regex(SearchingExpression);
                                        if (IsSearchByLogin)
                                        {
                                            foreach (var item in Accounts)
                                            {
                                                if (regex.IsMatch(item.Login) && !SearchedAccounts.Contains(item))
                                                { _uiContext.Send(d => { SearchedAccounts.Add(item); }, null); }
                                            }
                                        }
                                        if (IsSearchBySiteAddress)
                                        {
                                            foreach (var item in Accounts)
                                            {
                                                if (regex.IsMatch(item.SiteAddress) && !SearchedAccounts.Contains(item))
                                                { _uiContext.Send(d => { SearchedAccounts.Add(item); }, null); }
                                            }
                                        }
                                        if (IsSearchBySiteName)
                                        {
                                            foreach (var item in Accounts)
                                            {
                                                if (regex.IsMatch(item.SiteName) && !SearchedAccounts.Contains(item))
                                                { _uiContext.Send(d => { SearchedAccounts.Add(item); }, null); }
                                            }
                                        }
                                        if (IsSearchBySiteDescription)
                                        {
                                            foreach (var item in Accounts)
                                            {
                                                if (regex.IsMatch(item.SiteDescription) && !SearchedAccounts.Contains(item))
                                                { _uiContext.Send(d => { SearchedAccounts.Add(item); }, null); }
                                            }
                                        }
                                        if (!IsSearchByLogin && !IsSearchBySiteAddress && !IsSearchBySiteDescription && !IsSearchBySiteName)
                                        {
                                            foreach (var item in Accounts)
                                            {
                                                if ((regex.IsMatch(item.SiteDescription) || regex.IsMatch(item.SiteName) || regex.IsMatch(item.SiteAddress) || regex.IsMatch(item.Login)) && !SearchedAccounts.Contains(item))
                                                { _uiContext.Send(d => { SearchedAccounts.Add(item); }, null); }
                                            }
                                        }
                                    }
                                });
                            }
                            catch (Exception ex)
                            {
                                _uiContext.Send(d => { _dialogService.MessageDialog(ex.Message, ex.GetType().ToString()); }, null);
                            }

                        }, null);
                }
                return search_accounts_command;
            }
        }
        #endregion Command_SearchAccounts

        #region Command_UpdAccount
        private DelegateCommand upd_account_command = null;
        public ICommand UpdAccountCommand
        {
            get
            {
                if (upd_account_command == null)
                {
                    upd_account_command = new DelegateCommand(
                        async d =>
                        {
                            try
                            {
                                await Task.Run(() =>
                                {
                                    if (_dialogService.ConfirmDialog("Ви точно хочете відредагувати обраний акаунт?", "Підтвердження редагування"))
                                    {
                                        SelectedAccount.SiteName = UpdAccName.ToString();
                                        SelectedAccount.SiteAddress = UpdAccAddress.ToString();
                                        if (SelectedAccount.Password != UpdAccPassword.ToString()) { SelectedAccount.Encrypted = 0; }
                                        SelectedAccount.Password = UpdAccPassword.ToString();
                                        SelectedAccount.Login = UpdAccLogin.ToString();
                                        SelectedAccount.SiteDescription = UpdAccDescription.ToString();

                                        _uiContext.Send(d => { db.Entry(SelectedAccount).State = EntityState.Modified; }, null);
                                        _uiContext.Send(d => { db.SaveChangesAsync(); }, null);
                                        //_uiContext.Send(d => { OnPropertyChanged(nameof(SearchedAccounts)); }, null);
                                        //OnPropertyChanged(nameof(SearchedAccounts));
                                        SearchedAccounts = new(SearchedAccounts);
                                        var sel = SelectedAccount;
                                        SelectedAccount = null;
                                        SelectedAccount = sel;
                                    }
                                });
                            }
                            catch (Exception ex)
                            {
                                _uiContext.Send(d => { _dialogService.MessageDialog(ex.Message, ex.GetType().ToString()); }, null);
                            }

                        },
                        d => 
                        {
                            if (IsPasswordRecommendationsEnabled)
                            {
                                return (UpdAccAddress.Length > 0 &
                                UpdAccLogin.Length > 0 &
                                UpdAccName.Length > 0 &
                                IsUpdingPasswordRequirementsMet &
                                SelectedAccount != null);
                            }
                            else return (UpdAccAddress.Length > 0 &
                                UpdAccLogin.Length > 0 &
                                UpdAccName.Length > 0 &
                                UpdAccPassword.Length > 0 &
                                SelectedAccount != null);

                        });
                }
                return upd_account_command;
            }
        }
        #endregion Command_UpdAccount

        #region Command_DelAccount
        private DelegateCommand del_account_command = null;
        public ICommand DelAccountCommand
        {
            get
            {
                if (del_account_command == null)
                {
                    del_account_command = new DelegateCommand(
                    async d =>
                    {
                        try
                        {
                            await Task.Run(() =>
                            {
                                _uiContext.Send(del =>
                                {
                                    if (_dialogService.ConfirmDialog("Ви точно хочете видалити обраний акаунт?", "Підтвердження видалення"))
                                    {
                                        db.Accounts.Remove(SelectedAccount);
                                        db.SaveChangesAsync();
                                    }
                                    if (SelectedAccount == null && SearchedAccounts.Count > 0)
                                    {
                                        SelectedAccount = SearchedAccounts[SearchedAccounts.Count - 1];
                                    }
                                }, null);

                            });
                        }
                        catch (Exception ex)
                        {
                            _uiContext.Send(d => { _dialogService.MessageDialog(ex.Message, ex.GetType().ToString()); }, null);
                        }
                    },
                    d =>
                    {
                        return SelectedAccount != null;
                    });
                }
                return del_account_command;
            }
        }
        #endregion Command_UpdAccount

        #region Command_CryptPassword
        private DelegateCommand crypt_password_command = null;
        public ICommand CryptPasswordCommand
        {
            get
            {
                if (crypt_password_command == null)
                {
                    crypt_password_command = new DelegateCommand(
                    async d =>
                    {
                        try
                        {
                            await Task.Run(() =>
                            {
                                string message = SelectedAccount.Encrypted == 1 ?
                                    "Ви точно хочете розшифрувати пароль обраного акаунту?" :
                                    "Ви точно хочете зашифрувати пароль обраного акаунту?";
                                if (_dialogService.ConfirmDialog(message, "Підтвердження"))
                                {
                                    SelectedAccount.Crypt(SecretKey);
                                    _uiContext.Send(del =>
                                    {
                                        db.Entry(SelectedAccount).State = EntityState.Modified;
                                        db.SaveChangesAsync();
                                        SelectedAccount = SelectedAccount;
                                        SecretKey = "";
                                    }, null);
                                    
                                }
                            });
                        }
                        catch (System.Security.Cryptography.CryptographicException ex)
                        {
                            _uiContext.Send(d => { _dialogService.MessageDialog("Введено неправильний ключ шифрування!", "Помилка шифрування"); }, null);
                        }
                        catch (Exception ex)
                        {
                            _uiContext.Send(d => { _dialogService.MessageDialog(ex.Message, ex.GetType().ToString()); }, null);
                        }

                    },
                    d =>
                    {
                        return (SelectedAccount != null && SecretKey.Length > 0);
                    });
                }
                return crypt_password_command;
            }
        }
        #endregion Command_CryptPassword

        #region Command_GenerateAddingPassword
        private DelegateCommand generate_adding_password_command = null;
        public ICommand GenerateAddingPasswordCommand
        {
            get
            {
                if (generate_adding_password_command == null)
                {
                    generate_adding_password_command = new DelegateCommand(
                    async d =>
                    {
                        try
                        {
                            await Task.Run(() =>
                            {
                                
                                _uiContext.Send(del =>
                                {
                                    AddAccPassword = AesOperation.GetRandomPasswordString();
                                }, null);
                            });
                        }
                        catch (Exception ex)
                        {
                            _uiContext.Send(d => { _dialogService.MessageDialog(ex.Message, ex.GetType().ToString()); }, null);
                        }

                    },
                   null);
                }
                return generate_adding_password_command;
            }
        }
        #endregion Command_GenerateAddingPassword

        #region Command_GenerateUpdingPassword
        private DelegateCommand generate_upding_password_command = null;
        public ICommand GenerateUpdingPasswordCommand
        {
            get
            {
                if (generate_upding_password_command == null)
                {
                    generate_upding_password_command = new DelegateCommand(
                    async d =>
                    {
                        try
                        {
                            await Task.Run(() =>
                            {

                                _uiContext.Send(del =>
                                {
                                    UpdAccPassword = AesOperation.GetRandomPasswordString();
                                }, null);
                            });
                        }
                        catch (Exception ex)
                        {
                            _uiContext.Send(d => { _dialogService.MessageDialog(ex.Message, ex.GetType().ToString()); }, null);
                        }

                    },
                   null);
                }
                return generate_upding_password_command;
            }
        }
        #endregion Command_GenerateUpdingPassword
        #endregion Commands

        #region Methods
        // Оновлення даних у вкладці "Статистика" відбувається через callback у обробнику події зміни вкладки
        public async Task UpdateStatistics()
        {
            try
            {
                await Task.Run(() =>
                {
                    // Створюємо новий криптографічний об'єкт симетричного шифрування.
                    // Він необхідний, щоб отримати випадково згенерований ключ шифрування
                    var cipherKey = AesOperation.GetRandomString();
                    string crypted_pass = string.Empty;
                    _uiContext.Send(d =>
                    {

                        Login_Stat.Clear();
                        Password_Stat.Clear();
                        Login_Password_Stat.Clear();
                        foreach (var item in Accounts)
                        {
                            // Якщо немає акаунтів з таким логіном - додати, якщо є - збільшити лічильник на 1
                            if (!Login_Stat.ContainsKey(item.Login))
                            {
                                Login_Stat.TryAdd(item.Login, 1);
                            }
                            else
                            {
                                Login_Stat[item.Login] += 1;
                            }
                            // Якщо немає акаунтів з таким паролем - додати, якщо є - збільшити лічильник на 1
                            crypted_pass = AesOperation.EncryptString(cipherKey, item.Password);
                            if (!Password_Stat.ContainsKey(crypted_pass))
                            {
                                Password_Stat.TryAdd(crypted_pass, 1);
                            }
                            else
                            {
                                Password_Stat[crypted_pass] += 1;
                            }

                            List<string> pair = new List<string>() { item.Login, crypted_pass };
                            // Якщо немає акаунтів з такими логіном і паролем - додати, якщо є - збільшити лічильник на 1
                            if (!Login_Password_Stat.Keys.Any(key => key.SequenceEqual(pair)))
                            {
                                Login_Password_Stat.TryAdd(pair, 1);
                            }
                            else
                            {
                                Parallel.ForEach(Login_Password_Stat.Keys, (kk) => { if (kk.SequenceEqual(pair)) { Login_Password_Stat[kk] += 1; } });
                            }
                        }
                        Login_Stat = new(Login_Stat);
                        Password_Stat = new(Password_Stat);
                        Login_Password_Stat = new(Login_Password_Stat);
                    }, null);
                });
            }
            catch (Exception ex)
            {
                _uiContext.Send(d => { _dialogService.MessageDialog(ex.InnerException.Message, "Помилка!"); }, null);
            }
        }

        // Оновлення даних у вкладці "Оновлення акаунту" відбувається через callback у обробнику події зміни вкладки
        public async Task LoadSelectedAccountForUpdate()
        {
            try
            {
                await Task.Run(() =>
                {
                    if (SelectedAccount != null)
                    {
                        UpdAccAddress = SelectedAccount.SiteAddress.ToString();
                        UpdAccDescription = SelectedAccount?.SiteDescription.ToString();
                        UpdAccLogin = SelectedAccount.Login.ToString();
                        UpdAccName = SelectedAccount.SiteName.ToString();
                        UpdAccPassword = SelectedAccount.Password.ToString();
                    }
                });
            }
            catch (Exception ex)
            {
                _uiContext.Send(d => { _dialogService.MessageDialog(ex.Message, ex.GetType().ToString()); }, null);
            }
        }
        // Видалення акаунту у вкладці "Оновлення акаунту" відбувається через callback у обробнику події натискання на кнопку із підтвердженням
        #endregion Methods
    }
}
