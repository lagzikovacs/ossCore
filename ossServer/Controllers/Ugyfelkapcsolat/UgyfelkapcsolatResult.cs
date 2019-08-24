using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Ugyfelkapcsolat
{
    public class UgyfelkapcsolatResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<UgyfelkapcsolatDto> Result { get; set; }
    }
}
