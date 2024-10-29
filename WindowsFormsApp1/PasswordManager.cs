using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class PasswordManager
{
    private string _encryptionKey; // Ключ для шифрования паролей
    private string _connectionString = @"Data Source=MONKEY\SQLEXPRESS;Initial Catalog=shifr;Integrated Security=True"; // Строка подключения к базе данных

    // Конструктор класса, принимает ключ шифрования
    public PasswordManager(string encryptionKey)
    {
        _encryptionKey = encryptionKey; // Инициализация ключа шифрования
    }

    // Метод для добавления нового пароля
    public void AddPassword(string username, string password)
    {
        // Шифруем пароль перед сохранением
        string encryptedPassword = Encrypt(password, _encryptionKey);

        // Создание соединения с базой данных
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            // SQL-запрос для вставки новых данных
            string query = "INSERT INTO Passwords (Username, Password) VALUES (@username, @password)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@username", username); // Добавление параметра логина
            command.Parameters.AddWithValue("@password", encryptedPassword); // Добавление параметра зашифрованного пароля

            // Открываем соединение и выполняем команду
            connection.Open();
            command.ExecuteNonQuery(); // Выполнение SQL-запроса
        }
    }

    // Метод для получения всех паролей
    public Dictionary<string, string> GetPasswords()
    {
        var decryptedPasswords = new Dictionary<string, string>(); // Словарь для хранения расшифрованных паролей

        // Создание соединения с базой данных
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            // SQL-запрос для выборки данных
            string query = "SELECT Username, Password FROM Passwords";
            SqlCommand command = new SqlCommand(query, connection);

            // Открываем соединение и выполняем запрос
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader()) // Читаем данные из базы
            {
                while (reader.Read()) // Перебираем результаты
                {
                    string username = reader["Username"].ToString(); // Получаем логин
                    string encryptedPassword = reader["Password"].ToString(); // Получаем зашифрованный пароль
                    string decryptedPassword = Decrypt(encryptedPassword, _encryptionKey); // Расшифровываем пароль
                    decryptedPasswords[username] = decryptedPassword; // Сохраняем в словарь
                }
            }
        }
        return decryptedPasswords; // Возвращаем словарь с паролями
    }

    // Метод для удаления пароля по логину
    public bool DeletePassword(string username)
    {
        // Создание соединения с базой данных
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            // SQL-запрос для удаления данных
            string query = "DELETE FROM Passwords WHERE Username = @username";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@username", username); // Добавляем параметр логина

            // Открываем соединение и выполняем команду
            connection.Open();
            int rowsAffected = command.ExecuteNonQuery(); // Выполнение SQL-запроса
            return rowsAffected > 0; // Возвращаем true, если запись удалена
        }
    }

    // Метод для шифрования текста с использованием алгоритма AES
    private string Encrypt(string text, string key)
    {
        using (var aes = Aes.Create()) // Создание AES-алгоритма
        {
            // Генерация ключа и вектора инициализации
            var pdb = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes("SaltIsGoodForSecurity"));
            aes.Key = pdb.GetBytes(32); // Генерация ключа
            aes.IV = pdb.GetBytes(16); // Генерация вектора инициализации

            using (var ms = new MemoryStream()) // Память для хранения зашифрованного текста
            {
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write)) // Создание потока для шифрования
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(text); // Преобразование текста в байты
                    cs.Write(inputBytes, 0, inputBytes.Length); // Запись байтов в поток
                    cs.Close(); // Закрываем поток
                }
                return Convert.ToBase64String(ms.ToArray()); // Возвращаем зашифрованный текст в формате Base64
            }
        }
    }

    // Метод для расшифровки текста
    private string Decrypt(string cipherText, string key)
    {
        using (var aes = Aes.Create()) // Создание AES-алгоритма
        {
            // Генерация ключа и вектора инициализации
            var pdb = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes("SaltIsGoodForSecurity"));
            aes.Key = pdb.GetBytes(32); // Генерация ключа
            aes.IV = pdb.GetBytes(16); // Генерация вектора инициализации

            using (var ms = new MemoryStream()) // Память для хранения расшифрованного текста
            {
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write)) // Создание потока для расшифровки
                {
                    byte[] cipherBytes = Convert.FromBase64String(cipherText); // Преобразование зашифрованного текста в байты
                    cs.Write(cipherBytes, 0, cipherBytes.Length); // Запись байтов в поток
                    cs.Close(); // Закрываем поток
                }
                return Encoding.UTF8.GetString(ms.ToArray()); // Возвращаем расшифрованный текст
            }
        }
    }
}
