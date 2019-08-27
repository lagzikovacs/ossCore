using ossServer.Enums;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Ugyfelkapcsolat
{
    public class UgyfelkapcsolatParam
    {
        public int RekordTol { get; set; }
        public int LapMeret { get; set; }
        public int Ugyfelkod { get; set; }
        public List<SzMT> Fi { get; set; }
        public FromTo FromTo { get; set; }
    }
}
