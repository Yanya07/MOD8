using System;
using System.Data.SqlClient;

namespace WindowsFormsApp1
{
    internal class DataBase // Класс для управления соединением с базой данных
    {
        private readonly string _connectionString = @"Data Source=MONKEY\SQLEXPRESS;Initial Catalog=shifr;Integrated Security=True";
        private SqlConnection _sqlConnection;

        public DataBase()
        {
            _sqlConnection = new SqlConnection(_connectionString); // Инициализация соединения
        }

        public void OpenConnection() // Метод для открытия соединения с базой данных
        {
            if (_sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                _sqlConnection.Open();
            }
        }

        public void CloseConnection() // Метод для закрытия соединения с базой данных
        {
            if (_sqlConnection.State == System.Data.ConnectionState.Open)
            {
                _sqlConnection.Close();
            }
        }

        public SqlConnection GetConnection() // Метод для получения текущего соединения
        {
            return _sqlConnection;
        }
    }
}
