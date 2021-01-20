using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Bizonylat
{
    public class FuvardijParam
    {
        public BizonylatDto dtoAnyagszamla { get; set; }
        public BizonylatDto dtoFuvarszamla { get; set; }
        public decimal Fuvardij { get; set; }
    }
}
