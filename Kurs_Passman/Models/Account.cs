using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Windows;
using Microsoft.Data.Sqlite;
namespace Kurs_Passman.Models
{
    [Serializable]
    [DataContract]

    // Клас Account - зберігає інформацію про сайт та його акаунт з логіном та паролем
    public class Account
    {
        public int Id { get; set; }
        
        public int Encrypted { get; set; } = 0;             // Демонструє, чи зашифрований пароль
        
        public string SiteAddress { get; set; }             // Адреса сайту
        
        public string SiteName { get; set; }                // Назва сайту
       
        public string? SiteDescription { get; set; }        // Опис сайту
       
        public string Login { get; set; }                   // Логін акаунту на сайті
       
        public string Password { get; set; }                // Пароль акаунту на сайті

        
        // використовується під час шифрування паролю за бажанням користувача
        public void Crypt(string secretKey)
        {
            if (Encrypted == 1)
            {
                Password = AesOperation.DecryptString(secretKey, Password);
                Encrypted = 0;
            }
            else
            {
                Password = AesOperation.EncryptString(secretKey, Password);
                Encrypted = 1;
            }
        }
    }
}
