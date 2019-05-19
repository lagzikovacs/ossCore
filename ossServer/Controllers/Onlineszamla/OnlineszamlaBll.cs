using ossServer.Controllers.Bizonylat;
using ossServer.Controllers.Session;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Onlineszamla
{
    public class OnlineszamlaBll
    {
        public static List<OnlineszamlaDto> Select(ossContext context, string sid, 
            int rekordTol, int lapMeret, List<SzMT> szmt, out int osszesRekord)
        {
            SessionBll.Check(context, sid);

            var qry = OnlineszamlaDal.GetQuery(context, szmt);
            osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
            var result = new List<OnlineszamlaDto>();
            foreach (var e in entities)
            {
                var dto = ObjectUtils.Convert<Models.Navfeltoltes, OnlineszamlaDto>(e);

                dto.Bizonylatszam = e.BizonylatkodNavigation.Bizonylatszam;
                dto.Ugyfelnev = e.BizonylatkodNavigation.Ugyfelnev;
                dto.Bizonylatkelte = e.BizonylatkodNavigation.Bizonylatkelte;

                result.Add(dto);
            }

            return result;
        }

        public static string SzamlaFormaiEllenorzese(BizonylatComplexDto dto)
        {
            //var invoice = OnlineszamlaBll.GetInvoice(context, bizonylatKod);
            //var hibak = invoice.ValidateErrors();
            //return hibak.Any() ? string.Join(Environment.NewLine, hibak) : "Minden rendben!";

            return "";
        }

        public static string LetoltesOnlineszamlaFormatumban(BizonylatComplexDto dto)
        {
            return "";
        }
    }
}
