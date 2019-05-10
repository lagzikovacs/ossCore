using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Projekt
{
    public class ProjektParam
    {
        public int RekordTol { get; set; }
        public int LapMeret { get; set; }
        public int Statusz { get; set; }
        public List<SzMT> Fi { get; set; }
    }
}
