using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Penztar
{
    public class PenztarResult : EmptyResult
    {
        public List<PenztarDto> Result { get; set; }
    }
}
