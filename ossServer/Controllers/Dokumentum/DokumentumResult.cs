using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Dokumentum
{
    public class DokumentumResult : EmptyResult
    {
        public List<DokumentumDto> Result { get; set; }
    }
}
