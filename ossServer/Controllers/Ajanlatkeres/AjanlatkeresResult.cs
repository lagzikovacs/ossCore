using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Ajanlatkeres
{
    public class AjanlatkeresResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<AjanlatkeresDto> Result { get; set; }
    }
}
