using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySqlConnector;

namespace sistema_estoque.infrastructure.database
{
    /// <summary>
    /// Classe responsável por realizar a conexão do MySql utilizando uma string local
    /// </summary>
    public class DatabaseConnection
    {
        //Propriedades
        private readonly string connectionString = "Server=localhost;Port=3306;Database=sistema_estoque;User ID=root;Password=Souolider2020;"; public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}