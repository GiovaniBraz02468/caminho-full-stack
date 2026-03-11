using System.Text.Json;
using treinando_api.Http;
using treinando_api.Models;

namespace treinando_api.Services
{
    public class IbgeService
    {
        //Variáveis auxiliares
        private ApiClient _client;

        //Construtores
        public IbgeService()
        {
            _client = new ApiClient();
        }

        //Métodos
        public async Task<List<Estado>?> BuscarEstados()
        {
            var url = "https://servicodados.ibge.gov.br/api/v1/localidades/estados";
            var json = await _client.GetAsync(url);
            return JsonSerializer.Deserialize<List<Estado>>(json);
        }
    }
}