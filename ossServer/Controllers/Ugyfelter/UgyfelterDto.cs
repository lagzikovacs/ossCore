using ossServer.Controllers.Projekt;
using ossServer.Controllers.Ugyfel;
using System.Collections.Generic;

namespace ossServer.Controllers.Ugyfelter
{
    public class UgyfelterDto
    {
        public string sid { get; set; }
        public UgyfelDto ugyfelDto { get; set; }
        public List<ProjektDto> projektDto { get; set; }
    }
}
