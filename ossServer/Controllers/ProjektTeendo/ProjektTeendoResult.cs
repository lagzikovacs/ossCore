using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.ProjektTeendo
{
    public class ProjektTeendoResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<ProjektTeendoDto> Result { get; set; }
    }
}
