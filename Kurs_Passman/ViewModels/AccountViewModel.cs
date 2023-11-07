using Kurs_Passman.Models;
using System.Collections.Generic;
using System.Linq;

namespace Kurs_Passman.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        private Account account;

        public AccountViewModel(Account account)
        {
            this.account = account;
        }
        public bool IsEncrypted
        {
            get { return account.Encrypted; }
            set
            {
                account.Encrypted = value;
                OnPropertyChanged(nameof(account.Encrypted));
            }
        }
        public string SiteName
        {
            get { return account.SiteName; }
            set
            {
                account.SiteName = value;
                OnPropertyChanged(nameof(account.SiteName));
            }
        }

        public string SiteAddress
        {
            get { return account.SiteAddress; }
            set
            {
                account.SiteAddress = value;
                OnPropertyChanged(nameof(account.SiteAddress));
            }
        }

        public string SiteDescription
        {
            get { return account.SiteDescription; }
            set
            {
                account.SiteDescription = value;
                OnPropertyChanged(nameof(account.SiteDescription));
            }
        }

        public string Login
        {
            get { return account.Login; }
            set
            {
                account.Login = value;
                OnPropertyChanged(nameof(account.Login));
            }
        }

        public string Password
        {
            get { return account.Password; }
            set
            {
                account.Password = value;
                OnPropertyChanged(nameof(account.Password));
            }
        }

        public static void Save(string path, ref List<AccountViewModel> accountList)
        {
            List<Account> lp = new List<Account>();
            lp = accountList.Select(x => x.account).ToList();
            Account.Save(path, lp);
        }
        public static void Load(string path, ref ICollection<AccountViewModel> accountList)
        {
            List<Account> lp = new List<Account>();
            Account.Load(path, out lp);
            foreach (var item in lp)
            {
                accountList.Add(new AccountViewModel(item));
            }
        }
        public void Crypt(string secretKey)
        {
            this.account.Crypt(secretKey);
        }
    }
}
