using ossServer.Controllers.Bizonylat;
using ossServer.Controllers.Dokumentum;
using ossServer.Controllers.Irat;
using ossServer.Controllers.Projekt;
using ossServer.Controllers.ProjektKapcsolat;
using ossServer.Controllers.Ugyfel;
using System.Collections.Generic;

namespace ossServer.Controllers.Ugyfelter
{
    public class UgyfelterDto
    {
        public string sid { get; set; }
        public UgyfelDto ugyfelDto { get; set; }
        public List<ProjektDto> lstProjektDto { get; set; }
    }
}
