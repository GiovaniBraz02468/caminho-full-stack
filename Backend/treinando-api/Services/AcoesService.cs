using System.Text.Json;
using treinando_api.Http;
using treinando_api.Models;

namespace treinando_api.Services
{
    public class AcoesService
    {
        //Variáveis auxiliares
        private ApiClient _client;

        //Construtores
        public AcoesService()
        {
            _client = new ApiClient();
        }

        //Métodos
        public async Task<Acao?> BuscarCotacao(string ticker)
        {
            var url = $"https://brapi.dev/api/quote/{ticker}";
            var json = await _client.GetAsync(url);

            var resultado = JsonSerializer.Deserialize<AcaoResponse>(json);

            return resultado?.results?.FirstOrDefault();
        }
    }
}