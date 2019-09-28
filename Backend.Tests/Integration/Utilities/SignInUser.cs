using Backend.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Backend.Tests.Integration.Utilities
{
    public static class HttpClientExtensions
    {
        public static async Task SignIn(this HttpClient client, string email, string password)
        {
            var url = $"api/users/Login?email={email}&password={password}";
            var responseWithJwt = await client.PostAsync(url, null);
            responseWithJwt.EnsureSuccessStatusCode();
            string jwt = await responseWithJwt.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(jwt);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);
        }
    }
}
