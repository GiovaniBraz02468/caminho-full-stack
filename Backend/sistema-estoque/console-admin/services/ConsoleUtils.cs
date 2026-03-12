using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace sistema_estoque.console_admin.services
{
    /// <summary>
    /// Classe responsável por realizar validações genéricas em todo o sistema
    /// </summary>
    public class ConsoleUtils
    {
        //Métodos

        /// <summary>
        /// Função responsável por validar um campo obrigatório e obrigar o uusário a digitar corretamente
        /// </summary>
        /// <param name="campo">Nome do campo para retornar uma descrição mais clara para o cliente</param>
        /// <returns>Uma string contendo o valor digitado pelo usuário já valiidado</returns>
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

        /// <summary>
        /// Função genérica responsável por validar opções digitadas pelo cliente
        /// </summary>
        /// <param name="opcoesValidas">Um array contendo as opções válidas</param>
        /// <returns>Uma string contendo o valor digitado pelo usuário, validade para que seja uma das opções do opcoesValidas</returns>
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

        /// <summary>
        /// Função que realiza a leitura e validação para que seja um valor inteiro
        /// </summary>
        /// <param name="mensagem">Mensagem do sistema para o cliente, para deixar claro a entrada de dados</param>
        /// <param name="minimo">Valor mínimo aceitado pelo sistema, se for maior que 0, ele pede novamente o valor e fala que precisa ser maior que o valor mínimo</param>
        /// <returns>Um valor int já validado para utilizar no sistema</returns>
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

        /// <summary>
        /// Função que realiza a leitura e validação para que seja um valor decimal / inteiro
        /// </summary>
        /// <param name="mensagem">Mensagem do sistema para o cliente, para deixar claro a entrada de dados</param>
        /// <param name="minimo">Valor mínimo aceitado pelo sistema, se for maior que 0, ele pede novamente o valor e fala que precisa ser maior que o valor mínimo</param>
        /// <returns>Um valor decimal validado para utilizar no sistema</returns>
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