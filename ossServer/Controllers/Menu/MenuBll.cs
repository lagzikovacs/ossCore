using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Menu
{
    public class MenuBll
    {
        public static async Task<List<AngularMenuDto>> AngularMenuAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            var jogok = await CsoportBll.JogaimAsync(context, sid);

            var result = new List<AngularMenuDto>
            {
            new AngularMenuDto
            {
              Title = "Törzsadatok",
              Sub = new List<AngularMenuDto>
            {
              new AngularMenuDto {Title = "Irattipus", RouterLink = "/irattipus", Enabled = jogok.Contains(JogKod.PRIMITIVEK)},
              new AngularMenuDto {Title = "divider"},
              new AngularMenuDto {Title = "Fizetési mód", RouterLink = "/fizetesimod", Enabled = jogok.Contains(JogKod.PRIMITIVEK)},
              new AngularMenuDto {Title = "Pénznem", RouterLink = "/penznem", Enabled = jogok.Contains(JogKod.PRIMITIVEK)},
              new AngularMenuDto {Title = "divider"},
              new AngularMenuDto {Title = "Mennyiségi egység", RouterLink = "/me", Enabled = jogok.Contains(JogKod.PRIMITIVEK)},
              new AngularMenuDto {Title = "ÁFA kulcs", RouterLink = "/afakulcs", Enabled = jogok.Contains(JogKod.PRIMITIVEK)},
              new AngularMenuDto {Title = "Termékdíj", RouterLink = "/termekdij", Enabled = jogok.Contains(JogKod.PRIMITIVEK)},
              new AngularMenuDto {Title = "Cikk", RouterLink = "/cikk", Enabled = jogok.Contains(JogKod.CIKK)},
              new AngularMenuDto {Title = "divider"},
              new AngularMenuDto {Title = "Helység", RouterLink = "/helyseg", Enabled = jogok.Contains(JogKod.PRIMITIVEK)},
              new AngularMenuDto {Title = "Tevékenység", RouterLink = "/tevekenyseg", Enabled = jogok.Contains(JogKod.PRIMITIVEK)},
              new AngularMenuDto {Title = "Ügyfél", RouterLink = "/ugyfel", Enabled = jogok.Contains(JogKod.UGYFELEK)},
            }
            },
            new AngularMenuDto
            {
              Title = "Eszközök",
              Sub = new List<AngularMenuDto>
            {
              new AngularMenuDto { Title = "Projekt", RouterLink = "/projekt", Enabled = jogok.Contains(JogKod.PROJEKT) },
              new AngularMenuDto { Title = "Irat", RouterLink = "/irat", Enabled = jogok.Contains(JogKod.IRAT) },
              new AngularMenuDto { Title = "divider" },
              new AngularMenuDto { Title = "Pénztár", RouterLink = "/penztar", Enabled = jogok.Contains(JogKod.PENZTAR)},
              new AngularMenuDto { Title = "divider"},
              new AngularMenuDto { Title = "Ajánlatkérés", RouterLink = "/ajanlatkeres", Enabled = jogok.Contains(JogKod.AJANLATKERES)},
              new AngularMenuDto { Title = "Ügyféltér log", RouterLink = "/ugyfelterlog", Enabled = jogok.Contains(JogKod.UGYFELTERLOG)},
              new AngularMenuDto { Title = "divider"},
              new AngularMenuDto { Title = "Kapcsolati háló", RouterLink = "/kapcsolatihalo", Enabled = jogok.Contains(JogKod.KAPCSOLATIHALO)},
            }
            },
            new AngularMenuDto
            {
              Title = "Bizonylatok",
              Sub = new List<AngularMenuDto>
            {
              new AngularMenuDto {Title = "Díjbekérő", RouterLink = "/bizonylat/dijbekero", Enabled = jogok.Contains(JogKod.DIJBEKERO)},
              new AngularMenuDto {Title = "Előlegszámla", RouterLink = "/bizonylat/elolegszamla", Enabled = jogok.Contains(JogKod.ELOLEGSZAMLA)},
              new AngularMenuDto {Title = "divider"},
              new AngularMenuDto {Title = "Szállító", RouterLink = "/bizonylat/szallito", Enabled = jogok.Contains(JogKod.SZALLITO)},
              new AngularMenuDto {Title = "Számla", RouterLink = "/bizonylat/szamla", Enabled = jogok.Contains(JogKod.SZAMLA)},
              new AngularMenuDto {Title = "divider"},
              new AngularMenuDto {Title = "Megrendelés", RouterLink = "/bizonylat/megrendeles", Enabled = jogok.Contains(JogKod.MEGRENDELES)},
              new AngularMenuDto {Title = "Bejövő számla", RouterLink = "/bizonylat/bejovoszamla", Enabled = jogok.Contains(JogKod.BEJOVOSZAMLA)},
              new AngularMenuDto {Title = "divider"},
              new AngularMenuDto {Title = "Online számla", RouterLink = "/navfeltoltesellenorzese", Enabled = jogok.Contains(JogKod.NAVFELTOLTESELLENORZESE)},
              new AngularMenuDto {Title = "divider"},
              new AngularMenuDto {Title = "NAV adószám ellenőrzés", RouterLink = "/adoszamellenorzes", Enabled = true},
              new AngularMenuDto {Title = "NAV számlalekérdezés", RouterLink = "/szamlalekerdezes", Enabled = jogok.Contains(JogKod.NAVSZAMLALEKERDEZES)},
            }
            },
            new AngularMenuDto
            {
              Title = "Riportok",
              Sub = new List<AngularMenuDto>
            {
              new AngularMenuDto { Title = "Kimenő számla", RouterLink = "/riport/kimenoszamla", Enabled = jogok.Contains(JogKod.LEKERDEZES)},
              new AngularMenuDto { Title = "Bejövő számla", RouterLink = "/riport/bejovoszamla", Enabled = jogok.Contains(JogKod.LEKERDEZES) },
              new AngularMenuDto { Title = "divider" },
              new AngularMenuDto { Title = "Követelések", RouterLink = "/riport/koveteles", Enabled = jogok.Contains(JogKod.LEKERDEZES) },
              new AngularMenuDto { Title = "Tartozások", RouterLink = "/riport/tartozas", Enabled = jogok.Contains(JogKod.LEKERDEZES) },
              new AngularMenuDto { Title = "divider" },
              new AngularMenuDto { Title = "Beszerzés", RouterLink = "/riport/beszerzes", Enabled = jogok.Contains(JogKod.LEKERDEZES) },
              new AngularMenuDto { Title = "Készlet", RouterLink = "/riport/keszlet", Enabled = jogok.Contains(JogKod.LEKERDEZES) },
              new AngularMenuDto { Title = "divider" },
              new AngularMenuDto { Title = "Adóhatósági ellenőrzési adatszolgáltatás", RouterLink = "/riport/ngm", Enabled = jogok.Contains(JogKod.LEKERDEZES) }
            }
            },
            new AngularMenuDto
            {
              Title = "Segédeszközök",
              Sub = new List<AngularMenuDto>
            {
              new AngularMenuDto { Title = "Partíció", RouterLink = "/particio", Enabled = jogok.Contains(JogKod.PARTICIO) },
              new AngularMenuDto { Title = "Volume", RouterLink = "/volume", Enabled = jogok.Contains(JogKod.VOLUME) },
              new AngularMenuDto { Title = "divider" },
              new AngularMenuDto { Title = "Felhasználó", RouterLink = "/felhasznalo", Enabled = jogok.Contains(JogKod.FELHASZNALO) },
              new AngularMenuDto { Title = "Csoport", RouterLink = "/csoport", Enabled = jogok.Contains(JogKod.CSOPORT) },
              new AngularMenuDto { Title = "divider" },
              new AngularMenuDto { Title = "Új bejelentkezés", RouterLink = "/bejelentkezes", Enabled = true },
              new AngularMenuDto { Title = "Szerepkörválasztás", RouterLink = "/szerepkorvalasztas", Enabled = true },
              new AngularMenuDto { Title = "Jelszócsere", RouterLink = "/jelszocsere", Enabled = true },
              new AngularMenuDto { Title = "divider" },
              new AngularMenuDto { Title = "Vágólap", RouterLink = "/vagolap", Enabled = true }
            }
            }
            };

            return result;
        }
    }
}

