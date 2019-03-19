using System;

namespace Checkout.Utils
{
  public static class Extensions
  {
    public static T New<T>(this IServiceProvider serviceProvider)
    {
      return (T) serviceProvider.GetService(typeof(T));
    }
  }
}
