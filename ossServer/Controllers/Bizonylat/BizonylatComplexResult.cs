using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatComplexResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<BizonylatComplexDto> Result { get; set; }
    }
}
