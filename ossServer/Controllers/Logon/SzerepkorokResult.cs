using ossServer.BaseResults;
using ossServer.Controllers.Csoport;
using System.Collections.Generic;

namespace ossServer.Controllers.Logon
{
    public class SzerepkorokResult : EmptyResult
    {
        public List<CsoportDto> Result { get; set; }
    }
}
