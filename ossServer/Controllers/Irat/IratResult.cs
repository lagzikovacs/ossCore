using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Irat
{
    public class IratResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<IratDto> Result { get; set; }
    }
}
