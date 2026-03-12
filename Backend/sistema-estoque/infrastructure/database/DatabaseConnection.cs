using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;

namespace sistema_estoque.infrastructure.database
{
    public class DatabaseConnection
    {
        private readonly string connectionString = "Server=localhost;Port=3306;Database=sistema_estoque;User ID=root;Password=Souolider2020;"; public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}