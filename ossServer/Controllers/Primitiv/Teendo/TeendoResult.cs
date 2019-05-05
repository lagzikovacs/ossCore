using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Teendo
{
    public class TeendoResult : EmptyResult
    {
        public List<TeendoDto> Result { get; set; }
    }
}
