using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Checkout.WebTools
{
  public static class WebTools
  {
    /// <summary>
    /// Extension method for HttpClient to make GET requests that deserialize the response into the desired type
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response JSON into</typeparam>
    /// <param name="http">The instance of HttpClient on which the method is invoked</param>
    /// <param name="requestUri">The path from the base address to which the request should be routed</param>
    /// <returns>An instance of the given type T initialized with the response data</returns>
    public static async Task<T> Get<T>(this HttpClient http, string requestUri)
    {
      var result = await http.GetStringAsync(requestUri);
      
      return JsonConvert.DeserializeObject<T>(result);
    }

    /// <summary>
    /// Extension method for HttpClient to make PUT requests that serialize the request body as JSON
    /// </summary>
    /// <param name="http">The instance of HttpClient on which the method is invoked</param>
    /// <param name="requestUri">The path from the base address to which the request should be routed</param>
    /// <param name="requestBody">The object to be serialized as JSON</param>
    public static async Task Put(this HttpClient http, string requestUri, object requestBody = null)
    {
      var body = JsonConvert.SerializeObject(requestBody);
      var content = new StringContent(body, Encoding.UTF8, "application/json");

      await http.PutAsync(requestUri, content);
    }

    /// <summary>
    /// Extension method for HttpClient to make DELETE requests that serialize the request body as JSON
    /// </summary>
    /// <param name="http">The instance of HttpClient on which the method is invoked</param>
    /// <param name="requestUri">The path from the base address to which the request should be routed</param>
    /// <param name="requestBody">The object to be serialized as JSON</param>
    public static async Task Delete(this HttpClient http, string requestUri, object requestBody = null)
    {
      var body = JsonConvert.SerializeObject(requestBody);
      var content = new StringContent(body, Encoding.UTF8, "application/json");

      var request = new HttpRequestMessage(HttpMethod.Delete, requestUri)
      {
        Content = content
      };

      await http.SendAsync(request);
    }
  }
}
