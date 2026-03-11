using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace treinando_api.Utils
{
    public class Utils
    {
        //Métodos
        public static bool OpcaoValida(string opDigitada, params string[] opcoesValidas)
        {
            if (string.IsNullOrWhiteSpace(opDigitada))
                return false;

            return opcoesValidas != null && System.Array.Exists(opcoesValidas, o => o.Equals(opDigitada.Trim(), System.StringComparison.OrdinalIgnoreCase));
        }
        public static string TratarErroApi(Exception ex)
        {
            if (ex is HttpRequestException httpEx)
            {
                return httpEx.StatusCode switch
                {
                    System.Net.HttpStatusCode.BadRequest => "Verifique o valor enviado! O formato está incorreto.",
                    System.Net.HttpStatusCode.NotFound => "Valor não encontrado na base de dados.",
                    System.Net.HttpStatusCode.InternalServerError => "O servidor da API está instável. Tente mais tarde.",
                    _ => "Erro ao processar a requisição. Tente novamente."
                };
            }

            return "Erro de conexão! Verifique se você está online.";
        }
    }
}