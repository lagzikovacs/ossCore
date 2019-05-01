using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Afakulcs
{
    public class AfaKulcsResult : EmptyResult
    {
        public List<AfakulcsDto> Result { get; set; }
    }
}
