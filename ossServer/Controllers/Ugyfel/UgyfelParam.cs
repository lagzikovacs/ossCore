using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Ugyfel
{
    public class UgyfelParam
    {
        public int RekordTol { get; set; }
        public int LapMeret { get; set; }
        public int Csoport { get; set; }
        public List<SzMT> Fi { get; set; }
    }
}
