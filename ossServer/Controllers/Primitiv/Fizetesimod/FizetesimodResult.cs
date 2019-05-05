using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Fizetesimod
{
    public class FizetesimodResult : EmptyResult
    {
        public List<FizetesimodDto> Result { get; set; }
    }
}
