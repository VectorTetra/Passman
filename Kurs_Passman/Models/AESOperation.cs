using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace Kurs_Passman.Models
{
    public class AesOperation
    {
        // Цей клас надає функціонал для шифрування паролів з використанням
        // симетричного алгоритму блочного шифрування Advanced Encryption Standard (AES)
        // генератор випадкового рядка для шифрування
        public static string GetRandomPasswordString()
        {
            // Пароль,який буде повертати функція
            string passw = "";
            //Рандомайзер
            Random rand = new Random();
            // Створюємо масив із різних типів символів, які ми будемо використовувати
            List<char[]> collections = new()
            {
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(),
                "abcdefghijklmnopqrstuvwxyz!".ToCharArray(),
                "1234567890".ToCharArray(),
                "!@#$%^&*()_+-=".ToCharArray()
            };
            // Додамо по одному символу з кожної колекції, щоб дотриматись умов сильного пароля
            foreach (var item in collections)
            {
                int letter_num = rand.Next(0, item.Length);
                passw += item[letter_num];
            }
            // Додамо ще символи, доки їх не буде 16
            for (int i = 0; i < 12; i++)
            {
                var collec = collections[rand.Next(0, collections.Count)];
                int letter_num = rand.Next(0, collec.Length);
                passw += collec[letter_num];
            }

            return passw;

        }
        public static string GetRandomString(int length = 16)
        {
            // Створюємо масив символів, які ми будемо використовувати
            char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz!@#$%^&*()_+-=".ToCharArray();

            Random rand = new Random();
            
            string word = "";
            for (int j = 1; j <= length; j++)
            {
                int letter_num = rand.Next(0, letters.Length - 1);
                word += letters[letter_num];
            }

            return word;
           
        }
        // генератор повторів пароля
        private static string GetRepeatKey(string s, int n)
        {
            if (s.Length > 16)
            {
                s = s.Substring(0, 16);
            }
            var r = s;
            while (r.Length < n)
            {
                r += r;
            }

            return r.Substring(0, n);
        }
        // Функція шифрування пароля
        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;
            key = GetRepeatKey(key, 16);
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
        // Функція дешифрування пароля
        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);
            key = GetRepeatKey(key, 16);
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
