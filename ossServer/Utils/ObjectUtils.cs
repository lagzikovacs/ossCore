using System;
using System.Collections.Generic;
using System.Reflection;

namespace ossServer.Utils
{
  public class ObjectUtils
    {
      public static TObjectType Clone<TObjectType>(TObjectType @from)
      {
        var tipus = typeof(TObjectType);
        var result = Activator.CreateInstance<TObjectType>();

        var pinfo = tipus.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var p in pinfo)
        {
          if (p.PropertyType.IsValueType || p.PropertyType == typeof(string) && p.CanWrite)
            p.SetValue(result, p.GetValue(@from));
        }

        return result;
      }

      public static TOType Convert<TFromType, TOType>(TFromType @from)
      {
        var to = Activator.CreateInstance<TOType>();

        var pinfo = typeof(TFromType).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var pfrom in pinfo)
        {
          if (pfrom.PropertyType.IsValueType || pfrom.PropertyType == typeof(string) && pfrom.CanWrite)
          {
            var pto = typeof(TOType).GetProperty(pfrom.Name, pfrom.PropertyType);
            if (pto != null)
              pto.SetValue(to, pfrom.GetValue(@from));
          }
        }

        return to;
      }
        public static List<TOType> Convert<TFromType, TOType>(List<TFromType> @from)
        {
            var result = Activator.CreateInstance<List<TOType>>();
            foreach (var f in @from)
                result.Add(Convert<TFromType, TOType>(f));

            return result;
        }

      public static void Update<TFromType, TOType>(TFromType @from, TOType to)
      {
        var pinfo = typeof(TFromType).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var pfrom in pinfo)
        {
          if (pfrom.PropertyType.IsValueType || pfrom.PropertyType == typeof(string) && pfrom.CanWrite)
          {
            var pto = typeof(TOType).GetProperty(pfrom.Name, pfrom.PropertyType);
            if (pto != null)
              pto.SetValue(to, pfrom.GetValue(@from));
          }
        }
      }
  }
}
