using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Particio
{
    public class ParticioResult : EmptyResult
    {
        public List<ParticioDto> Result { get; set; }
    }
}
