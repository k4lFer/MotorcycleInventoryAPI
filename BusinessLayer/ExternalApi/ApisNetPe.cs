using System.Net;
using System.Net.Http.Headers;

namespace BusinessLayer.ExternalApi
{
    public class ApisNetPe
    {
        public ApisNetPe() { }
        private readonly IHttpClientFactory _httpClientFactory;

        public ApisNetPe(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

 public async Task<(bool Success, string ResponseBody, string ErrorMessage)> CheckDniAsync(string dni)
        {
            if (string.IsNullOrEmpty(dni))
            {
                return (false, null, "El número de DNI no puede estar vacío.");
            }

            string token = "apis-token-12155.EN9nPNJ7jkSweBbr8Bv0C7cVyBzienRm";
            string url = $"https://api.apis.net.pe/v2/reniec/dni?numero={dni}";

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Add("Referer", "https://apis.net.pe/consulta-dni-api");

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return (true, responseBody, null);
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.UnprocessableEntity: // 422
                            return (false, null, "El formato del DNI es inválido. La longitud debe ser igual a 8 y solo debe contener números.");

                        case HttpStatusCode.NotFound: // 404
                            return (false, null, "El número de DNI que buscas no existe.");

                        default:
                            return (false, null, $"Error en la consulta del DNI: {response.StatusCode}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return (false, null, $"Error de conexión: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, null, $"Error inesperado: {ex.Message}");
            }
        }
        
        public async Task<string> CheckRucAsync(string ruc)
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
    }
}