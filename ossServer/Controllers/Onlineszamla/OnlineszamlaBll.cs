using ossServer.Controllers.Bizonylat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Onlineszamla
{
    public class OnlineszamlaBll
    {
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
