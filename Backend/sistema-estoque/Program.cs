using MySqlConnector;
using sistema_estoque.infrastructure.database;

Console.WriteLine("Testando conexão com MySQL...");
var db = new DatabaseConnection();

try
{
    using var connection = db.GetConnection();
    connection.Open();
    Console.WriteLine("Conectado ao MySQL com sucesso!");
}
catch (Exception ex)
{
    Console.WriteLine("Erro ao conectar:");
    Console.WriteLine(ex.Message);
}

Console.ReadLine();