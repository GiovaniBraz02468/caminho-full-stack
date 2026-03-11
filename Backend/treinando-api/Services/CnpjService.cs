using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using treinando_api.Http;
using treinando_api.Models;

namespace treinando_api.Services
{
    public class CnpjService
    {
        //Variáveis auxiliares
        private ApiClient _client;

        //Construtores
        public CnpjService()
        {
            _client = new ApiClient();
        }

        //Métodos
        public async Task<Empresa?> BuscarCnpj(string cnpj)
        {
            cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            var url = $"https://receitaws.com.br/v1/cnpj/{cnpj}";
            var json = await _client.GetAsync(url);
            return JsonSerializer.Deserialize<Empresa>(json);
        }
    }
}