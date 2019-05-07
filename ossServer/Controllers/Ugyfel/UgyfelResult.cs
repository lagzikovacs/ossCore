using System.Collections.Generic;

namespace ossServer.Controllers.Ugyfel
{
    public class UgyfelResult
    {
        public int OsszesRekord { get; set; }
        public List<UgyfelDto> Result { get; set; }
    }
}
