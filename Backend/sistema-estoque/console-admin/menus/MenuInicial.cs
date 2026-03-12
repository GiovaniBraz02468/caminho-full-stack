using sistema_estoque.console_admin.services;

namespace sistema_estoque.console_admin.menus
{
    /// <summary>
    /// Classe responsável por gerenciar o menu principal do sistema
    /// </summary>
    public class MenuInicial
    {
        //Métodos

        /// <summary>
        /// Função responsável por exibir o menu principal e direcionar para a opção escolhida
        /// </summary>
        public void Exibir()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"=== {Icones.Home} SISTEMA DE ESTOQUE ===");
                Console.WriteLine($"1 - {Icones.Admin}  Sistema Administrativo (Console)");
                Console.WriteLine($"2 - {Icones.API} Ligar API (Em breve)");
                Console.WriteLine($"0 - {Icones.Sair} Sair");

                Console.WriteLine("Escolha uma opção: ");
                var opcao = Console.ReadLine();

                if (opcao == "1")
                {
                    new MenuLogin().Exibir();
                }
                else if (opcao == "0")
                {
                    Console.WriteLine($"{Icones.Sair} Até uma próxima!");
                    break;
                }
            }
        }
    }
}