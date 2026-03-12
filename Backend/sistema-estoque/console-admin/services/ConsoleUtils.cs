using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace sistema_estoque.console_admin.services
{
    public class ConsoleUtils
    {
        //Métodos
        public static string LerInputObrigatorio(string campo)
        {
            string entrada;
            do
            {
                Console.Write($"{campo}: ");
                entrada = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(entrada))
                {
                    Console.WriteLine($"{Icones.Alerta}  O campo {campo} é obrigatório!");
                }
            } while (string.IsNullOrEmpty(entrada));
            return entrada;
        }
        public static string LerOpcaoMenu(string[] opcoesValidas)
        {
            while (true)
            {
                Console.WriteLine("Escolha uma opção: ");
                string entrada = Console.ReadLine()?.Trim() ?? "";

                if (opcoesValidas.Contains(entrada))
                {
                    return entrada;
                }

                Console.WriteLine($"{Icones.Erro}  Opção inválida! Tente novamente.");
            }
        }

        public static int LerIntOpcional(string mensagem, int minimo = 0)
        {
            while (true)
            {
                Console.Write($"{mensagem}: ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(input))
                {
                    if (minimo <= 0) return 0;
                    Console.WriteLine($"{Icones.Erro} Erro: Este campo é obrigatório e o valor mínimo é {minimo}.");
                    continue;
                }

                if (int.TryParse(input, out int resultado) && resultado >= minimo)
                {
                    return resultado;
                }

                Console.WriteLine($"{Icones.Erro} Erro: Digite um número válido (mínimo: {minimo}).");
            }
        }

        public static decimal LerDecimalOpcional(string mensagem, decimal minimo = 0)
        {
            while (true)
            {
                Console.Write($"{mensagem}: ");
                string input = (Console.ReadLine() ?? "").Replace(',', '.').Trim();

                if (string.IsNullOrEmpty(input))
                {
                    if (minimo <= 0) return 0;
                    Console.WriteLine($"{Icones.Erro} Erro: Este valor é obrigatório (mínimo: {minimo:C2}).");
                    continue;
                }

                if (decimal.TryParse(input, CultureInfo.InvariantCulture, out decimal resultado) && resultado >= minimo)
                {
                    return resultado;
                }

                Console.WriteLine($"{Icones.Erro} Erro: Digite um valor válido (mínimo: {minimo:C2}).");
            }
        }
    }
}