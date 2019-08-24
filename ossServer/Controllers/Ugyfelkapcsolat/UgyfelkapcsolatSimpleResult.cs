using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Ugyfelkapcsolat
{
    public class UgyfelkapcsolatSimpleResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<UgyfelkapcsolatDto> Result { get; set; }
    }
}
