using System.Net.Http;

namespace treinando_api.Http
{
    public class ApiClient
    {
        //Variáveis auxiliares
        private HttpClient _http;

        //Construtores
        public ApiClient()
        {
            _http = new HttpClient();
        }

        //Métodos
        public async Task<string> GetAsync(string url)
        {
            var response = await _http.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}