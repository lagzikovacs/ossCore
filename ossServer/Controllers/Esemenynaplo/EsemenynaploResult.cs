using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Esemenynaplo
{
    public class EsemenynaploResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<EsemenynaploDto> Result { get; set; }
    }
}
