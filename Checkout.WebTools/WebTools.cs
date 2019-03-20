using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.WebTools
{
  public static class WebTools
  {
    public static async Task<T> Get<T>(this HttpClient http, string requestUri)
    {
      var result = await http.GetStringAsync(requestUri);

      return JsonConvert.DeserializeObject<T>(result);
    }

    public static async Task Put(this HttpClient http, string requestUri, object requestBody = null)
    {
      var body = JsonConvert.SerializeObject(requestBody);
      var content = new StringContent(body, Encoding.UTF8, "application/json");

      await http.PutAsync(requestUri, content);
    }
  }
}
