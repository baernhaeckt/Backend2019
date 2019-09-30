using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Backend.Tests.Integration.Utilities
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<TObject> OnSuccessDeserialize<TObject>(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<TObject>(await response.Content.ReadAsStringAsync());
        }

        public static void EnsureNotSuccessStatusCode(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Status code successful. " + response.StatusCode);
            }
        }
    }
}