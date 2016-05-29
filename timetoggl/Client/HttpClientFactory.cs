using System.Net.Http;
using System.Security;
using TimeToggl.Helpers;
using TimeToggl.Security;
using TimeToggl.Extensions;

namespace TimeToggl.Client
{
    public static class HttpClientFactory
    {
        public static HttpClient GetClient(string param1, SecureString param2)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", GenerateAuthHeader(param1, param2));

            return client;
        }

        public static HttpClient GetClient(string param1, string param2)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", GenerateAuthHeader(param1, param2));

            return client;
        }

        private static string GenerateAuthHeader(string user, SecureString pass)
        {
            string combined = ($"{user}:{(pass.ToNonSecureString())}").Base64Encode();
            return $"Basic {combined}";
        }

        private static string GenerateAuthHeader(string user, string pass)
        {
            string combined = ($"{user}:{pass}").Base64Encode();
            return $"Basic {combined}";
        }
    }
}
