using System;

namespace Checkout.Utils
{
  public static class Extensions
  {
    /// <summary>
    /// Extension method to retrieve an instance of a service registered in the service provider by its type
    /// </summary>
    /// <typeparam name="T">The type of the service</typeparam>
    /// <param name="serviceProvider">The instance of IServiceProvider on which this method is being invoked</param>
    /// <returns>An instance of the requested service</returns>
    public static T New<T>(this IServiceProvider serviceProvider)
    {
      return (T) serviceProvider.GetService(typeof(T));
    }
  }
}
