using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Projekt
{
    public class ProjektResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<ProjektDto> Result { get; set; }
    }
}
