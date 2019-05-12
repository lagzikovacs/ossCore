using ossServer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Utils
{
    public class SzMTUtils
    {
        private const string NemTartalmazza = "List<FilterItem> Fi nem tartalmazza 'pontosan egyszer' a(z) {0} szempontot!";
        private const string ErtekeNem = "{0} értéke nem '{1}'";

        private static object Get(List<SzMT> fi, Szempont szempont)
        {
            if (fi.Count(s => s.Szempont == szempont) == 1)
                return fi.First(s => s.Szempont == szempont).Minta;
            throw new Exception(string.Format(NemTartalmazza, szempont));
        }

        public static DateTime GetDate(List<SzMT> fi, Szempont szempont)
        {
            var o = Get(fi, szempont);
            if (!(o is DateTime))
                throw new Exception(string.Format(ErtekeNem, szempont, typeof(DateTime)));
            return (DateTime)o;
        }

        public static int GetInt(List<SzMT> fi, Szempont szempont)
        {
            var o = Get(fi, szempont);
            if (!((o is int) || (o is long))) //buta, persze, a json miatt...
                throw new Exception(string.Format(ErtekeNem, szempont, typeof(int)));
            return int.Parse(o.ToString());
        }

        public static string GetString(List<SzMT> fi, Szempont szempont)
        {
            var o = Get(fi, szempont);
            if (!(o is string))
                throw new Exception(string.Format(ErtekeNem, szempont, typeof(string)));
            return (string)o;
        }

        public static BizonylatNyomtatasTipus GetBizonylatNyomtatasTipus(List<SzMT> fi, Szempont szempont)
        {
            var o = Get(fi, szempont);
            return (BizonylatNyomtatasTipus)Enum.Parse(typeof(BizonylatNyomtatasTipus), o.ToString()); //json...
            //if (!(o is BizonylatNyomtatasTipus))
            //  throw new Exception(string.Format(ErtekeNem, szempont, typeof(BizonylatNyomtatasTipus)));
            //return (BizonylatNyomtatasTipus) o;
        }
    }
}
