using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Onlineszamla
{
    public class OnlineszamlaParam
    {
        public int RekordTol { get; set; }
        public int LapMeret { get; set; }
        public List<SzMT> Fi { get; set; }
    }
}
