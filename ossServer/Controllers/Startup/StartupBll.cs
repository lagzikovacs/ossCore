using ossServer.Controllers.Ajanlatkeres;
using ossServer.Controllers.Cikk;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Dokumentum;
using ossServer.Controllers.Irat;
using ossServer.Controllers.Kifizetes;
using ossServer.Controllers.Penztar;
using ossServer.Controllers.PenztarTetel;
using ossServer.Controllers.Primitiv.Afakulcs;
using ossServer.Controllers.Primitiv.Felhasznalo;
using ossServer.Controllers.Primitiv.Fizetesimod;
using ossServer.Controllers.Primitiv.Helyseg;
using ossServer.Controllers.Primitiv.Irattipus;
using ossServer.Controllers.Primitiv.Me;
using ossServer.Controllers.Primitiv.Penznem;
using ossServer.Controllers.Primitiv.Teendo;
using ossServer.Controllers.Primitiv.Termekdij;
using ossServer.Controllers.Projekt;
using ossServer.Controllers.ProjektTeendo;
using ossServer.Controllers.Session;
using ossServer.Controllers.SzamlazasiRend;
using ossServer.Controllers.Ugyfel;
using ossServer.Controllers.UgyfelterLog;
using ossServer.Controllers.Volume;
using ossServer.Models;

namespace ossServer.Controllers.Startup
{
    public class StartupBll
    {
        public static StartupResult Get(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            var result = new StartupResult
            {
                Afakulcs_Grid = AfakulcsBll.GridColumns(),
                Afakulcs_Reszletek = AfakulcsBll.ReszletekColumns(),
                Felhasznalo_Grid = FelhasznaloBll.GridColumns(),
                Felhasznalo_Reszletek = FelhasznaloBll.ReszletekColumns(),
                Fizetesimod_Grid = FizetesimodBll.GridColumns(),
                Fizetesimod_Reszletek = FizetesimodBll.ReszletekColumns(),
                Helyseg_Grid = HelysegBll.GridColumns(),
                Helyseg_Reszletek = HelysegBll.ReszletekColumns(),
                Irattipus_Grid = IrattipusBll.GridColumns(),
                Irattipus_Reszletek = IrattipusBll.ReszletekColumns(),
                Me_Grid = MennyisegiegysegBll.GridColumns(),
                Me_Reszletek = MennyisegiegysegBll.ReszletekColumns(),
                Penznem_Grid = PenznemBll.GridColumns(),
                Penznem_Reszletek = PenznemBll.ReszletekColumns(),
                Teendo_Grid = TeendoBll.GridColumns(),
                Teendo_Reszletek = TeendoBll.ReszletekColumns(),
                Termekdij_Grid = TermekdijBll.GridColumns(),
                Termekdij_Reszletek = TermekdijBll.ReszletekColumns(),

                Cikk_Grid = CikkBll.GridColumns(),
                BeszerzesKivet_Grid = CikkBll.BeszerzesKivetGridColumns(),
                Cikk_Reszletek = CikkBll.ReszletekColumns(),
                Ugyfel_Grid = UgyfelBll.GridColumns(),
                Ugyfel_Reszletek = UgyfelBll.ReszletekColumns(),

                Projekt_Grid = ProjektBll.GridColumns(),
                Projekt_Reszletek = ProjektBll.ReszletekColumns(),
                Projektteendo_Grid = ProjektTeendoBll.GridColumns(),
                Projektteendo_Reszletek = ProjektTeendoBll.ReszletekColumns(),
                Szamlazasirend_Grid = SzamlazasiRendBll.GridColumns(),
                Szamlazasirend_Reszletek = SzamlazasiRendBll.ReszletekColumns(),

                Irat_Grid = IratBll.GridColumns(),
                Irat_Reszletek = IratBll.ReszletekColumns(),

                Csoport_Grid = CsoportBll.GridColumns(),
                Csoport_Reszletek = CsoportBll.ReszletekColumns(),

                Ajanlatkeres_Grid = AjanlatkeresBll.GridColumns(),
                Ajanlatkeres_Reszletek = AjanlatkeresBll.ReszletekColumns(),

                Penztar_Grid = PenztarBll.GridColumns(),
                Penztar_Reszletek = PenztarBll.ReszletekColumns(),
                Penztartetel_Grid = PenztarTetelBll.GridColumns(),
                Penztartetel_Reszletek = PenztarTetelBll.ReszletekColumns(),

                Kifizetes_Grid = KifizetesBll.GridColumns(),
                Kifizetes_Reszletek = KifizetesBll.ReszletekColumns(),
                Dokumentum_Grid = DokumentumBll.GridColumns(),
                Dokumentum_Reszletek = DokumentumBll.ReszletekColumns(),
                Volume_Grid = VolumeBll.GridColumns(),
                Volume_Reszletek = VolumeBll.ReszletekColumns(),
                Ugyfelterlog_Grid = UgyfelterLogBll.GridColumns(),
                Ugyfelterlog_Reszletek = UgyfelterLogBll.ReszletekColumns(),
            };

            return result;
        }
    }
}
