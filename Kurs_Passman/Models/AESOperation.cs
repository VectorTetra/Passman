using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kurs_Passman.Models
{
    public class AesOperation
    {
        //генератор повторів пароля, в будь-якому випадку повертає 16-символьний ключ
        //private static string GetRepeatKey(string s, int n)
        //{
        //    if (s.Length > 16)
        //    {
        //        s = s.Substring(0, 16);
        //    }
        //    var r = s;
        //    while (r.Length < n)
        //    {
        //        r += r;
        //    }

        //    return r.Substring(0, n);
        //}


        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;
            //key = GetRepeatKey(key, 16);
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

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);
            //key = GetRepeatKey(key, 16);
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
