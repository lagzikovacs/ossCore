using ossServer.Enums;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatParam
    {
        public int RekordTol { get; set; }
        public int LapMeret { get; set; }
        public BizonylatTipus BizonylatTipus { get; set; }
        public List<SzMT> Fi { get; set; }
    }
}
