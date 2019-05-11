using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.ProjektKapcsolat
{
    public class ProjektKapcsolatResult : EmptyResult
    {
        public List<ProjektKapcsolatDto> Result { get; set; }
    }
}
