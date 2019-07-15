using ossServer.Controllers.Dokumentum;
using ossServer.Controllers.Irat;
using ossServer.Controllers.Projekt;
using System.Collections.Generic;

namespace ossServer.Controllers.Fotozas
{
    public class FotozasDto
    {
        public string sid { get; set; }
        public List<IratDto> iratDto { get; set; }
        public List<DokumentumDto> dokumentumDto { get; set; }
        public List<ProjektDto> projektDto { get; set; }
    }
}
