using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace ClientAPP
{
    class Program
    {
        static void Main(string[] args)
        {
            CallAPIAsyncUsingPassword();
            Console.ReadLine();
        }

        static async void CallAPIAsync()
        {
            var discoveryClient = new DiscoveryClient("http://localhost:65404");
            var disco = await discoveryClient.GetAsync();
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = await client.GetAsync("http://localhost:58436/identity");
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

        static async void CallAPIAsyncUsingPassword()
        {
            var discoveryClient = new DiscoveryClient("http://localhost:65404");
            var disco = await discoveryClient.GetAsync();
            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "api1");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");
        }
    }
}