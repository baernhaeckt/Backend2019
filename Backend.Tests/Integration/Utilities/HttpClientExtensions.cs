using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Models;
using Newtonsoft.Json;

namespace Backend.Tests.Integration.Utilities
{
    public static class HttpClientExtensions
    {
        public static async Task SignIn(this HttpClient client, string email, string password)
        {
            var url = new Uri($"api/users/Login?email={email}&password={password}", UriKind.Relative);
            HttpResponseMessage responseWithJwt = await client.PostAsync(url, null);
            responseWithJwt.EnsureSuccessStatusCode();
            string jwt = await responseWithJwt.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(jwt);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);
        }
    }
}