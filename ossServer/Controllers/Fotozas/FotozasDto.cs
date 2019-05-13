using ossServer.Controllers.Dokumentum;
using ossServer.Controllers.Irat;
using System.Collections.Generic;

namespace ossServer.Controllers.Fotozas
{
    public class FotozasDto
    {
        public string sid { get; set; }
        public List<IratDto> iratDto { get; set; }
        public List<DokumentumDto> dokumentumDto { get; set; }
    }
}
