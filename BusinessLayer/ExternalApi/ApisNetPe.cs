using System.Net.Http.Headers;

namespace BusinessLayer.ExternalApi
{
    public class ApisNetPe
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApisNetPe(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> CheckDniAsync(string dni)
        {
            try
            {
                string token = "apis-token-12155.EN9nPNJ7jkSweBbr8Bv0C7cVyBzienRm";
                string url = $"https://api.apis.net.pe/v2/reniec/dni?numero={dni}";
                
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("Referer", "https://apis.net.pe/consulta-dni-api");

                HttpResponseMessage response = await client.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch
            {
                return null;
            }
        }
        
        public async Task<string> CheckRucAsync(string ruc)
        {
            try
            {
                string token = "apis-token-12155.EN9nPNJ7jkSweBbr8Bv0C7cVyBzienRm";
                string url = $"https://api.apis.net.pe/v2/sunat/ruc?numero={ruc}";
                
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("Referer", "http://apis.net.pe/api-ruc");

                HttpResponseMessage response = await client.GetAsync(url);
                
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch
            {
                return null;
            }
        }
    }
}