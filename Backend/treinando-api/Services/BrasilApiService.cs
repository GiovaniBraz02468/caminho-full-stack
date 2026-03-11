using System.Text.Json;
using treinando_api.Http;
using treinando_api.Models;

namespace treinando_api.Services
{
    public class BrasilApiService
    {
        //Variáveis auxiliares
        private ApiClient _client;

        //Construtores
        public BrasilApiService()
        {
            _client = new ApiClient();
        }

        //Método
        public async Task<Cep?> BuscarCep(string cep)
        {
            var url = $"https://brasilapi.com.br/api/cep/v1/{cep}";
            var json = await _client.GetAsync(url);
            var resultado = JsonSerializer.Deserialize<Cep>(json);
            return resultado;
        }
    }

}