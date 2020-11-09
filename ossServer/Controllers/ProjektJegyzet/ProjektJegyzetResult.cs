using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.ProjektJegyzet
{
    public class ProjektJegyzetResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<ProjektJegyzetDto> Result { get; set; }
    }
}
