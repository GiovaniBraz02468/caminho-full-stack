using MySqlConnector;
using sistema_estoque.console_admin.menus;
using sistema_estoque.infrastructure.database;

namespace sistema_estoque.console_admin
{
    /// <summary>
    /// Classe principal que serve como ponto de entrada para a aplicação de estoque.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Função principal que inicia a execução do console.
        /// </summary>
        public static void Main()
        {
            var menuInicial = new MenuInicial();
            menuInicial.Exibir();
        }
    }
}