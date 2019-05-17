using NGM.VI30232014;
using ossServer.Controllers.Bizonylat;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ossServer.Controllers.NGM
{
    public class NgmBll
    {
        public static string Adatszolgaltatas(ossContext context, string sid,
            NGMMode mode, DateTime szamlaKelteTol, DateTime szamlaKelteIg, string szamlaSzamTol, string szamlaSzamIg)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.LEKERDEZES);

            //az xml-hez mind a 4 paraméter szükséges
            List<Models.Bizonylat> entities;
            switch (mode)
            {
                case NGMMode.SzamlaKelte:
                    entities = BizonylatDal.Select_SzamlaKelte(context, szamlaKelteTol, szamlaKelteIg);
                    szamlaSzamTol = entities.Select(s => s.Bizonylatszam).Min();
                    szamlaSzamIg = entities.Select(s => s.Bizonylatszam).Max();
                    break;
                case NGMMode.SzamlaSzam:
                    entities = BizonylatDal.Select_SzamlaSzam(context, szamlaSzamTol, szamlaSzamIg);
                    szamlaKelteTol = entities.Select(s => s.Bizonylatkelte).Min();
                    szamlaKelteIg = entities.Select(s => s.Bizonylatkelte).Max();
                    break;
                default:
                    throw new Exception($"{mode}: ismeretlen NGMMode!");
            }

            if (entities.Count == 0)
                throw new Exception("A megadott tartományban nincsenek számlák!");

            var szamlakXml = new Szamlak
            {
                EloallitasDatuma = DateTime.Now,
                KezdoSzamlaSzama = szamlaSzamTol,
                ZaroSzamlaSzama = szamlaSzamIg,
                KezdoDatum = szamlaKelteTol,
                ZaroDatum = szamlaKelteIg
            };

            foreach (var peldany in entities)
            {
                var ngmSzamla = new Szamla();
                ngmSzamla.Fejlec.SzamlaSorszama = peldany.Bizonylatszam;
                ngmSzamla.Fejlec.SzamlaTipusa = peldany.Ezstornozo ? SzamlaTipusok.ErvenytelenitoSzamla : SzamlaTipusok.Szamla;
                ngmSzamla.Fejlec.SzamlaKelte = peldany.Bizonylatkelte;
                ngmSzamla.Fejlec.TeljesitesDatuma = peldany.Teljesiteskelte;
                ngmSzamla.SzamlaKibocsato.Adoszam = $"{peldany.Szallitoadotorzsszam}-{peldany.Szallitoadoafakod}-{peldany.Szallitoadomegyekod}";
                ngmSzamla.SzamlaKibocsato.Kisadozo = false;
                ngmSzamla.SzamlaKibocsato.Nev = peldany.Szallitonev;
                ngmSzamla.SzamlaKibocsato.Cim.Iranyitoszam = peldany.Szallitoiranyitoszam;
                ngmSzamla.SzamlaKibocsato.Cim.Telepules = peldany.Szallitohelysegnev;
                // TODO ez több partíció esetén nem jó!!
                ngmSzamla.SzamlaKibocsato.Cim.KozteruletNeve = "Túzok";
                ngmSzamla.SzamlaKibocsato.Cim.KozteruletJellege = "utca";
                ngmSzamla.SzamlaKibocsato.Cim.Hazszam = "42.";
                ngmSzamla.SzamlaKibocsato.EgyeniVallalkozo = false;
                ngmSzamla.SzamlaBefogado.Adoszam = peldany.Ugyfeladoszam;
                ngmSzamla.SzamlaBefogado.KozossegiAdoszam = string.Empty;
                ngmSzamla.SzamlaBefogado.Nev = peldany.Ugyfelnev;
                ngmSzamla.SzamlaBefogado.Cim.Iranyitoszam = peldany.Ugyfeliranyitoszam;
                ngmSzamla.SzamlaBefogado.Cim.Telepules = peldany.Ugyfelhelysegnev;
                ngmSzamla.SzamlaBefogado.Cim.KozteruletNeve = peldany.Ugyfelkozterulet;
                ngmSzamla.SzamlaBefogado.Cim.KozteruletJellege = peldany.Ugyfelkozterulettipus;
                ngmSzamla.SzamlaBefogado.Cim.Hazszam = peldany.Ugyfelhazszam;

                foreach (var tetel in peldany.Bizonylattetel)
                {
                    var tszt = new TermekSzolgaltatasTetel
                    {
                        Megnevezes = tetel.Megnevezes,
                        BesorolasiSzam = "-",
                        Mennyiseg = tetel.Mennyiseg,
                        Mertekegyseg = tetel.Me,
                        KozvetitettSzolgaltatas = true,
                        NettoAr = tetel.Netto,
                        NettoEgysegar = tetel.Egysegar,
                        AdoKulcs = tetel.Afamerteke,
                        AdoErtek = tetel.Afa,
                        BruttoAr = tetel.Brutto
                    };
                    ngmSzamla.TermekSzolgaltatasTetelek.Add(tszt);
                }

                ngmSzamla.NemKotelezo.FizetesModja = peldany.Fizetesimod;
                ngmSzamla.NemKotelezo.FizetesiHatarido = peldany.Fizetesihatarido;
                ngmSzamla.NemKotelezo.SzamlaPenzneme = peldany.Penznem;

                if (ngmSzamla.Fejlec.SzamlaTipusa != SzamlaTipusok.SzamlavalEgyTekintetAlaEsoOkirat)
                {
                    foreach (var afabontas in peldany.Bizonylatafa)
                    {
                        ngmSzamla.Osszesites.AfaRovatok.Add(new AfaRovat
                        {
                            NettoAr = afabontas.Netto,
                            Adokulcs = afabontas.Afamerteke,
                            AdoErtek = afabontas.Afa,
                            BruttoAr = afabontas.Brutto
                        });
                    }
                    ngmSzamla.Osszesites.Vegosszeg.NettoArOsszesen = peldany.Netto;
                    ngmSzamla.Osszesites.Vegosszeg.AfaOsszesen = peldany.Afa;
                    ngmSzamla.Osszesites.Vegosszeg.BruttoOsszesen = peldany.Brutto;
                }

                szamlakXml.SzamlakAdatai.Add(ngmSzamla);
            }

            var result = "";

            using (var sw = new StringWriter())
            {
                var xwsettings = new XmlWriterSettings
                {
                    CheckCharacters = true,
                    CloseOutput = true,
                    Encoding = Encoding.UTF8,
                    Indent = true,
                    IndentChars = " ",
                    NewLineChars = Environment.NewLine,
                    NewLineOnAttributes = false,
                    NewLineHandling = NewLineHandling.Entitize
                };

                using (var xw = XmlWriter.Create(sw, xwsettings))
                    szamlakXml.ToXml(xw);

                result = sw.ToString();
            }

            return result;
        }
    }
}
