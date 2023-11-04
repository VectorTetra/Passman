using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs_Passman.Models
{
    // Цей клас використовується для шифрування/дешифрування паролів із вказаним ключем
    // Оскільки методи цього класу є статичними, створювати об'єкт класу не потрібно
    public class XORCipher
    {
        //генератор повторів пароля
        private static string GetRepeatKey(string s, int n)
        {
            var r = s;
            while (r.Length < n)
            {
                r += r;
            }

            return r.Substring(0, n);
        }

        //метод шифрування/дешифрування
        private static string Cipher(string text, string secretKey)
        {
            var currentKey = GetRepeatKey(secretKey, text.Length);
            var res = string.Empty;
            for (var i = 0; i < text.Length; i++)
            {
                res += ((char)(text[i] ^ currentKey[i])).ToString();
            }

            return res;
        }

        //шифрування тексту
        public static string Encrypt(string plainText, string password)
            => Cipher(plainText, password);

        //дешифрування тексту
        public static string Decrypt(string encryptedText, string password)
            => Cipher(encryptedText, password);
    }
}
