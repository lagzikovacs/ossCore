using GemBox.Document;
using GemBox.Document.Tables;
using ossServer.Enums;
using ossServer.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ossServer.Controllers.BizonylatNyomtatas
{
    public class BizonylatPrinter
    {
        private readonly DocumentModel _outputDoc;

        public BizonylatPrinter()
        {
            ComponentInfo.SetLicense("D873-P6FI-T8I0-4WLW");

            _outputDoc = new DocumentModel();
        }

        public BizonylatTipus BizonylatTipus { get; set; }
        public string BizonylatFejlec { get; set; }
        public string Peldany { get; set; }
        public string EredetiVMasolat { get; set; }

        public string SzallitoNev { get; set; }
        public string SzallitoCim { get; set; }
        public string SzallitoAdoszam { get; set; }
        public string SzallitoBankszamla1 { get; set; }
        public string SzallitoBankszamla2 { get; set; }

        public string VevoNev { get; set; }
        public string VevoCim { get; set; }
        public string VevoAdoszam { get; set; }

        public string BizonylatKelte { get; set; }
        public string TeljesitesKelte { get; set; }
        public string FizetesiMod { get; set; }
        public string FizetesiHatarido { get; set; }
        public string Bizonylatszam { get; set; }

        public string MegjegyzesFej { get; set; }

        public string Netto { get; set; }
        public string Afa { get; set; }
        public string Brutto { get; set; }
        public string Termekdij { get; set; }

        public List<BizonylatPrinterTetel> LstTetel { get; } = new List<BizonylatPrinterTetel>();
        public List<BizonylatPrinterAfa> LstAfa { get; } = new List<BizonylatPrinterAfa>();
        public List<BizonylatPrinterTermekdij> LstTermekdij { get; } = new List<BizonylatPrinterTermekdij>();

        public string Penznem { get; set; }
        public string Arfolyam { get; set; }
        public string Azaz { get; set; }

        public bool TermekdijNulla { get; set; }
        public string AfaFt { get; set; }

        public string Keszitette { get; set; }
        public string Verzio { get; set; }

        public byte[] Szamlakep { get; set; }

        public void UjPeldany(string peldany, string eredetiVMasolat)
        {
            DocumentModel document;
            using (var msDocx = new MemoryStream())
            {
                msDocx.Write(Szamlakep, 0, Szamlakep.Count());
                document = DocumentModel.Load(msDocx, LoadOptions.DocxDefault);
            }

            var fejMezok = new
            {
                BizonylatFejlec,

                Peldany = peldany,
                EredetiVMasolat = eredetiVMasolat,

                SzallitoNev,
                SzallitoCim,
                SzallitoAdoszam,
                SzallitoBankszamla1,
                SzallitoBankszamla2,

                VevoNev,
                VevoCim,
                VevoAdoszam,

                BizonylatKelte,
                TeljesitesKelte,
                FizetesiMod,
                FizetesiHatarido,
                Bizonylatszam,

                MegjegyzesFej,

                Brutto,
                Penznem,
                Arfolyam,
                Azaz,

                AfaFt,

                Keszitette,

                Verzio
            };
            document.MailMerge.Execute(fejMezok);

            Table[] Tables = document.GetChildElements(true, ElementType.Table).Cast<Table>().ToArray();

            Table TetelTable = Tables[1];
            TableRow TetelMinta = TetelTable.Rows[1].Clone(true);
            TableRow TetelMintaMegjegyzes = TetelTable.Rows[2].Clone(true);
            TetelTable.Rows.RemoveAt(2);
            TetelTable.Rows.RemoveAt(1);

            Table AfaTable = Tables[2];
            TableRow AfaMinta = AfaTable.Rows[1].Clone(true);
            AfaTable.Rows.RemoveAt(1);

            Table OsszesitoTable = Tables[3];
            if (Penznem == "HUF" || (BizonylatTipus != BizonylatTipus.Szamla && BizonylatTipus != BizonylatTipus.ElolegSzamla))
                OsszesitoTable.Rows.RemoveAt(2);

            TableRow LastRow;

            foreach (var Tetel in LstTetel)
            {
                TetelTable.Rows.Add(TetelMinta.Clone(true));
                LastRow = TetelTable.Rows.Last();

                DocxUtils.SetCellValue(LastRow, 0, Tetel.Megnevezes);
                DocxUtils.SetCellValue(LastRow, 1, Tetel.Mennyiseg);
                DocxUtils.SetCellValue(LastRow, 2, Tetel.Me);
                DocxUtils.SetCellValue(LastRow, 3, Tetel.Egysegar);
                DocxUtils.SetCellValue(LastRow, 4, Tetel.AfaKulcs);
                DocxUtils.SetCellValue(LastRow, 5, Tetel.Netto);
                DocxUtils.SetCellValue(LastRow, 6, Tetel.Afa);
                DocxUtils.SetCellValue(LastRow, 7, Tetel.Brutto);

                if (!string.IsNullOrEmpty(Tetel.Megjegyzes))
                {
                    TetelTable.Rows.Add(TetelMintaMegjegyzes.Clone(true));
                    LastRow = TetelTable.Rows.Last();

                    DocxUtils.SetCellValue(LastRow, 1, Tetel.Megjegyzes);
                }

                if (!string.IsNullOrEmpty(Tetel.Termekdij) &&
                  (BizonylatTipus == BizonylatTipus.Szamla || BizonylatTipus == BizonylatTipus.ElolegSzamla || BizonylatTipus == BizonylatTipus.Szallito))
                {
                    TetelTable.Rows.Add(TetelMintaMegjegyzes.Clone(true));
                    LastRow = TetelTable.Rows.Last();

                    DocxUtils.SetCellValue(LastRow, 1, Tetel.Termekdij);
                }
            }

            LastRow = TetelTable.Rows.Last();

            DocxUtils.SetCellBorder(TetelTable.Rows[0], MultipleBorderTypes.Bottom, 0, 7);
            DocxUtils.SetCellBorder(LastRow, MultipleBorderTypes.Bottom, 0, LastRow.Cells.Count - 1);

            foreach (var afa in LstAfa)
            {
                AfaTable.Rows.Add(AfaMinta.Clone(true));
                LastRow = AfaTable.Rows.Last();

                DocxUtils.SetCellValue(LastRow, 0, afa.AfaKulcs);
                DocxUtils.SetCellValue(LastRow, 1, afa.Netto);
                DocxUtils.SetCellValue(LastRow, 2, afa.Afa);
                DocxUtils.SetCellValue(LastRow, 3, afa.Brutto);
            }

            AfaTable.Rows.Add(AfaMinta.Clone(true));
            LastRow = AfaTable.Rows.Last();

            DocxUtils.SetCellValue(LastRow, 0, "");
            DocxUtils.SetCellValue(LastRow, 1, Netto);
            DocxUtils.SetCellValue(LastRow, 2, Afa);
            DocxUtils.SetCellValue(LastRow, 3, Brutto);

            DocxUtils.SetCellBorder(AfaTable.Rows[0], MultipleBorderTypes.Bottom, 0, 3);
            DocxUtils.SetCellBorder(LastRow, MultipleBorderTypes.Top, 0, 3);

            Table termekdijTable = Tables[4];
            TableRow termekdijMinta = termekdijTable.Rows[1].Clone(true);
            termekdijTable.Rows.RemoveAt(1);
            if (!TermekdijNulla &&
                (BizonylatTipus == BizonylatTipus.Szamla || BizonylatTipus == BizonylatTipus.ElolegSzamla || BizonylatTipus == BizonylatTipus.Szallito))
            {
                foreach (var termekdij in LstTermekdij)
                {
                    termekdijTable.Rows.Add(termekdijMinta.Clone(true));
                    LastRow = termekdijTable.Rows.Last();

                    DocxUtils.SetCellValue(LastRow, 0, termekdij.TermekdijKT);
                    DocxUtils.SetCellValue(LastRow, 1, termekdij.OssztomegKg);
                    DocxUtils.SetCellValue(LastRow, 2, termekdij.TermekdijEgysegar);
                    DocxUtils.SetCellValue(LastRow, 3, termekdij.Termekdij);
                }

                termekdijTable.Rows.Add(termekdijMinta.Clone(true));
                LastRow = termekdijTable.Rows.Last();

                DocxUtils.SetCellValue(LastRow, 0, "");
                DocxUtils.SetCellValue(LastRow, 1, "");
                DocxUtils.SetCellValue(LastRow, 2, "");
                DocxUtils.SetCellValue(LastRow, 3, Termekdij);

                DocxUtils.SetCellBorder(termekdijTable.Rows[0], MultipleBorderTypes.Bottom, 0, 3);
                DocxUtils.SetCellBorder(LastRow, MultipleBorderTypes.Top, 0, 3);
            }
            else
                termekdijTable.Rows.RemoveAt(0);

            var mapping = new ImportMapping(document, _outputDoc, false);
            foreach (var sourceSection in document.Sections)
            {
                var destinationSection = _outputDoc.Import(sourceSection, true, mapping);
                _outputDoc.Sections.Add(destinationSection);
            }
        }

        public byte[] Print()
        {
            byte[] result;

            using (var msPdf = new MemoryStream())
            {
                _outputDoc.Save(msPdf, SaveOptions.DocxDefault);
                result = new byte[msPdf.Length];
                msPdf.Read(result, 0, (int)msPdf.Length);
            }

            return result;
        }

        public void Setup(Models.Bizonylat entityBizonylat, byte[] szamlakep, string fejlec, string verzio)
        {
            Szamlakep = szamlakep;
            BizonylatFejlec = fejlec;
            Verzio = verzio;

            BizonylatTipus = (BizonylatTipus)entityBizonylat.Bizonylattipuskod;

            var kimeno = BizonylatTipus != BizonylatTipus.BejovoSzamla &&
                         BizonylatTipus != BizonylatTipus.Megrendeles;

            var szallitocim = $"{entityBizonylat.Szallitoiranyitoszam} {entityBizonylat.Szallitohelysegnev}, {entityBizonylat.Szallitoutcahazszam}";
            var szallitoadoszam = $"{entityBizonylat.Szallitoadotorzsszam}-{entityBizonylat.Szallitoadoafakod}-{entityBizonylat.Szallitoadomegyekod}";

            var vevocim = $"{entityBizonylat.Ugyfeliranyitoszam} {entityBizonylat.Ugyfelhelysegnev}, {entityBizonylat.Ugyfelkozterulet} {entityBizonylat.Ugyfelkozterulettipus} {entityBizonylat.Ugyfelhazszam}";

            SzallitoNev = kimeno ? entityBizonylat.Szallitonev : entityBizonylat.Ugyfelnev;
            SzallitoCim = kimeno ? szallitocim : vevocim;
            SzallitoAdoszam = kimeno ? szallitoadoszam : entityBizonylat.Ugyfeladoszam;
            SzallitoBankszamla1 = kimeno ? entityBizonylat.Szallitobankszamla1 : "";
            SzallitoBankszamla2 = kimeno ? entityBizonylat.Szallitobankszamla2 : "";

            VevoNev = kimeno ? entityBizonylat.Ugyfelnev : entityBizonylat.Szallitonev;
            VevoCim = kimeno ? vevocim : szallitocim;
            VevoAdoszam = kimeno ? entityBizonylat.Ugyfeladoszam : szallitoadoszam;

            BizonylatKelte = entityBizonylat.Bizonylatkelte.ToShortDateString();
            TeljesitesKelte = entityBizonylat.Teljesiteskelte.ToShortDateString();
            FizetesiMod = entityBizonylat.Fizetesimod;
            FizetesiHatarido = entityBizonylat.Fizetesihatarido.ToShortDateString();
            Bizonylatszam = entityBizonylat.Bizonylatszam;

            MegjegyzesFej = entityBizonylat.Megjegyzesfej;

            Netto = entityBizonylat.Netto.ToString("#,##0.00");
            Afa = entityBizonylat.Afa.ToString("#,##0.00");
            Brutto = entityBizonylat.Brutto.ToString("#,##0.00");
            Termekdij = entityBizonylat.Termekdij.ToString("#,##0.00");

            Penznem = entityBizonylat.Penznem;
            Arfolyam = entityBizonylat.Arfolyam.ToString("#,##0.00");
            Azaz = entityBizonylat.Azaz;

            TermekdijNulla = entityBizonylat.Termekdij == 0;
            AfaFt = Calc.RealRound(entityBizonylat.Afa * entityBizonylat.Arfolyam, 1).ToString("#,##0.00");

            Keszitette = entityBizonylat.Letrehozta;

            LstTetel.Clear();
            foreach (var tetel in entityBizonylat.Bizonylattetel)
            {
                var t = new BizonylatPrinterTetel
                {
                    Megnevezes = tetel.Megnevezes,
                    Megjegyzes = tetel.Megjegyzes,
                    Mennyiseg = tetel.Mennyiseg.ToString("#,##0.00"),
                    Me = tetel.Me,
                    Egysegar = tetel.Egysegar.ToString("#,##0.00"),

                    AfaKulcs = tetel.Afakulcs,
                    Netto = tetel.Netto.ToString("#,##0.00"),
                    Afa = tetel.Afa.ToString("#,##0.00"),
                    Brutto = tetel.Brutto.ToString("#,##0.00"),
                };

                if (tetel.Termekdijas)
                    t.Termekdij =
                      $"KT {tetel.Termekdijkt} termékdíj a bruttó árból: {tetel.Mennyiseg:#,##0.00} {tetel.Me} x {tetel.Tomegkg:#,##0.00} kg x {tetel.Termekdijegysegar:#,##0.00} HUF/kg = {tetel.Termekdij:#,##0.00} HUF";

                LstTetel.Add(t);
            }

            LstAfa.Clear();
            foreach (var afa in entityBizonylat.Bizonylatafa)
                LstAfa.Add(new BizonylatPrinterAfa
                {
                    AfaKulcs = afa.Afakulcs,
                    Netto = afa.Netto.ToString("#,##0.00"),
                    Afa = afa.Afa.ToString("#,##0.00"),
                    Brutto = afa.Brutto.ToString("#,##0.00"),
                });

            LstTermekdij.Clear();
            foreach (var termekdij in entityBizonylat.Bizonylattermekdij)
            {
                LstTermekdij.Add(new BizonylatPrinterTermekdij
                {
                    TermekdijKT = termekdij.Termekdijkt,
                    OssztomegKg = termekdij.Ossztomegkg.ToString("#,##0.00"),
                    TermekdijEgysegar = termekdij.Termekdijegysegar.ToString("#,##0.00"),
                    Termekdij = termekdij.Termekdij.ToString("#,##0.00"),
                });
            }
            
        }
    }
}
