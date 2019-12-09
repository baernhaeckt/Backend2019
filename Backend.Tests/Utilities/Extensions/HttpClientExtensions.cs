using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Backend.Core.Features.Partner.Models;
using Backend.Core.Features.UserManagement.Models;
using Newtonsoft.Json;

namespace Backend.Tests.Utilities.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task SignIn(this HttpClient client, string email, string password)
        {
            var url = new Uri("api/users/Login", UriKind.Relative);
            StringContent content = new UserLoginRequest { Email = email, Password = password }.ToStringContent();
            HttpResponseMessage responseWithJwt = await client.PostAsync(url, content);
            responseWithJwt.EnsureSuccessStatusCode();
            string jwt = await responseWithJwt.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<UserLoginResponse>(jwt);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);
        }

        public static async Task SignInTokenIssuer(this HttpClient client, Guid id, string secret)
        {
            var url = new Uri("api/tokenIssuers/Login", UriKind.Relative);
            StringContent content = new PartnerLoginRequest { PartnerId = id, Secret = secret }.ToStringContent();
            HttpResponseMessage responseWithJwt = await client.PostAsync(url, content);
            responseWithJwt.EnsureSuccessStatusCode();
            string jwt = await responseWithJwt.Content.ReadAsStringAsync();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        }

        public static async Task<string> CreateUserAndSignIn(this HttpClient client)
        {
            string email = DataGenerator.RandomEmail();
            var url = new Uri("api/users/Register", UriKind.Relative);
            StringContent content = new RegisterUserRequest { Email = email }.ToStringContent();
            HttpResponseMessage response = await client.PostAsync(url, content);
            UserLoginResponse userLoginResponse = await response.OnSuccessDeserialize<UserLoginResponse>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userLoginResponse.Token);
            return email;
        }
    }
}