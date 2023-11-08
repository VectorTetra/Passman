﻿using System;
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
        [DataMember]
        public bool Encrypted = false;                     // Демонструє, чи зашифрований пароль
        [DataMember]
        public string SiteAddress { get; set; }             // Адреса сайту
        [DataMember]
        public string SiteName { get; set; }                // Назва сайту
        [DataMember]
        public string? SiteDescription { get; set; }        // Опис сайту
        [DataMember]
        public string Login { get; set; }                   // Логін акаунту на сайті
        [DataMember]
        public string Password { get; set; }                // Пароль акаунту на сайті

        // статичний метод завантаження масиву акаунтів із файлу шляхом десеріалізації
        public static void Load(string path, out List<Account> accountList)
        {
            DataContractJsonSerializer json21 = new DataContractJsonSerializer(typeof(List<Account>));
            FileStream fstr = new FileStream(path, FileMode.Open);
            accountList = new();
            if (fstr.Length > 0)
            {
                accountList = (List<Account>)json21.ReadObject(fstr);
                foreach (var item in accountList)
                {
                    //item.Password = Encoding.Unicode.GetString(Encoding.Convert(Encoding.UTF32, Encoding.Unicode, Encoding.UTF32.GetBytes(item.Password)));
                    //item.Crypt("ioP7PtH7R3zuq7A");
                    if (!item.Encrypted)
                    {
                        item.Password = AesOperation.DecryptString("vJIE9TwyIZcy8nbYTm7J9fHCqFvRL1fa", item.Password);
                    }

                }
            }

            fstr.Close();
            
        }
        // статичний метод запису масиву акаунтів у файл шляхом серіалізації
        public static void Save(string path, List<Account> accountList)
        {
            foreach (var item in accountList)
            {
                if (!item.Encrypted)
                {
                    item.Password = AesOperation.EncryptString("vJIE9TwyIZcy8nbYTm7J9fHCqFvRL1fa", item.Password);
                }

                //item.Crypt("ioP7PtH7R3zuq7A");
                //item.Password = Encoding.UTF32.GetString( Encoding.Convert(Encoding.Unicode, Encoding.UTF32, Encoding.Unicode.GetBytes(item.Password)));
            }
           

            DataContractJsonSerializer jsser = new DataContractJsonSerializer(typeof(List<Account>));
            FileStream fstr = new FileStream(path, FileMode.OpenOrCreate);
            jsser.WriteObject(fstr, accountList);
            fstr.Close();
        }
        // статичний метод шифрування / дешифрування паролю для об'єкту класу Account
        // використовується у методах Save / Load , а також під час шифрування паролю за бажанням користувача
        public void Crypt(string secretKey)
        {
            if (Encrypted)
            {
                Password = AesOperation.DecryptString(secretKey, Password);
                Encrypted = !Encrypted;
            }
            else
            {
                Password = AesOperation.EncryptString(secretKey, Password);
                Encrypted = !Encrypted;
            }

        }
    }
}
