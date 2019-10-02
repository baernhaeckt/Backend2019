using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Backend.Tests.Integration.Utilities.Extensions
{
    public static class ObjectExtensions
    {
        public static StringContent ToStringContent(this object parameter)
        {
            return new StringContent(JsonConvert.SerializeObject(parameter), Encoding.UTF8, "application/json");
        }
    }
}