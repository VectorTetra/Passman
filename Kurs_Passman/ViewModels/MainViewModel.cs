﻿//using JB.Collections.Reactive;
using Kurs_Passman.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Kurs_Passman.Commands;
using Microsoft.EntityFrameworkCore;
using System.Windows.Documents;
using Kurs_Passman.Interfaces;

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
        PassManContext db;
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
                CryptButtonCaption = (SelectedAccount.Encrypted == 1 ? "Розшифрувати" : "Зашифрувати");
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

        #region DialogService
        private readonly IDialogService _dialogService;
        #endregion DialogService

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
                        d =>
                        {
                            db.Accounts.Add(new Account { Login = AddAccLogin, SiteName = AddAccName, SiteAddress = AddAccAddress, Password = AddAccPassword, SiteDescription = AddAccDescription });
                            db.SaveChangesAsync();
                            AddAccAddress = string.Empty;
                            AddAccLogin = string.Empty;
                            AddAccDescription = string.Empty;
                            AddAccName = string.Empty;
                            AddAccPassword = string.Empty;
                            if (SearchingExpression == string.Empty)
                            {
                                SearchedAccounts = Accounts;
                            }
                        },
                        d =>
                        {
                            return (AddAccAddress.Length > 0 &&
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
                        d =>
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
                                        { SearchedAccounts.Add(item); }
                                    }
                                }
                                if (IsSearchBySiteAddress)
                                {
                                    foreach (var item in Accounts)
                                    {
                                        if (regex.IsMatch(item.SiteAddress) && !SearchedAccounts.Contains(item))
                                        { SearchedAccounts.Add(item); }
                                    }
                                }
                                if (IsSearchBySiteName)
                                {
                                    foreach (var item in Accounts)
                                    {
                                        if (regex.IsMatch(item.SiteName) && !SearchedAccounts.Contains(item))
                                        { SearchedAccounts.Add(item); }
                                    }
                                }
                                if (IsSearchBySiteDescription)
                                {
                                    foreach (var item in Accounts)
                                    {
                                        if (regex.IsMatch(item.SiteDescription) && !SearchedAccounts.Contains(item))
                                        { SearchedAccounts.Add(item); }
                                    }
                                }
                                if (!IsSearchByLogin && !IsSearchBySiteAddress && !IsSearchBySiteDescription && !IsSearchBySiteName)
                                {
                                    foreach (var item in Accounts)
                                    {
                                        if ((regex.IsMatch(item.SiteDescription) || regex.IsMatch(item.SiteName) || regex.IsMatch(item.SiteAddress) || regex.IsMatch(item.Login)) && !SearchedAccounts.Contains(item))
                                        { SearchedAccounts.Add(item); }
                                    }
                                }
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
                        d =>
                        {
                            if (_dialogService.ConfirmDialog("Ви точно хочете відредагувати обраний акаунт?"))
                            {
                                SelectedAccount.SiteName = UpdAccName.ToString();
                                SelectedAccount.SiteAddress = UpdAccAddress.ToString();
                                if (SelectedAccount.Password != UpdAccPassword.ToString()) { SelectedAccount.Encrypted = 0; }
                                SelectedAccount.Password = UpdAccPassword.ToString();
                                SelectedAccount.Login = UpdAccLogin.ToString();
                                SelectedAccount.SiteDescription = UpdAccDescription.ToString();
                                SelectedAccount = SelectedAccount;
                                db.Entry(SelectedAccount).State = EntityState.Modified;
                                db.SaveChangesAsync();
                            }
                        },
                        d => { return SelectedAccount != null; });
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
                if (upd_account_command == null)
                {
                    upd_account_command = new DelegateCommand(
                    d =>
                    {
                        if (_dialogService.ConfirmDialog("Ви точно хочете видалити обраний акаунт?"))
                        {
                            db.Accounts.Remove(SelectedAccount);
                            db.SaveChangesAsync();
                        }
                    },
                    d =>
                    {
                        return SelectedAccount != null;
                    });
                }
                return upd_account_command;
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
                    d =>
                    {
                        string message = SelectedAccount.Encrypted == 1 ? 
                        "Ви точно хочете розшифрувати пароль обраного акаунту?" : 
                        "Ви точно хочете зашифрувати пароль обраного акаунту?";
                        if (_dialogService.ConfirmDialog(message))
                        {
                            SelectedAccount.Crypt(SecretKey);
                            db.Entry(SelectedAccount).State = EntityState.Modified;
                            db.SaveChangesAsync();
                            SelectedAccount = SelectedAccount;
                            SecretKey = "";
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
        #endregion Commands

        #region Methods
        // Оновлення даних у вкладці "Статистика" відбувається через callback у обробнику події зміни вкладки
        public Task UpdateStatistics()
        {
            return Task.Run(() => 
            {
                // Створюємо новий криптографічний об'єкт симетричного шифрування.
                // Він необхідний, щоб отримати випадково згенерований ключ шифрування
                var cipherKey = AesOperation.GetRandomString();
                string crypted_pass = string.Empty;
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
            });
            
        }

        // Оновлення даних у вкладці "Оновлення акаунту" відбувається через callback у обробнику події зміни вкладки
        public void LoadSelectedAccountForUpdate()
        {
            if (SelectedAccount != null)
            {
                UpdAccAddress = SelectedAccount.SiteAddress.ToString();
                UpdAccDescription = SelectedAccount?.SiteDescription.ToString();
                UpdAccLogin = SelectedAccount.Login.ToString();
                UpdAccName = SelectedAccount.SiteName.ToString();
                UpdAccPassword = SelectedAccount.Password.ToString();
            }
        }
        // Видалення акаунту у вкладці "Оновлення акаунту" відбувається через callback у обробнику події натискання на кнопку із підтвердженням
        public void DeleteSelectedAccount()
        {
            db.Accounts.Remove(SelectedAccount);
            db.SaveChanges();

            if (Accounts.Count > 0)
            {
                this.SelectedAccount = Accounts[0];
                LoadSelectedAccountForUpdate();
            }
            else
            {
                UpdAccAddress = string.Empty;
                UpdAccDescription = string.Empty;
                UpdAccLogin = string.Empty;
                UpdAccName = string.Empty;
                UpdAccPassword = string.Empty;
            }

        }
        // Збереження данних відбувається автоматично через callback під час закриття головного вікна застосунку
        public void SaveData(string path)
        {
            var list = this.Accounts.ToList();
            Account.Save(path, list);
        }
        // Завантаження данних відбувається автоматично через callback під час відкриття головного вікна застосунку
        public void LoadData(string path)
        {
            List<Account> list = new List<Account>();
            Account.Load(path, out list);
            this.Accounts = new ObservableCollection<Account>(list);
            this.SearchedAccounts = new ObservableCollection<Account>(list);
        }
        // Шифрування паролю відбувається автоматично через callback у обробнику події натискання на кнопку із підтвердженням
        public void CryptPassword()
        {
            SelectedAccount.Crypt(SecretKey);
            // Переприсвоєння для оновлення даних у текстбоксах
            //var sel = SelectedAccount;
            //SelectedAccount = Accounts[0];
            //SelectedAccount = sel;
            db.Entry(SelectedAccount).State = EntityState.Modified;
            db.SaveChangesAsync();
            SelectedAccount = SelectedAccount;
            SecretKey = "";
        }
        #endregion Methods
    }
}
