using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Onlineszamla
{
    public class OnlineszamlaResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<OnlineszamlaDto> Result { get; set; }
    }
}
