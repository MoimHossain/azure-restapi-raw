

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NameOfProject
{
    public class AzureApi
    {
        private readonly string tenantId;
        private readonly string clientId;
        private readonly string secret;

        private const string RESOURCE = "https://management.azure.com/";
        private const string MEDIATYPE = "application/x-www-form-urlencoded";

        public AzureApi(string tenantId, string clientId, string secret)
        {
            this.tenantId = tenantId;
            this.clientId = clientId;
            this.secret = secret;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var url = $"https://login.microsoftonline.com/{this.tenantId}/oauth2/token";            
            var httpClient = new HttpClient();
            var body = $"grant_type=client_credentials&client_id={clientId}&client_secret={secret}&resource={RESOURCE}";

            var response = await httpClient.PostAsync(url, new StringContent(body, Encoding.UTF8, MEDIATYPE));
            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<Token>(tokenResponse).access_token;
            }

            return string.Empty;
        }

        private class Token
        {
            public string access_token { get; set; }
        }
    }
}
