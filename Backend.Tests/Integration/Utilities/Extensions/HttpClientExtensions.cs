using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Backend.Core.Features.UserManagement.Models;
using Newtonsoft.Json;

namespace Backend.Tests.Integration.Utilities.Extensions
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

        public static async Task SignInTokenIssuer(this HttpClient client, Guid id, string secret)
        {
            var url = new Uri($"api/tokenIssuers/Login?id={id}&secret={secret}", UriKind.Relative);
            HttpResponseMessage responseWithJwt = await client.PostAsync(url, null);
            responseWithJwt.EnsureSuccessStatusCode();
            string jwt = await responseWithJwt.Content.ReadAsStringAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        public static async Task<string> CreateUserAndSignIn(this HttpClient client)
        {
            string email = DataGenerator.RandomEmail();
            var url = new Uri("api/users/Register?email=" + email, UriKind.Relative);
            HttpResponseMessage response = await client.PostAsync(url, null);
            LoginResponse loginResponse = await response.OnSuccessDeserialize<LoginResponse>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);
            return email;
        }
    }
}