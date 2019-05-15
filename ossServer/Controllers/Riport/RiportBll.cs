using GemBox.Spreadsheet;
using ossServer.Controllers.Penztar;
using ossServer.Controllers.Ugyfel;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.IO;
using System.Linq;

namespace ossServer.Controllers.Riport
{
    public class RiportBll
    {
        private void Fejlec(int sor, int oszlop, string f)
        {
            _sheet.Cells[sor, oszlop].Value = f;
            _sheet.Cells[sor, oszlop].Style.Borders.SetBorders(MultipleBorders.Bottom, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Double);
        }
        private void Mezo(int sor, int oszlop, object value)
        {
            XlsUtils.Mezo(_sheet, sor, oszlop, value);
        }
        private void Vegosszeg(int sor, int oszlop, decimal d)
        {
            _sheet.Cells[sor, oszlop].Value = Calc.RealRound(d, (decimal)0.01);
            _sheet.Cells[sor, oszlop].Style.Borders.SetBorders(MultipleBorders.Top, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Double);
        }
        private void AutosizeColumns(int rowStart)
        {
            int columnCount = _sheet.CalculateMaxUsedColumns();
            //for (int i = 0; i < columnCount; i++)
            //    _sheet.Columns[i].AutoFit(1, _sheet.Rows[rowStart], _sheet.Rows[_sheet.Rows.Count - 1]);
        }

        public byte[] KimenoSzamlaLst(ossContext context, DateTime teljesitesKeltetol, DateTime teljesitesKelteig)
        {
            var bizonylatkodok = RiportDal.KimenoSzamlakBizonylatkodok(context, teljesitesKeltetol, teljesitesKelteig);

            BeginReport("Kimenő számlák");

            var rowStart = 3;
            var rowNum = rowStart;

            Fejlec(rowNum, 0, "Bizonylatszám");
            Fejlec(rowNum, 1, "Ügyfél neve");
            Fejlec(rowNum, 2, "Bizonylat kelte");
            Fejlec(rowNum, 3, "Teljesítés kelte");
            Fejlec(rowNum, 4, "Fizetési határidő");
            Fejlec(rowNum, 5, "Fizetési mód");
            Fejlec(rowNum, 6, "NettoFt");
            Fejlec(rowNum, 7, "ÁFA");
            Fejlec(rowNum, 8, "Brutto");
            Fejlec(rowNum, 9, "Pénznem");
            Fejlec(rowNum, 10, "Árfolyam");
            Fejlec(rowNum, 11, "NettoFt, Ft");
            ++rowNum;

            decimal sumNettoFt = 0;

            while (bizonylatkodok.Count > 0)
            {
                var egyAdag = bizonylatkodok.Take(100).ToList();

                var riporttetelek = RiportDal.BizonylatRiporttetelek(context, egyAdag);
                foreach (var tetel in riporttetelek)
                {
                    Mezo(rowNum, 0, tetel.Bizonylatszam);
                    Mezo(rowNum, 1, tetel.Ugyfelnev);
                    Mezo(rowNum, 2, tetel.Bizonylatkelte);
                    Mezo(rowNum, 3, tetel.Teljesiteskelte);
                    Mezo(rowNum, 4, tetel.Fizetesihatarido);
                    Mezo(rowNum, 5, tetel.Fizetesimod);
                    Mezo(rowNum, 6, tetel.Netto);
                    Mezo(rowNum, 7, tetel.Afa);
                    Mezo(rowNum, 8, tetel.Brutto);
                    Mezo(rowNum, 9, tetel.Penznem);
                    Mezo(rowNum, 10, tetel.Arfolyam);

                    var nettoFt = tetel.Netto * tetel.Arfolyam;
                    Mezo(rowNum, 11, nettoFt);

                    ++rowNum;

                    sumNettoFt += nettoFt;
                }

                bizonylatkodok.RemoveRange(0, egyAdag.Count);
            }

            Vegosszeg(rowNum, 11, sumNettoFt);

            AutosizeColumns(rowStart);

            Mezo(0, 0, "Kimenő számlák");
            Mezo(1, 0, "A teljesítés kelte: " + teljesitesKeltetol.ToShortDateString() + " - " +
              teljesitesKelteig.ToShortDateString());

            return EndReport();
        }

        public byte[] BejovoSzamla(ossContext context, DateTime teljesitesKeltetol, DateTime teljesitesKelteig)
        {
            var bizonylatkodok = RiportDal.BejovoSzamlakBizonylatkodok(context, teljesitesKeltetol, teljesitesKelteig);

            BeginReport("Bejövő számlák");

            var rowStart = 3;
            var rowNum = rowStart;

            Fejlec(rowNum, 0, "Bizonylatszám");
            Fejlec(rowNum, 1, "Ügyfel neve");
            Fejlec(rowNum, 2, "Bizonylat kelte");
            Fejlec(rowNum, 3, "Teljesítés kelte");
            Fejlec(rowNum, 4, "Fizetési határidő");
            Fejlec(rowNum, 5, "Fizetési mód");
            Fejlec(rowNum, 6, "NettoFt");
            Fejlec(rowNum, 7, "ÁFA");
            Fejlec(rowNum, 8, "Brutto");
            Fejlec(rowNum, 9, "Pénznem");
            Fejlec(rowNum, 10, "Árfolyam");
            Fejlec(rowNum, 11, "NettoFt, Ft");
            ++rowNum;

            decimal sumNettoFt = 0;

            while (bizonylatkodok.Count > 0)
            {
                var egyAdag = bizonylatkodok.Take(100).ToList();

                var riporttetelek = RiportDal.BizonylatRiporttetelek(context, egyAdag);
                foreach (var tetel in riporttetelek)
                {
                    Mezo(rowNum, 0, tetel.Bizonylatszam);
                    Mezo(rowNum, 1, tetel.Ugyfelnev);
                    Mezo(rowNum, 2, tetel.Bizonylatkelte);
                    Mezo(rowNum, 3, tetel.Teljesiteskelte);
                    Mezo(rowNum, 4, tetel.Fizetesihatarido);
                    Mezo(rowNum, 5, tetel.Fizetesimod);
                    Mezo(rowNum, 6, tetel.Netto);
                    Mezo(rowNum, 7, tetel.Afa);
                    Mezo(rowNum, 8, tetel.Brutto);
                    Mezo(rowNum, 9, tetel.Penznem);
                    Mezo(rowNum, 10, tetel.Arfolyam);
                    var nettoFt = tetel.Netto * tetel.Arfolyam;
                    Mezo(rowNum, 11, nettoFt);
                    ++rowNum;

                    sumNettoFt += nettoFt;
                }

                bizonylatkodok.RemoveRange(0, egyAdag.Count);
            }

            Vegosszeg(rowNum, 11, sumNettoFt);

            AutosizeColumns(rowStart);

            Mezo(0, 0, "Bejövő számlák");
            Mezo(1, 0,
              "A teljesítés kelte: " + teljesitesKeltetol.ToShortDateString() + " - " +
              teljesitesKelteig.ToShortDateString());

            return EndReport();
        }

        public byte[] KovetelesekLst(ossContext context, DateTime ezenANapon, bool lejart)
        {
            var bizonylatkodok = RiportDal.KovetelesekBizonylatkodok(context, ezenANapon, lejart);

            BeginReport("Követelések");

            var rowStart = 3;
            var rowNum = rowStart;

            Fejlec(rowNum, 0, "Bizonylatszám");
            Fejlec(rowNum, 1, "Ügyfel neve");
            Fejlec(rowNum, 2, "Bizonylat kelte");
            Fejlec(rowNum, 3, "Teljesítés kelte");
            Fejlec(rowNum, 4, "Fizetési határidő");
            Fejlec(rowNum, 5, "Fizetési mód");
            Fejlec(rowNum, 6, "NettoFt");
            Fejlec(rowNum, 7, "ÁFA");
            Fejlec(rowNum, 8, "Brutto");
            Fejlec(rowNum, 9, "Pénznem");
            Fejlec(rowNum, 10, "Árfolyam");
            Fejlec(rowNum, 11, "Brutto, Ft");
            Fejlec(rowNum, 12, "Megfizetve");
            ++rowNum;

            decimal sumBruttoFt = 0;

            while (bizonylatkodok.Count > 0)
            {
                var egyAdag = bizonylatkodok.Take(100).ToList();

                var riporttetelek = RiportDal.KovetelesekTartozasokRiporttetelek(context, egyAdag, ezenANapon);
                foreach (var tetel in riporttetelek)
                {
                    Mezo(rowNum, 0, tetel.Bizonylatszam);
                    Mezo(rowNum, 1, tetel.Ugyfelnev);
                    Mezo(rowNum, 2, tetel.Bizonylatkelte);
                    Mezo(rowNum, 3, tetel.Teljesiteskelte);
                    Mezo(rowNum, 4, tetel.Fizetesihatarido);
                    Mezo(rowNum, 5, tetel.Fizetesimod);
                    Mezo(rowNum, 6, tetel.Netto);
                    Mezo(rowNum, 7, tetel.Afa);
                    Mezo(rowNum, 8, tetel.Brutto);
                    Mezo(rowNum, 9, tetel.Penznem);
                    Mezo(rowNum, 10, tetel.Arfolyam);
                    var bruttoFt = tetel.Brutto * tetel.Arfolyam;
                    Mezo(rowNum, 11, bruttoFt);
                    Mezo(rowNum, 12, tetel.Megfizetve);
                    ++rowNum;

                    sumBruttoFt += bruttoFt;
                }

                bizonylatkodok.RemoveRange(0, egyAdag.Count);
            }

            Vegosszeg(rowNum, 11, sumBruttoFt);

            AutosizeColumns(rowStart);

            Mezo(0, 0, "Követelések");
            Mezo(1, 0, "Dátum: " + ezenANapon.ToShortDateString());

            return EndReport();
        }

        public byte[] TartozasokLst(ossContext context, DateTime ezenANapon, bool lejart)
        {
            var bizonylatkodok = RiportDal.TartozasokBizonylatkodok(context, ezenANapon, lejart);

            BeginReport("Tartozások");

            var rowStart = 3;
            var rowNum = rowStart;

            Fejlec(rowNum, 0, "Bizonylatszám");
            Fejlec(rowNum, 1, "Ügyfel neve");
            Fejlec(rowNum, 2, "Bizonylat kelte");
            Fejlec(rowNum, 3, "Teljesítés kelte");
            Fejlec(rowNum, 4, "Fizetési határidő");
            Fejlec(rowNum, 5, "Fizetési mód");
            Fejlec(rowNum, 6, "NettoFt");
            Fejlec(rowNum, 7, "ÁFA");
            Fejlec(rowNum, 8, "Brutto");
            Fejlec(rowNum, 9, "Pénznem");
            Fejlec(rowNum, 10, "Árfolyam");
            Fejlec(rowNum, 11, "Brutto, Ft");
            Fejlec(rowNum, 12, "Megfizetve");
            ++rowNum;

            decimal sumBruttoFt = 0;

            while (bizonylatkodok.Count > 0)
            {
                var egyAdag = bizonylatkodok.Take(100).ToList();

                var riporttetelek = RiportDal.KovetelesekTartozasokRiporttetelek(context, egyAdag, ezenANapon);
                foreach (var tetel in riporttetelek)
                {
                    Mezo(rowNum, 0, tetel.Bizonylatszam);
                    Mezo(rowNum, 1, tetel.Ugyfelnev);
                    Mezo(rowNum, 2, tetel.Bizonylatkelte);
                    Mezo(rowNum, 3, tetel.Teljesiteskelte);
                    Mezo(rowNum, 4, tetel.Fizetesihatarido);
                    Mezo(rowNum, 5, tetel.Fizetesimod);
                    Mezo(rowNum, 6, tetel.Netto);
                    Mezo(rowNum, 7, tetel.Afa);
                    Mezo(rowNum, 8, tetel.Brutto);
                    Mezo(rowNum, 9, tetel.Penznem);
                    Mezo(rowNum, 10, tetel.Arfolyam);
                    var bruttoFt = tetel.Brutto * tetel.Arfolyam;
                    Mezo(rowNum, 11, bruttoFt);
                    Mezo(rowNum, 12, tetel.Megfizetve);
                    ++rowNum;

                    sumBruttoFt += bruttoFt;
                }

                bizonylatkodok.RemoveRange(0, egyAdag.Count);
            }

            Vegosszeg(rowNum, 11, sumBruttoFt);

            AutosizeColumns(rowStart);

            Mezo(0, 0, "Tartozások");
            Mezo(1, 0, "Dátum: " + ezenANapon.ToShortDateString());

            return EndReport();
        }

        public byte[] BeszerzesLst(ossContext context, DateTime teljesitesKeltetol,
          DateTime teljesitesKelteig, bool reszletekIs)
        {
            var riporttetelek = RiportDal.BeszerzesRiporttetelek(context, teljesitesKeltetol, teljesitesKelteig);

            var nev = "Beszerzés";
            if (reszletekIs)
                nev += " részletesen";

            BeginReport(nev);

            var rowStart = 3;
            var rowNum = rowStart;

            Fejlec(rowNum, 0, "Megnevezés");
            Fejlec(rowNum, 1, "Mennyiség");
            Fejlec(rowNum, 2, "Me");
            Fejlec(rowNum, 3, "NettoFt, Ft");

            if (reszletekIs)
            {
                Fejlec(rowNum, 4, "Bizonylatszám");
                Fejlec(rowNum, 5, "A bizonylat kelte");
                Fejlec(rowNum, 6, "A teljesítés kelte");
                Fejlec(rowNum, 7, "Ügyfél");
            }

            ++rowNum;

            decimal sumNettoFt = 0;


            foreach (var tetel in riporttetelek)
            {
                Mezo(rowNum, 0, tetel.Megnevezes);
                Mezo(rowNum, 1, tetel.Mennyiseg);
                Mezo(rowNum, 2, tetel.Me);
                Mezo(rowNum, 3, tetel.Nettoft);

                if (reszletekIs)
                {
                    ++rowNum;

                    foreach (var b in tetel.BizonylatFej)
                    {
                        Mezo(rowNum, 4, b.Bizonylatszam);
                        Mezo(rowNum, 5, b.Bizonylatkelte);
                        Mezo(rowNum, 6, b.Teljesiteskelte);
                        Mezo(rowNum, 7, b.Ugyfelnev);

                        ++rowNum;
                    }
                }

                ++rowNum;

                sumNettoFt += tetel.Nettoft;
            }

            Vegosszeg(rowNum, 3, sumNettoFt);

            AutosizeColumns(rowStart);

            Mezo(0, 0, nev);
            Mezo(1, 0,
              "A teljesítés kelte: " + teljesitesKeltetol.ToShortDateString() + " - " +
              teljesitesKelteig.ToShortDateString());

            return EndReport();
        }

        public byte[] KeszletLst(ossContext context, DateTime ezenIdopontig)
        {
            var lstDto = RiportDal.KeszletErtekNelkul(context, ezenIdopontig);

            const string nev = "Készlet";

            BeginReport(nev);

            var rowStart = 3;
            var rowNum = rowStart;

            Fejlec(rowNum, 0, "Megnevezés");
            Fejlec(rowNum, 1, "Mennyiség");
            Fejlec(rowNum, 2, "Me");
            Fejlec(rowNum, 3, "Besz. érték");
            Fejlec(rowNum, 4, "Utolsó bevét");
            Fejlec(rowNum, 5, "Utolsó ár");
            Fejlec(rowNum, 6, "Utolsó ár pénzneme");
            Fejlec(rowNum, 7, "Utolsó ár forintban");
            Fejlec(rowNum, 8, "Beszerzések száma");

            ++rowNum;

            decimal sumBeszErtek = 0;

            foreach (var dto in lstDto)
            {
                dto.Keszletertek = 0;
                dto.Utolsobevet = null;

                //a 0 készletű cikkek nincsenek a listában
                //a negatív készletnek pedig nincs értéke
                if (dto.Keszlet > 0)
                {
                    RiportDal.KeszletErteke(context, dto, ezenIdopontig);
                }

                Mezo(rowNum, 0, dto.Cikk);
                Mezo(rowNum, 1, dto.Keszlet);
                Mezo(rowNum, 2, dto.Me);
                Mezo(rowNum, 3, dto.Keszletertek);
                Mezo(rowNum, 4, dto.Utolsobevet);
                Mezo(rowNum, 5, dto.Utolsoar);
                Mezo(rowNum, 6, dto.Utolsoarpenzneme);
                Mezo(rowNum, 7, dto.Utolsoarforintban);
                Mezo(rowNum, 8, dto.Beszerzesekszama);

                ++rowNum;

                sumBeszErtek += dto.Keszletertek;
            }

            Vegosszeg(rowNum, 3, sumBeszErtek);

            AutosizeColumns(rowStart);

            Mezo(0, 0, nev);
            Mezo(1, 0, "Időpont: " + ezenIdopontig.ToShortDateString());

            return EndReport();
        }

        public byte[] PenztarTetelLst(ossContext context, int penztarKod, DateTime datumTol,
          DateTime datumIg)
        {
            var penztar = PenztarDal.Get(context, penztarKod);

            BeginReport("Pénztártételek");

            var rowStart = 3;
            var rowNum = rowStart;

            Fejlec(rowNum, 0, "Pénztárbizonylatszám");
            Fejlec(rowNum, 1, "Dátum");
            Fejlec(rowNum, 2, "Jogcím");
            Fejlec(rowNum, 3, "Ügyfél");
            Fejlec(rowNum, 4, "Bizonylatszám");
            Fejlec(rowNum, 5, "Bevétel");
            Fejlec(rowNum, 6, "Kiadás");
            Fejlec(rowNum, 7, "Megjegyzés");
            ++rowNum;

            var riporttetelek = RiportDal.PenztarTetel(context, penztarKod, datumTol, datumIg);

            foreach (var tetel in riporttetelek)
            {
                Mezo(rowNum, 0, tetel.Penztarbizonylatszam);
                Mezo(rowNum, 1, tetel.Datum);
                Mezo(rowNum, 2, tetel.Jogcim);
                Mezo(rowNum, 3, tetel.Ugyfelnev);
                Mezo(rowNum, 4, tetel.Bizonylatszam);
                Mezo(rowNum, 5, tetel.Bevetel);
                Mezo(rowNum, 6, tetel.Kiadas);
                Mezo(rowNum, 7, tetel.Megjegyzes);

                ++rowNum;
            }

            AutosizeColumns(rowStart);

            Mezo(0, 0, "Pénztártételek, " + penztar.Penztar1 + " " + penztar.Penznem);
            Mezo(1, 0,
              "Dátum: " + datumTol.ToShortDateString() + " - " + datumIg.ToShortDateString());

            return EndReport();
        }

        public byte[] ProjektLst(ossContext context, int statusz, string nev)
        {
            BeginReport("Projektek");

            var rowStart = 3;
            var rowNum = rowStart;

            Fejlec(rowNum, 0, "No");

            Fejlec(rowNum, 1, "Műszaki állapot");
            Fejlec(rowNum, 2, "Ügyfél");
            Fejlec(rowNum, 3, "Cím");
            Fejlec(rowNum, 4, "Telepítési cím");

            Fejlec(rowNum, 5, "Telefon");
            Fejlec(rowNum, 6, "Email");

            Fejlec(rowNum, 7, "A projekt jellege");
            Fejlec(rowNum, 8, "Inverter");
            Fejlec(rowNum, 9, "");
            Fejlec(rowNum, 10, "Napelem");
            Fejlec(rowNum, 11, "");
            Fejlec(rowNum, 12, "Méret, kW");
            ++rowNum;

            var riporttetelek = RiportDal.Projekt(context, statusz);

            foreach (var tetel in riporttetelek)
            {
                Mezo(rowNum, 0, tetel.Projektkod);

                Mezo(rowNum, 1, tetel.Muszakiallapot);
                Mezo(rowNum, 2, tetel.UgyfelkodNavigation.Nev);
                Mezo(rowNum, 3, UgyfelBll.Cim(tetel.UgyfelkodNavigation));
                Mezo(rowNum, 4, tetel.Telepitesicim);

                Mezo(rowNum, 5, tetel.UgyfelkodNavigation.Telefon);
                Mezo(rowNum, 6, tetel.UgyfelkodNavigation.Email);

                Mezo(rowNum, 7, tetel.Projektjellege);
                Mezo(rowNum, 8, tetel.Inverter);
                Mezo(rowNum, 9, tetel.Inverterallapot);
                Mezo(rowNum, 10, tetel.Napelem);
                Mezo(rowNum, 11, tetel.Napelemallapot);
                Mezo(rowNum, 12, tetel.Dckw);

                ++rowNum;
            }

            AutosizeColumns(rowStart);

            Mezo(0, 0, "Projektek: " + nev);
            Mezo(1, 0, "Dátum: " + DateTime.Now.Date.ToShortDateString());

            return EndReport();
        }



        private ExcelFile _excel;
        private ExcelWorksheet _sheet;

        private void BeginReport(string riportName)
        {
            _excel = new ExcelFile();
            _sheet = _excel.Worksheets.Add(riportName);
        }

        private byte[] EndReport()
        {
            byte[] result;

            using (var xlsstream = new MemoryStream())
            {
                _excel.Save(xlsstream, SaveOptions.XlsDefault);
                result = xlsstream.ToArray();
            }

            return result;
        }
    }
}
