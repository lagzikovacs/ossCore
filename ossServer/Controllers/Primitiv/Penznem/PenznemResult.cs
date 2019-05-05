using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Penznem
{
    public class PenznemResult : EmptyResult
    {
        public List<PenznemDto> Result { get; set; }
    }
}
