using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Kifizetes
{
    public class KifizetesResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<KifizetesDto> Result { get; set; }
    }
}
