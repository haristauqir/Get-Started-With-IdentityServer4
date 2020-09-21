using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace Client
{
    public static class Client
    {
        public static async Task TestIdentityServer()
        {
            try
            {
                var client = new HttpClient();

                //** get discovery document
                var disco = GetDiscoveryDocument(client);

                //** get token
                var tokenResponse = GetToken(client, disco.Result).Result;
                
                if (!string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
                {
                    // call api
                    var apiClient = new HttpClient();
                    apiClient.SetBearerToken(tokenResponse.AccessToken);

                    var response = await apiClient.GetAsync("https://localhost:6001/identity");
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(response.StatusCode);
                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(JArray.Parse(content));
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Finishing....");
        }


        public static async Task<DiscoveryDocumentResponse> GetDiscoveryDocument(HttpClient client)
        {
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }
            return disco;
        }   

        public static async Task<TokenResponse> GetToken(HttpClient client, DiscoveryDocumentResponse disco)
        {
            // request token
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api2"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return tokenResponse;
            }

            Console.WriteLine(tokenResponse.Json);
            return tokenResponse;
        }
    }
}