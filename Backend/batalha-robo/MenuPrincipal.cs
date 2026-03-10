namespace batalha_robo
{
    public class MenuPrincipal
    {
        //Métodos
        public static void Apresentacao()
        {
            Console.WriteLine("Seja bem vindo ao RFC! (Luta de Robôs!!!!)");
        }
        public static void Despedida()
        {
            Console.WriteLine("Até uma próxima vez!");
        }
        public static string ProcessaMenu()
        {
            while (true)
            {
                Console.WriteLine("Escolha uma opção!");
                Console.WriteLine("1 - Cadastrar um robô");
                Console.WriteLine($"2 - listar os robôs ({BatalhaRobo.Torneio.Participantes.Count()} cadastrado(s))");
                Console.WriteLine("3 - Editar ou apagar dados de um robô");
                Console.WriteLine("4 - Vamos lutar!");
                Console.WriteLine("5 - Sair do sistema");

                string? opcao = Console.ReadLine() ?? "";

                if (Utils.ValidaOp(opcao))
                {
                    return opcao;
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Opção '{opcao}' inválida! Tente novamente.");
                Console.ResetColor();

            }
        }
        public static void ProcessaOpcao(string opcao)
        {
            switch (opcao)
            {
                case "1":
                    Interface.InterfaceCadastrar();
                    break;
                case "2":
                    Interface.InterfaceListar();
                    break;
                case "3":
                    Interface.InterfaceEditaDeleta();
                    break;
                case "4":
                    Interface.InterfaceBatalha();
                    break;
            }
        }
    }
}