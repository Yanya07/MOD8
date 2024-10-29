using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private PasswordManager _passwordManager;

        public Form1()
        {
            InitializeComponent();
            _passwordManager = new PasswordManager("MyVeryStrongEncryptionKey!123"); // Инициализация менеджера паролей с ключом шифрования
        }

        private void AddPasswordButton_Click(object sender, EventArgs e) // Обработчик события для добавления пароля
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) // Проверка на пустое поле логина или пароля
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.");
                return;
            }

            _passwordManager.AddPassword(username, password); // Добавление пароля в базу данных
            MessageBox.Show("Пароль сохранён.");
            ClearTextBoxes(); // Очистка полей ввода
            UpdatePasswordList(); // Обновление списка паролей
        }

        private void ShowPasswordsButton_Click(object sender, EventArgs e) // Обработчик события для отображения всех паролей
        {
            string masterPassword = PromptForMasterPassword(); // Запрос мастер-пароля
            if (masterPassword != "YourMasterPassword") // Замените YourMasterPassword на ваш мастер-пароль
            {
                MessageBox.Show("Неверный мастер-пароль.");
                return;
            }

            var passwords = _passwordManager.GetPasswords();
            passwordsListBox.Items.Clear();

            foreach (var entry in passwords)
            {
                passwordsListBox.Items.Add($"Логин: {entry.Key}, Пароль: {entry.Value}");
            }
        }

        private void DeletePasswordButton_Click(object sender, EventArgs e) // Обработчик события для удаления пароля по логину
        {
            string username = usernameTextBox.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Введите логин для удаления.");
                return;
            }

            if (_passwordManager.DeletePassword(username)) // Удаление пароля
            {
                MessageBox.Show("Пароль успешно удалён.");
            }
            else
            {
                MessageBox.Show("Логин не найден.");
            }
            ClearTextBoxes(); // Очистка полей ввода
            UpdatePasswordList(); // Обновление списка паролей
        }

        private void UpdatePasswordList() // Метод для обновления списка паролей
        {
            var passwords = _passwordManager.GetPasswords();
            passwordsListBox.Items.Clear();

            foreach (var entry in passwords)
            {
                passwordsListBox.Items.Add($"Логин: {entry.Key}, Пароль: {entry.Value}");
            }
        }

        private void ClearTextBoxes() // Метод для очистки полей ввода логина и пароля
        {
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";
        }

        private string PromptForMasterPassword() // Метод для запроса мастер-пароля
        {
            using (var form = new Form())
            {
                form.Text = "Введите мастер-пароль";
                var textBox = new TextBox() { PasswordChar = '*', Dock = DockStyle.Fill };
                var buttonOk = new Button() { Text = "OK", Dock = DockStyle.Bottom };
                buttonOk.Click += (sender, e) => { form.DialogResult = DialogResult.OK; form.Close(); };
                form.Controls.Add(textBox);
                form.Controls.Add(buttonOk);
                form.AcceptButton = buttonOk;

                return form.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty; // Возвращает введенный мастер-пароль
            }
        }
    }

    public class PasswordManager
    {
        private readonly string _encryptionKey;
        private readonly DataBase _database;

        public PasswordManager(string encryptionKey)
        {
            _encryptionKey = encryptionKey;
            _database = new DataBase();
        }

        public void AddPassword(string username, string password) // Метод для добавления пароля
        {
            string encryptedPassword = Encrypt(password, _encryptionKey);

            try
            {
                using (var connection = _database.GetConnection())
                {
                    _database.OpenConnection(); // Открываем соединение перед выполнением команд

                    string query = "INSERT INTO Passwords (Username, Password) VALUES (@username, @password)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", encryptedPassword);

                    command.ExecuteNonQuery(); // Выполнение команды
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении пароля: {ex.Message}");
            }
            finally
            {
                _database.CloseConnection(); // Закрываем соединение
            }
        }
        public bool DeletePassword(string username) // Метод для удаления пароля
        {
            using (var connection = _database.GetConnection())
            {
                _database.OpenConnection(); // Открываем соединение перед выполнением команд

                string query = "DELETE FROM Passwords WHERE Username = @username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);

                int rowsAffected = command.ExecuteNonQuery(); // Выполнение команды
                return rowsAffected > 0; // Возвращает true, если запись удалена
            }
        }

        public Dictionary<string, string> GetPasswords() // Метод для получения всех паролей
        {
            var passwords = new Dictionary<string, string>(); // Словарь для хранения логинов и паролей

            using (var connection = _database.GetConnection())
            {
                _database.OpenConnection(); // Открываем соединение перед выполнением команд

                string query = "SELECT Username, Password FROM Passwords"; // SQL-запрос
                SqlCommand command = new SqlCommand(query, connection);

                using (SqlDataReader reader = command.ExecuteReader()) // Чтение данных
                {
                    while (reader.Read())
                    {
                        string username = reader["Username"].ToString(); // Чтение логина
                        string encryptedPassword = reader["Password"].ToString(); // Чтение зашифрованного пароля
                        string decryptedPassword = Decrypt(encryptedPassword, _encryptionKey); // Дешифрование пароля
                        passwords.Add(username, decryptedPassword); // Добавление в словарь
                    }
                }
            }

            return passwords; // Возвращает словарь паролей
        }

        private string Encrypt(string text, string key) // Метод для шифрования текста с использованием AES
        {
            using (var aes = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes("SaltIsGoodForSecurity")); // Генерация ключа и вектора инициализации
                aes.Key = pdb.GetBytes(32);
                aes.IV = pdb.GetBytes(16);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        byte[] inputBytes = Encoding.UTF8.GetBytes(text);
                        cs.Write(inputBytes, 0, inputBytes.Length);
                        cs.Close();
                    }
                    return Convert.ToBase64String(ms.ToArray()); // Возвращает зашифрованный текст в формате base64
                }
            }
        }

        private string Decrypt(string cipherText, string key) // Метод для расшифровки текста, зашифрованного методом Encrypt
        {
            using (var aes = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes("SaltIsGoodForSecurity"));
                aes.Key = pdb.GetBytes(32);
                aes.IV = pdb.GetBytes(16);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipherText);
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    return Encoding.UTF8.GetString(ms.ToArray()); // Возвращает расшифрованный текст
                }
            }
        }
    }
}
