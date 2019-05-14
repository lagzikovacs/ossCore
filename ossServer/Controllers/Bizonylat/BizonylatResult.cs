using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<BizonylatDto> Result { get; set; }
    }
}
