using ossServer.BaseResults;
using ossServer.Enums;
using System.Collections.Generic;

namespace ossServer.Controllers.Csoport
{
    public class JogaimResult : EmptyResult
    {
        public List<JogKod> Result { get; set; }
    }
}
