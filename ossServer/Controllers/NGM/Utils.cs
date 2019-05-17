using System;

namespace ossServer.Controllers.NGM
{
  public static class Utils
  {
    public static void ExceptionIfEmpty(string adatNeve, string value)
    {
      if (string.IsNullOrEmpty(value))
        throw new Exception($"Az {adatNeve} nem lehet üres.");
    }

    public static void ExceptionIfEmpty(string adatNeve, DateTime? value)
    {
      if (value == null)
        throw new Exception($"Az {adatNeve} nem lehet üres.");
    }

    public static string Left(string value, int length)
    {
      if (string.IsNullOrEmpty(value))
        return value;
      if (value.Length < length)
        return value;
      return value.Substring(0, length);
    }
  }
}