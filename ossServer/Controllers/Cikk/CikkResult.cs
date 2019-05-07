using System.Collections.Generic;

namespace ossServer.Controllers.Cikk
{
    public class CikkResult
    {
        public int OsszesRekord { get; set; }
        public List<CikkDto> Result { get; set; }
    }
}
