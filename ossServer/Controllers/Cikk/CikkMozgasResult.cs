using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Cikk
{
    public class CikkMozgasResult : EmptyResult
    {
        public List<CikkMozgasTetelDto> Result { get; set; }
    }
}
