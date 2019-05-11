using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.SzamlazasiRend
{
    public class SzamlazasiRendResult : EmptyResult
    {
        public int OsszesRekord { get; set; }
        public List<SzamlazasiRendDto> Result { get; set; }
    }
}
