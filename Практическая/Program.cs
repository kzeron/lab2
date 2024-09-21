using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
   
    static readonly byte[] Key = Convert.FromBase64String("Tct0+4C7NzCjP1BajO+3eA==");

    static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567890abcdef");

    static void Main(string[] args)
    {

        string plainText = "Ку";

        // Шифруем текст
        string cipherText = Encrypt(plainText);
        // Сохраняем зашифрованный текст в input.txt
        File.WriteAllText("input.txt", cipherText);
        Console.WriteLine("Зашифрованное сообщение сохранено в input.txt");

        try
        {
            // Чтение зашифрованного сообщения из файла input.txt
            cipherText = File.ReadAllText("input.txt");

            // Дешифрование сообщения
            string decryptedMessage = Decrypt(cipherText);

            // Запись расшифрованного сообщения в output.txt
            File.WriteAllText("output.txt", decryptedMessage);
            Console.WriteLine("Дешифрованное сообщение записано в output.txt");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Произошла ошибка: " + ex.Message);
        }
    }

    static string Encrypt(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }
    }

    static string Decrypt(string cipherText)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            using (MemoryStream memoryStream = new MemoryStream(cipherBytes))
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
            using (StreamReader decryptReader = new StreamReader(cryptoStream))
            {
                return decryptReader.ReadToEnd();
            }
        }
    }
}
