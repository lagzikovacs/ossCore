using GemBox.Spreadsheet;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Dokumentum;
using ossServer.Controllers.Irat;
using ossServer.Controllers.Particio;
using ossServer.Controllers.Primitiv.Felhasznalo;
using ossServer.Controllers.Primitiv.Irattipus;
using ossServer.Controllers.Projekt;
using ossServer.Controllers.ProjektKapcsolat;
using ossServer.Controllers.Session;
using ossServer.Controllers.Ugyfel;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ajanlat
{
    public class AjanlatBll
    {
        public static async Task<AjanlatParam> CreateNewAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.AJANLATKESZITES);

            return new AjanlatParam
            {
                AjanlatBuf = new List<AjanlatBuf> {
                  new AjanlatBuf {AjanlatTetelTipus = AjanlatTetelTipus.Napelem, TetelTipus = "Napelem", Garancia = 10 },
                  new AjanlatBuf {AjanlatTetelTipus = AjanlatTetelTipus.Inverter, TetelTipus = "Inverter", Garancia = 5},
                  new AjanlatBuf {AjanlatTetelTipus = AjanlatTetelTipus.MechanikaiSzerelveny, TetelTipus = "Mech. szerelvény", Garancia = 10},
                  new AjanlatBuf {AjanlatTetelTipus = AjanlatTetelTipus.VillamosSzerelveny, TetelTipus = "Vill. szerelvény", Garancia = 5},
                  new AjanlatBuf {AjanlatTetelTipus = AjanlatTetelTipus.Ugyintezes, TetelTipus = "Ügyintézés"},
                  new AjanlatBuf {AjanlatTetelTipus = AjanlatTetelTipus.Munkadij, TetelTipus = "Munkadíj", Garancia = 10},
                  new AjanlatBuf {AjanlatTetelTipus = AjanlatTetelTipus.TuzesetiKapcsolo, TetelTipus = "Tűzeseti kapcsoló"},
                },
                Fi = new List<SzMT> {
                  new SzMT {Szempont = Szempont.Ervenyes},
                  new SzMT {Szempont = Szempont.Tajolas},
                  new SzMT {Szempont = Szempont.Termeles},
                  new SzMT {Szempont = Szempont.Megjegyzes},
                  new SzMT {Szempont = Szempont.SzuksegesAramerosseg},
                },
                ProjektKod = -1,

                Ervenyes = DateTime.Now.AddMonths(1),
                SzuksegesAramerosseg = "",
                Tajolas = "déli",
                Termeles = 1100,
                Megjegyzes = "",

                Netto = 0,
                Afa = 0,
                Brutto = 0
            };
        }

        public static async Task<int> AjanlatKesztitesAsync(ossContext context, string sid, 
            int projektKod, List<AjanlatBuf> ajanlatBuf, List<SzMT> fi)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.AJANLATKESZITES);

            ExcelFile _excel;
            ExcelWorksheet _sheet;

            SpreadsheetInfo.SetLicense("ERDD-TN5J-YKX9-H1KX");

            var ervenyes = SzMTUtils.GetDate(fi, Szempont.Ervenyes);
            var tajolas = SzMTUtils.GetString(fi, Szempont.Tajolas);
            var termeles = SzMTUtils.GetInt(fi, Szempont.Termeles);
            var megjegyzes = SzMTUtils.GetString(fi, Szempont.Megjegyzes);
            var szuksegesAramerosseg = SzMTUtils.GetString(fi, Szempont.SzuksegesAramerosseg);

            var entityProjekt = ProjektDal.Get(context, projektKod);
            var entityParticio = ParticioDal.Get(context);
            var iratKod = entityParticio.ProjektAjanlatIratkod != null ?
                (int)entityParticio.ProjektAjanlatIratkod : throw new Exception(string.Format(Messages.ParticioHiba, "PROJEKT_AJANLAT_IRATKOD"));
            var fb = await IratBll.LetoltesAsync(context, sid, iratKod);

            using (var msExcel = new MemoryStream())
            {
                msExcel.Write(fb.b, 0, fb.b.Count());
                msExcel.Position = 0;
                _excel = ExcelFile.Load(msExcel, GemBox.Spreadsheet.LoadOptions.XlsDefault);
            }
            if (_excel.Worksheets.Count != 1)
                throw new Exception("Az alapbizonylat nincs előkészítve!");
            _sheet = _excel.Worksheets[0];

            XlsUtils.Mezo(_sheet, 7, 3, entityProjekt.UgyfelkodNavigation.Nev);
            XlsUtils.Mezo(_sheet, 7, 8, DateTime.Now.Date.ToShortDateString());
            XlsUtils.Mezo(_sheet, 8, 8, ervenyes.Date.ToShortDateString());
            var ugyfelCim = UgyfelBll.Cim(entityProjekt.UgyfelkodNavigation).Trim(' ', ',');
            XlsUtils.Mezo(_sheet, 10, 3, ugyfelCim);

            var napelem = ajanlatBuf.First(s => s.AjanlatTetelTipus == AjanlatTetelTipus.Napelem);
            var inverter = ajanlatBuf.First(s => s.AjanlatTetelTipus == AjanlatTetelTipus.Inverter);
            var mechanikaiSzerelveny =
                ajanlatBuf.First(s => s.AjanlatTetelTipus == AjanlatTetelTipus.MechanikaiSzerelveny);
            var villamosSzerelveny = ajanlatBuf.First(s => s.AjanlatTetelTipus == AjanlatTetelTipus.VillamosSzerelveny);
            var ugyintezes = ajanlatBuf.First(s => s.AjanlatTetelTipus == AjanlatTetelTipus.Ugyintezes);
            var munkadij = ajanlatBuf.First(s => s.AjanlatTetelTipus == AjanlatTetelTipus.Munkadij);
            var tuzesetiKapcsolo = ajanlatBuf.First(s => s.AjanlatTetelTipus == AjanlatTetelTipus.TuzesetiKapcsolo);

            var dckW = Calc.RealRound(napelem.OsszTeljesitmeny / 1000, 0.01m);
            var ackVa = Calc.RealRound(inverter.OsszTeljesitmeny / 1000, 0.01m);

            XlsUtils.Mezo(_sheet, 13, 3, dckW);
            XlsUtils.Mezo(_sheet, 14, 3, ackVa);
            XlsUtils.Mezo(_sheet, 15, 3, szuksegesAramerosseg);

            XlsUtils.Mezo(_sheet, 13, 5, tajolas);
            XlsUtils.Mezo(_sheet, 14, 5, "min. " + (dckW * termeles).ToString("#0.") +
                " kWh, azaz " + (dckW * termeles * 40).ToString("#0.") + " Ft");

            XlsUtils.Mezo(_sheet, 18, 3, napelem.CikkNev);
            XlsUtils.Mezo(_sheet, 18, 5, napelem.Mennyiseg);
            XlsUtils.Mezo(_sheet, 18, 6, napelem.EgysegAr);

            XlsUtils.Mezo(_sheet, 19, 3, inverter.CikkNev);
            XlsUtils.Mezo(_sheet, 19, 5, inverter.Mennyiseg);
            XlsUtils.Mezo(_sheet, 19, 6, inverter.EgysegAr);

            XlsUtils.Mezo(_sheet, 20, 3, mechanikaiSzerelveny.CikkNev);
            XlsUtils.Mezo(_sheet, 20, 5, mechanikaiSzerelveny.Mennyiseg);
            XlsUtils.Mezo(_sheet, 20, 6, mechanikaiSzerelveny.EgysegAr);

            XlsUtils.Mezo(_sheet, 21, 3, villamosSzerelveny.CikkNev);
            XlsUtils.Mezo(_sheet, 21, 5, villamosSzerelveny.Mennyiseg);
            XlsUtils.Mezo(_sheet, 21, 6, villamosSzerelveny.EgysegAr);

            XlsUtils.Mezo(_sheet, 22, 7, ugyintezes.EgysegAr);

            XlsUtils.Mezo(_sheet, 23, 7, munkadij.EgysegAr);

            XlsUtils.Mezo(_sheet, 26, 7, tuzesetiKapcsolo.EgysegAr);

            var gar =
                "A napelemekre {0} év gyári, az inverterre {1} év gyári, mechanikai szerelvényre {2} év, villamos szerelvényre {3} év, kivitelezésre {4} év garancia érvényes.";
            XlsUtils.Mezo(_sheet, 32, 3,
                string.Format(gar, napelem.Garancia, inverter.Garancia, mechanikaiSzerelveny.Garancia,
                villamosSzerelveny.Garancia, munkadij.Garancia));

            var felhasznalo = await FelhasznaloDal.GetAsync(context, context.CurrentSession.Felhasznalokod);
            XlsUtils.Mezo(_sheet, 8, 3, $"{felhasznalo.Nev}, {felhasznalo.Telefon}, {felhasznalo.Email}");

            //az excel mezőkkiértékelése
            _sheet.Calculate();

            const string irattipus = "Ajánlat";
            var lstIrattipus = (await IrattipusDal.ReadAsync(context, irattipus)).Where(s => s.Irattipus1 == irattipus).ToList();
            if (lstIrattipus.Count != 1)
                throw new Exception($"Hiányzó irattipus: {irattipus}!");

            var ujIrat = new Models.Irat
            {
                Irany = "Ki",
                Keletkezett = DateTime.Now.Date,
                Irattipuskod = lstIrattipus[0].Irattipuskod,
                Ugyfelkod = entityProjekt.Ugyfelkod,
                Targy = megjegyzes
            };

            var ujIratKod = await IratDal.AddAsync(context, ujIrat);

            using (var stream = new MemoryStream())
            {
                _excel.Save(stream, SaveOptions.XlsDefault);

                stream.Position = 0;
                var fajlBuf = new FajlBuf
                {
                    Meret = (int)stream.Length,
                    Ext = fb.Ext,
                    Hash = Crypt.MD5Hash(stream),
                    IratKod = ujIratKod,
                    Megjegyzes = megjegyzes,
                    b = stream.ToArray()
                };

                await DokumentumBll.FeltoltesAsync(context, sid, fajlBuf);
            }
            
            return await ProjektKapcsolatBll.AddIratToProjektAsync(context, sid, projektKod, ujIratKod);
        }

        public static async Task<AjanlatParam> AjanlatCalcAsync(ossContext context, string sid, AjanlatParam ap)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.AJANLATKESZITES);

            foreach (var buf in ap.AjanlatBuf)
            {
                buf.Mennyiseg = Calc.RealRound(buf.Mennyiseg, 1m);
                buf.EgysegAr = Calc.RealRound(buf.EgysegAr, 1m);
                buf.Netto = Calc.RealRound(buf.Mennyiseg * buf.EgysegAr, 1m);
                buf.Afa = Calc.RealRound(buf.Netto * buf.AfaMerteke / 100m, 1m);
                buf.Brutto = Calc.RealRound(buf.Netto + buf.Afa, 1m);

                buf.OsszTeljesitmeny = Calc.RealRound(buf.Mennyiseg * buf.EgysegnyiTeljesitmeny, 1m);
            }

            ap.Netto = ap.AjanlatBuf.Sum(s => s.Netto);
            ap.Afa = ap.AjanlatBuf.Sum(s => s.Afa);
            ap.Brutto = ap.Netto + ap.Afa;

            return ap;
        }
    }
}
