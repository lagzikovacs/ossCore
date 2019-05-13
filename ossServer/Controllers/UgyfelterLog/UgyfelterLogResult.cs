using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.UgyfelterLog
{
    public class UgyfelterLogResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<UgyfelterLogDto> Result { get; set; }
    }
}
