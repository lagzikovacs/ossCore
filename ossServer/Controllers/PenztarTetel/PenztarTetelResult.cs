using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.PenztarTetel
{
    public class PenztarTetelResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<PenztarTetelDto> Result { get; set; }
    }
}
