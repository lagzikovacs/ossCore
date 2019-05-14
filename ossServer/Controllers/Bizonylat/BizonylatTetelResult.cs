using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatTetelResult : EmptyResult
    {
        public List<BizonylatTetelDto> Result { get; set; }
    }
}
