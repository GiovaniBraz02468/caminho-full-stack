using treinando_api.Services;

namespace treinando_api.Interface
{
    public class Menuconsole
    {
        //Variáveis auxiliares
        private BrasilApiService _api;

        //Construtores
        public Menuconsole()
        {
            _api = new BrasilApiService();
        }

        //Métodos
        public async Task Iniciar()
        {
            Console.WriteLine("Seja bem-vindo(a) ao sistema de APi's brasileiras!");
            while (true)
            {
                Console.WriteLine("Escolha uma opção");
                Console.WriteLine("1 - Buscar CEP");
                Console.WriteLine("2 - Cotação de ações");
                Console.WriteLine("3 - IBGE (Listar estados)");
                Console.WriteLine("4 - Consulta Razão social");
                Console.WriteLine("5 - Sair do sistema");

                string opcao = Console.ReadLine() ?? "";

                if (Utils.Utils.OpcaoValida(opcao, "1", "2", "3", "4", "5"))
                {
                    if (opcao == "5")
                    {
                        Console.WriteLine("Até uma próxima vez!");
                        return;
                    }
                    else
                    {
                        ExecutaInterfaces executador = new ExecutaInterfaces();
                        await executador.VerificaOp(opcao);
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Opção {opcao} inválida, tente novamente!");
                }
            }
        }
    }
}