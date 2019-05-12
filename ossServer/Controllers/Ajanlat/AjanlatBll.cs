using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ajanlat
{
    public class AjanlatBll
    {
        private readonly string _sid;

        private ExcelFile _excel;
        private ExcelWorksheet _sheet;

        public AjanlatBll(string sid)
        {
            _sid = sid;

            SpreadsheetInfo.SetLicense("ERDD-TN5J-YKX9-H1KX");
        }

        public AjanlatParam CreateNew()
        {
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

        private void Mezo(int sor, int oszlop, object value)
        {
            XlsUtils.Mezo(_sheet, sor, oszlop, value);
        }

        public int AjanlatKesztites(int projektKod, List<AjanlatBuf> ajanlatBuf, List<SzMT> fi)
        {
            using (var model = OSSContext.NewContext(_sid))
                try
                {
                    model.Open(true);
                    CsoportDal.Joge(model, JogKod.AJANLATKESZITES);

                    var ervenyes = SzMTUtils.GetDate(fi, Szempont.Ervenyes);
                    var tajolas = SzMTUtils.GetString(fi, Szempont.Tajolas);
                    var termeles = SzMTUtils.GetInt(fi, Szempont.Termeles);
                    var megjegyzes = SzMTUtils.GetString(fi, Szempont.Megjegyzes);
                    var szuksegesAramerosseg = SzMTUtils.GetString(fi, Szempont.SzuksegesAramerosseg);

                    var entityProjekt = ProjektDal.Get(model, projektKod);
                    var entityParticio = ParticioDal.Get(model);
                    var iratKod = entityParticio.PROJEKT_AJANLAT_IRATKOD != null ?
                      (int)entityParticio.PROJEKT_AJANLAT_IRATKOD : throw new Exception(string.Format(Messages.ParticioHiba, "PROJEKT_AJANLAT_IRATKOD"));
                    var fb = IratBll.Letoltes(model, iratKod);

                    using (var msExcel = new MemoryStream())
                    {
                        msExcel.Write(fb.b, 0, fb.b.Count());
                        msExcel.Position = 0;
                        _excel = ExcelFile.Load(msExcel, GemBox.Spreadsheet.LoadOptions.XlsDefault);
                    }
                    if (_excel.Worksheets.Count != 1)
                        throw new Exception("Az alapbizonylat nincs előkészítve!");
                    _sheet = _excel.Worksheets[0];

                    Mezo(7, 3, entityProjekt.UGYFEL.NEV);
                    Mezo(7, 8, DateTime.Now.Date.ToShortDateString());
                    Mezo(8, 8, ervenyes.Date.ToShortDateString());
                    var ugyfelCim = UgyfelUtils.Cim(entityProjekt.UGYFEL).Trim(' ', ',');
                    Mezo(10, 3, ugyfelCim);

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

                    Mezo(13, 3, dckW);
                    Mezo(14, 3, ackVa);
                    Mezo(15, 3, szuksegesAramerosseg);

                    Mezo(13, 5, tajolas);
                    Mezo(14, 5, "min. " + (dckW * termeles).ToString("#0.") +
                      " kWh, azaz " + (dckW * termeles * 40).ToString("#0.") + " Ft");

                    Mezo(18, 3, napelem.CikkNev);
                    Mezo(18, 5, napelem.Mennyiseg);
                    Mezo(18, 6, napelem.EgysegAr);

                    Mezo(19, 3, inverter.CikkNev);
                    Mezo(19, 5, inverter.Mennyiseg);
                    Mezo(19, 6, inverter.EgysegAr);

                    Mezo(20, 3, mechanikaiSzerelveny.CikkNev);
                    Mezo(20, 5, mechanikaiSzerelveny.Mennyiseg);
                    Mezo(20, 6, mechanikaiSzerelveny.EgysegAr);

                    Mezo(21, 3, villamosSzerelveny.CikkNev);
                    Mezo(21, 5, villamosSzerelveny.Mennyiseg);
                    Mezo(21, 6, villamosSzerelveny.EgysegAr);

                    Mezo(22, 7, ugyintezes.EgysegAr);

                    Mezo(23, 7, munkadij.EgysegAr);

                    Mezo(26, 7, tuzesetiKapcsolo.EgysegAr);

                    var gar =
                      "A napelemekre {0} év gyári, az inverterre {1} év gyári, mechanikai szerelvényre {2} év, villamos szerelvényre {3} év, kivitelezésre {4} év garancia érvényes.";
                    Mezo(32, 3,
                      string.Format(gar, napelem.Garancia, inverter.Garancia, mechanikaiSzerelveny.Garancia,
                        villamosSzerelveny.Garancia, munkadij.Garancia));

                    var felhasznalo = FelhasznaloDal.Get(model, model.Session.FELHASZNALOKOD);
                    Mezo(8, 3, $"{felhasznalo.NEV}, {felhasznalo.TELEFON}, {felhasznalo.EMAIL}");

                    //az excel mezőkkiértékelése
                    _sheet.Calculate();

                    const string irattipus = "Ajánlat";
                    var lstIrattipus = IrattipusDal.Read(model, irattipus).Where(s => s.IRATTIPUS1 == irattipus).ToList();
                    if (lstIrattipus.Count != 1)
                        throw new Exception($"Hiányzó irattipus: {irattipus}!");

                    var ujIrat = new IRAT
                    {
                        IRANY = "Ki",
                        KELETKEZETT = DateTime.Now.Date,
                        IRATTIPUSKOD = lstIrattipus[0].IRATTIPUSKOD,
                        UGYFELKOD = entityProjekt.UGYFELKOD,
                        TARGY = megjegyzes
                    };

                    var ujIratKod = IratDal.Add(model, ujIrat);

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

                        DokumentumBll.Feltoltes(model, fajlBuf);
                    }

                    var result = ProjektKapcsolatBll.AddIratToProjekt(model, projektKod, ujIratKod);

                    if (model.Session.LOGOL)
                    {
                        EsemenynaploBll.Bejegyzes(model, EsemenynaploBejegyzesek.Ajanlatkeszites);
                        OssHub.Uzenet(model.Session.FELHASZNALO, EsemenynaploBejegyzesek.Ajanlatkeszites);
                    }

                    model.Commit();

                    return result;
                }
                catch (Exception ex)
                {
                    model.Rollback();
                    throw OSSContext.FaultException(ex);
                }
                finally
                {
                    model.Close();
                }
        }

        public AjanlatParam AjanlatCalc(AjanlatParam ap)
        {
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
