using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.BizonylatKapcsolat
{
    public class BizonylatKapcsolatResult : EmptyResult
    {
        public List<BizonylatKapcsolatDto> Result { get; set; }
    }
}
