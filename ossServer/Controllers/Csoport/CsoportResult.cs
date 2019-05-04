using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Csoport
{
    public class CsoportResult : EmptyResult
    {
        public List<CsoportDto> Result { get; set; }
    }
}
