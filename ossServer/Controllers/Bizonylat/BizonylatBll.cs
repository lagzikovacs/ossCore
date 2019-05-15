using ossServer.Controllers.Csoport;
using ossServer.Controllers.Onlineszamla;
using ossServer.Controllers.Particio;
using ossServer.Controllers.Primitiv.Penznem;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatBll
    {
        public static BizonylatTipusLeiro[] Bl =
        {
          new BizonylatTipusLeiro(),
          new BizonylatTipusLeiro
          {
            BizonylatTipus = BizonylatTipus.Szamla,
            BizonylatNev = "Számla",
            BizonylatFejlec = "Számla",
            KodNev = KodNev.Szamla,
            GenBizonylatszam = true,
            Stornozhato = true,
            JogKod = JogKod.SZAMLA,
            FizetesiModIs = true,
            Elotag = ""
          },
          new BizonylatTipusLeiro
          {
            BizonylatTipus = BizonylatTipus.Szallito,
            BizonylatNev = "Szállító",
            BizonylatFejlec = "Szállítólevél",
            KodNev = KodNev.Szallito,
            GenBizonylatszam = true,
            Stornozhato = false,
            JogKod = JogKod.SZALLITO,
            FizetesiModIs = false,
            Elotag = ""
          },
          new BizonylatTipusLeiro
          {
            BizonylatTipus = BizonylatTipus.BejovoSzamla,
            BizonylatNev = "Bejövő számla",
            BizonylatFejlec = "Bejövő számla másolat",
            KodNev = KodNev.BejovoSzamla,
            GenBizonylatszam = false,
            Stornozhato = false,
            JogKod = JogKod.BEJOVOSZAMLA,
            FizetesiModIs = true,
            Elotag = ""
          },
          new BizonylatTipusLeiro
          {
            BizonylatTipus = BizonylatTipus.Megrendeles,
            BizonylatNev = "Megrendelés",
            BizonylatFejlec = "Megrendelés",
            KodNev = KodNev.Megrendeles,
            GenBizonylatszam = true,
            Stornozhato = true,
            JogKod = JogKod.MEGRENDELES,
            FizetesiModIs = true,
            Elotag = ""
          },
          new BizonylatTipusLeiro(),
          new BizonylatTipusLeiro(),
          new BizonylatTipusLeiro
          {
            BizonylatTipus = BizonylatTipus.DijBekero,
            BizonylatNev = "Díjbekérő",
            BizonylatFejlec = "Díjbekérő",
            KodNev = KodNev.DijBekero,
            GenBizonylatszam = true,
            Stornozhato = true,
            JogKod = JogKod.DIJBEKERO,
            FizetesiModIs = true,
            Elotag = "DB"
          },
          new BizonylatTipusLeiro
          {
            BizonylatTipus = BizonylatTipus.ElolegSzamla,
            BizonylatNev = "Előlegszámla",
            BizonylatFejlec = "Előlegszámla",
            KodNev = KodNev.ElolegSzamla,
            GenBizonylatszam = true,
            Stornozhato = true,
            JogKod = JogKod.ELOLEGSZAMLA,
            FizetesiModIs = true,
            Elotag = "E"
          }
        };

        public static BizonylatTipusLeiro BizonylatLeiro(ossContext context, string sid, 
            BizonylatTipus bizonylatTipus)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLAT);

            return Bl[bizonylatTipus.GetHashCode()];
        }

        public static BizonylatComplexDto CreateNewComplex(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            var particio = ParticioDal.Get(context);

            var result = new BizonylatComplexDto
            {
                Dto = new BizonylatDto
                {
                    Szallitonev = particio.SzallitoNev,
                    Szallitoiranyitoszam = particio.SzallitoIranyitoszam,
                    Szallitohelysegnev = particio.SzallitoHelysegnev,
                    Szallitoutcahazszam = particio.SzallitoUtcahazszam,
                    Szallitobankszamla1 = particio.SzallitoBankszamla1,
                    Szallitobankszamla2 = particio.SzallitoBankszamla2,
                    Szallitoadotorzsszam = particio.SzallitoAdotorzsszam,
                    Szallitoadoafakod = particio.SzallitoAdoafakod,
                    Szallitoadomegyekod = particio.SzallitoAdomegyekod,

                    Bizonylatkelte = DateTime.Today,
                    Teljesiteskelte = DateTime.Today,
                    Fizetesihatarido = DateTime.Today,
                    Szallitasihatarido = DateTime.Today,
                    Kifizetesrendben = false,
                    Kiszallitva = false,
                    Arfolyam = 1,
                    Ezstornozott = false,
                    Ezstornozo = false,

                    Netto = 0,
                    Afa = 0,
                    Brutto = 0,

                    Termekdij = 0,

                    Nyomtatottpeldanyokszama = 0
                }
            };

            const string penznem = "HUF";
            var lstPenznem = PenznemDal.Read(context, penznem);
            if (lstPenznem.Count == 1)
            {
                result.Dto.Penznemkod = lstPenznem[0].Penznemkod;
                result.Dto.Penznem = penznem;
            }

            result.LstTetelDto = new List<BizonylatTetelDto>();
            result.LstAfaDto = new List<BizonylatAfaDto>();
            result.LstTermekdijDto = new List<BizonylatTermekdijDto>();

            return result;
        }

        private static BizonylatDto CalcCim(BizonylatDto dto)
        {
            dto.Ugyfelcim = $"{dto.Ugyfeliranyitoszam} {dto.Ugyfelhelysegnev}, {dto.Ugyfelkozterulet} {dto.Ugyfelkozterulettipus} {dto.Ugyfelhazszam}";

            return dto;
        }

        public static BizonylatDto Get(ossContext context, string sid, int bizonylatKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLAT);

            var entity = BizonylatDal.Get(context, bizonylatKod);
            var dto = ObjectUtils.Convert<Models.Bizonylat, BizonylatDto>(entity);

            return CalcCim(dto);
        }

        public static BizonylatComplexDto GetComplex(ossContext context, string sid, int bizonylatKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLAT);

            var entitycomplex = BizonylatDal.GetComplex(context, bizonylatKod);
            var dto = ObjectUtils.Convert<Models.Bizonylat, BizonylatDto>(entitycomplex);

            var result = new BizonylatComplexDto
            {
                Dto = CalcCim(dto),
                LstTetelDto = new List<BizonylatTetelDto>(),
                LstAfaDto = new List<BizonylatAfaDto>(),
                LstTermekdijDto = new List<BizonylatTermekdijDto>()
            };

            foreach (var bt in entitycomplex.Bizonylattetel.OrderBy(s => s.Bizonylattetelkod))
                result.LstTetelDto.Add(ObjectUtils.Convert<Models.Bizonylattetel, BizonylatTetelDto>(bt));
            foreach (var ba in entitycomplex.Bizonylatafa.OrderBy(s => s.Afakulcs))
                result.LstAfaDto.Add(ObjectUtils.Convert<Models.Bizonylatafa, BizonylatAfaDto>(ba));
            foreach (var btd in entitycomplex.Bizonylattermekdij.OrderBy(s => s.Termekdijkt))
                result.LstTermekdijDto.Add(ObjectUtils.Convert<Models.Bizonylattermekdij, BizonylatTermekdijDto>(btd));

            return result;
        }

        public static void Delete(ossContext context, string sid, BizonylatDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            BizonylatDal.Lock(context, dto.Bizonylatkod, dto.Modositva);
            var entity = BizonylatDal.Get(context, dto.Bizonylatkod);
            BizonylatDal.Delete(context, entity);
        }

        public static List<BizonylatDto> Select(ossContext context, string sid, int rekordTol, int lapMeret, 
            BizonylatTipus bizonylatTipus, List<SzMT> szmt, out int osszesRekord)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLAT);

            var qry = BizonylatDal.GetQuery(context, bizonylatTipus, szmt);
            osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
            var result = ObjectUtils.Convert<Models.Bizonylat, BizonylatDto>(entities);
            foreach (var r in result)
                CalcCim(r);

            return result;
        }




        private static string GenerateBizonylatszam(ossContext context, int bizonylattipusKod)
        {
            return Bl[bizonylattipusKod].Elotag +
                context.KodGen(Bl[bizonylattipusKod].KodNev).ToString("00000");
        }

        public static int Save(ossContext context, string sid, BizonylatComplexDto complexDto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            if (!string.IsNullOrEmpty(complexDto.Dto.Bizonylatszam))
                throw new Exception($"Ez a bizonylat már nem módosítható: {complexDto.Dto.Bizonylatszam}!");

            foreach (var l in complexDto.LstTetelDto)
                BizonylatUtils.BizonylattetelCalc(l);
            BizonylatUtils.SumEsAfaEsTermekdij(complexDto.Dto, complexDto.LstTetelDto, complexDto.LstAfaDto, complexDto.LstTermekdijDto);

            Models.Bizonylat entity;

            int bizonylatKod;

            if (complexDto.Dto.Bizonylatkod == 0)
            {
                //a fej még nem volt soha kiírva
                entity = ObjectUtils.Convert<BizonylatDto, Models.Bizonylat>(complexDto.Dto);
                bizonylatKod = BizonylatDal.Add(context, entity);
            }
            else
            {
                //updateelni a fejet
                bizonylatKod = complexDto.Dto.Bizonylatkod;
                BizonylatDal.Lock(context, bizonylatKod, complexDto.Dto.Modositva);
                entity = BizonylatDal.Get(context, bizonylatKod);
                ObjectUtils.Update(complexDto.Dto, entity);
                BizonylatDal.Update(context, entity);

                //törölni az esetleges létező tételt-áfát-
                var entitesTetel = BizonylatTetelDal.Select(context, bizonylatKod);
                foreach (var l in entitesTetel)
                    BizonylatTetelDal.Delete(context, l);

                var entitesAfa = BizonylatAfaDal.Select(context, bizonylatKod);
                foreach (var l in entitesAfa)
                    BizonylatAfaDal.Delete(context, l);

                var entitesTermekdij = BizonylatTermekdijDal.Select(context, bizonylatKod);
                foreach (var l in entitesTermekdij)
                    BizonylatTermekdijDal.Delete(context, l);
            }

            //beírni a bizonylatkódot a tételbe-áfába-termékdíjba
            foreach (var l in complexDto.LstTetelDto)
            {
                l.Bizonylatkod = bizonylatKod;

                var entityTetel = ObjectUtils.Convert<BizonylatTetelDto, Models.Bizonylattetel>(l);
                BizonylatTetelDal.Add(context, entityTetel);
            }
            foreach (var l in complexDto.LstAfaDto)
            {
                l.Bizonylatkod = bizonylatKod;

                var entityAfa = ObjectUtils.Convert<BizonylatAfaDto, Models.Bizonylatafa>(l);
                BizonylatAfaDal.Add(context, entityAfa);
            }
            foreach (var l in complexDto.LstTermekdijDto)
            {
                l.Bizonylatkod = bizonylatKod;

                var entityTermekdij = ObjectUtils.Convert<BizonylatTermekdijDto, Models.Bizonylattermekdij>(l);
                BizonylatTermekdijDal.Add(context, entityTermekdij);
            }

            return bizonylatKod;
        }

        public static int UjBizonylatMintaAlapjan(ossContext context, string sid, 
            int bizonylatKod, BizonylatTipus bizonylatTipus)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            var entity = BizonylatDal.GetComplex(context, bizonylatKod);

            var complexDto = new BizonylatComplexDto
            {
                Dto = ObjectUtils.Convert<Models.Bizonylat, BizonylatDto>(entity),
                LstTetelDto = new List<BizonylatTetelDto>(),
                LstAfaDto = new List<BizonylatAfaDto>(),
                LstTermekdijDto = new List<BizonylatTermekdijDto>()
            };
            complexDto.Dto.Bizonylatkod = 0;
            complexDto.Dto.Bizonylattipuskod = bizonylatTipus.GetHashCode();
            complexDto.Dto.Bizonylatszam = null;
            complexDto.Dto.Nyomtatottpeldanyokszama = 0;

            foreach (var le in entity.Bizonylattetel)
            {
                var l = ObjectUtils.Convert<Models.Bizonylattetel, BizonylatTetelDto>(le);
                l.Bizonylatkod = 0;

                complexDto.LstTetelDto.Add(l);
            }

            //Save: az Áfa tételek törlődnek és újraszámítódnak
            return Save(context, sid, complexDto);
        }

        public static int Storno(ossContext context, string sid, BizonylatDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            BizonylatDal.Lock(context, dto.Bizonylatkod, dto.Modositva);

            //a fej/tételek/áfa lekérése/módosítása/hozzáadása lépésenként
            var stornozando = BizonylatDal.Get(context, dto.Bizonylatkod);
            if (stornozando.Bizonylatszam == null)
                throw new Exception("Lezáratlan bizonylatot nem lehet stornozni!");
            if (stornozando.Ezstornozott)
                throw new Exception("A bizonylat már stornozott!");

            //ha díjbekérő -> nem jön létre másik bizonylat
            if (stornozando.Bizonylattipuskod == (int)BizonylatTipus.DijBekero)
            {
                stornozando.Ezstornozott = true;
                BizonylatDal.Update(context, stornozando);

                return 0;
            }

            var stornozo = ObjectUtils.Clone(stornozando);

            stornozo.Bizonylatszam = GenerateBizonylatszam(context, stornozo.Bizonylattipuskod);
            stornozo.Megjegyzesfej = $"A(z) {stornozando.Bizonylatszam} számú bizonylat stornója.";
            stornozo.Netto = -stornozo.Netto;
            stornozo.Afa = -stornozo.Afa;
            stornozo.Brutto = -stornozo.Brutto;
            stornozo.Termekdij = -stornozo.Termekdij;
            stornozo.Ezstornozo = true;
            stornozo.Stornozottbizonylatkod = stornozando.Bizonylatkod;
            stornozo.Bizonylatkelte = DateTime.Today;
            // Stornónál a teljesítés kelte és a fizetési határidő ugyanaz kell hogy maradjon!
            stornozo.Azaz = Azaz.Szovegge(stornozo.Brutto);

            BizonylatDal.Add(context, stornozo);

            //a tételek lekérése/másolása/módosítása/hozzáadása lépésenként
            var stornozoTetel = BizonylatTetelDal.Select(context, dto.Bizonylatkod);
            foreach (var t in stornozoTetel)
            {
                var tetel = ObjectUtils.Clone(t);

                tetel.Bizonylatkod = stornozo.Bizonylatkod;
                tetel.Mennyiseg = -tetel.Mennyiseg;
                tetel.Netto = -tetel.Netto;
                tetel.Afa = -tetel.Afa;
                tetel.Brutto = -tetel.Brutto;

                tetel.Ossztomegkg = -tetel.Ossztomegkg;
                tetel.Termekdij = -tetel.Termekdij;

                BizonylatTetelDal.Add(context, tetel);
            }

            //az áfa lekérése/másolása/módosítása/hozzáadása lépésenként
            var stornozoAfa = BizonylatAfaDal.Select(context, dto.Bizonylatkod);
            foreach (var t in stornozoAfa)
            {
                var afa = ObjectUtils.Clone(t);

                afa.Bizonylatkod = stornozo.Bizonylatkod;
                afa.Netto = -afa.Netto;
                afa.Afa = -afa.Afa;
                afa.Brutto = -afa.Brutto;

                BizonylatAfaDal.Add(context, afa);
            }

            //termékdíj másolása
            var stornozotermekdij = BizonylatTermekdijDal.Select(context, dto.Bizonylatkod);
            foreach (var t in stornozotermekdij)
            {
                var termekdij = ObjectUtils.Clone(t);

                termekdij.Bizonylatkod = stornozo.Bizonylatkod;
                termekdij.Ossztomegkg = -termekdij.Ossztomegkg;
                termekdij.Termekdij = -termekdij.Termekdij;

                BizonylatTermekdijDal.Add(context, termekdij);
            }

            //ellenőrizni a tartalmat
            // TODO
            //var invoice = OnlineszamlaBll.GetInvoice(context, stornozo.Bizonylatkod);
            //var hibak = invoice.ValidateErrors();
            //if (hibak.Any())
            //    throw new Exception(string.Join(Environment.NewLine, hibak));

            //hozzáadni a feltöltendők listájához
            OnlineszamlaDal.Add(context, stornozo.Bizonylatkod);


            //az eredetiben ennyi módosítás
            stornozando.Ezstornozott = true;
            stornozando.Stornozobizonylatkod = stornozo.Bizonylatkod;
            BizonylatDal.Update(context, stornozando);

            return stornozo.Bizonylatkod;
        }

        public static int Kibocsatas(ossContext context, string sid, 
            BizonylatDto dto, string bizonylatszam)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            BizonylatDal.Lock(context, dto.Bizonylatkod, dto.Modositva);
            var entity = BizonylatDal.Get(context, dto.Bizonylatkod);
            if (entity.Bizonylatszam != null)
                throw new Exception("Ez a bizonylat már ki van bocsátva!");

            if (string.IsNullOrEmpty(dto.Ugyfelnev) ||
                string.IsNullOrEmpty(dto.Ugyfeliranyitoszam) || string.IsNullOrEmpty(dto.Ugyfelhelysegnev) ||
                string.IsNullOrEmpty(dto.Ugyfelkozterulet) || string.IsNullOrEmpty(dto.Ugyfelkozterulettipus) ||
                string.IsNullOrEmpty(dto.Ugyfelhazszam))
                throw new Exception("Hiányzó ügyféladatok!");

            if (dto.Penznem != "HUF" & dto.Arfolyam == 1)
                throw new Exception("Hibás az árfolyam!");

            if (Bl[dto.Bizonylattipuskod].FizetesiModIs & string.IsNullOrEmpty(dto.Fizetesimod))
                throw new Exception("Hiányzó fizetési mód!");

            if (Bl[entity.Bizonylattipuskod].GenBizonylatszam)
                entity.Bizonylatszam = GenerateBizonylatszam(context, entity.Bizonylattipuskod);
            else
                entity.Bizonylatszam = bizonylatszam ?? throw new Exception("A bizonylatszám nem lehet üres!");
            var result = BizonylatDal.Update(context, entity);

            //ha számla és előlegszámla
            if ((BizonylatTipus)entity.Bizonylattipuskod == BizonylatTipus.Szamla ||
                (BizonylatTipus)entity.Bizonylattipuskod == BizonylatTipus.ElolegSzamla)
            {
                //ellenőrizni a tartalmat
                // TODO
                //var invoice = OnlineszamlaBll.GetInvoice(context, entity.BIZONYLATKOD);
                //var hibak = invoice.ValidateErrors();
                //if (hibak.Any())
                //    throw new Exception(string.Join(Environment.NewLine, hibak));

                //hozzáadni a feltöltendők listájához
                OnlineszamlaDal.Add(context, entity.Bizonylatkod);
            }

            return result;
        }

        public static int KifizetesRendben(ossContext context, string sid, BizonylatDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            BizonylatDal.Lock(context, dto.Bizonylatkod, dto.Modositva);
            var entity = BizonylatDal.Get(context, dto.Bizonylatkod);
            if (entity.Kifizetesrendben == true) //null miatt
                entity.Kifizetesrendben = false;
            else
                entity.Kifizetesrendben = true;
            return BizonylatDal.Update(context, entity);
        }

        public static int Kiszallitva(ossContext context, string sid, BizonylatDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            BizonylatDal.Lock(context, dto.Bizonylatkod, dto.Modositva);
            var entity = BizonylatDal.Get(context, dto.Bizonylatkod);
            if (entity.Kiszallitva == true) //null miatt
                entity.Kiszallitva = false;
            else
                entity.Kiszallitva = true;
            return BizonylatDal.Update(context, entity);
        }

        public static int GetBizonylatEredetiPeldany(ossContext context)
        {
            var entityParticio = ParticioDal.Get(context);
            var result = entityParticio.BizonylatEredetipeldanyokSzama ?? throw new Exception(string.Format(Messages.ParticioHiba, "BizonylatEredetipeldanyokSzama"));

            if (result <= 0 || result > 3)
                throw new Exception($"BizonylatEredetipeldanyokSzama: Hibás érték, legyen 1, 2 vagy 3, most {result} !");

            return result;
        }

        public static int GetBizonylatMasolat(ossContext context)
        {
            var entityParticio = ParticioDal.Get(context);
            var result = entityParticio.BizonylatMasolatokSzama ?? throw new Exception(string.Format(Messages.ParticioHiba, "BizonylatMasolatokSzama"));

            if (result <= 0 || result > 3)
                throw new Exception($"BizonylatMasolatokSzama: Hibás érték, legyen 1, 2 vagy 3, most {result} !");

            return result;
        }

        //internal static void BizonylatPrinterSetup(BizonylatPrinter printer, BIZONYLAT entityBizonylat,
        //  byte[] szamlakep)
        //{
        //    printer.BizonylatTipus = (BizonylatTipus)entityBizonylat.BIZONYLATTIPUSKOD;

        //    var kimeno = printer.BizonylatTipus != BizonylatTipus.BejovoSzamla &&
        //                 printer.BizonylatTipus != BizonylatTipus.Megrendeles;

        //    var szallitocim = $"{entityBizonylat.SZALLITOIRANYITOSZAM} {entityBizonylat.SZALLITOHELYSEGNEV}, {entityBizonylat.SZALLITOUTCAHAZSZAM}";
        //    var szallitoadoszam = $"{entityBizonylat.SZALLITOADOTORZSSZAM}-{entityBizonylat.SZALLITOADOAFAKOD}-{entityBizonylat.SZALLITOADOMEGYEKOD}";

        //    var vevocim = $"{entityBizonylat.UGYFELIRANYITOSZAM} {entityBizonylat.UGYFELHELYSEGNEV}, {entityBizonylat.UGYFELKOZTERULET} {entityBizonylat.UGYFELKOZTERULETTIPUS} {entityBizonylat.UGYFELHAZSZAM}";

        //    printer.SzallitoNev = kimeno ? entityBizonylat.SZALLITONEV : entityBizonylat.UGYFELNEV;
        //    printer.SzallitoCim = kimeno ? szallitocim : vevocim;
        //    printer.SzallitoAdoszam = kimeno ? szallitoadoszam : entityBizonylat.UGYFELADOSZAM;
        //    printer.SzallitoBankszamla1 = kimeno ? entityBizonylat.SZALLITOBANKSZAMLA1 : "";
        //    printer.SzallitoBankszamla2 = kimeno ? entityBizonylat.SZALLITOBANKSZAMLA2 : "";

        //    printer.VevoNev = kimeno ? entityBizonylat.UGYFELNEV : entityBizonylat.SZALLITONEV;
        //    printer.VevoCim = kimeno ? vevocim : szallitocim;
        //    printer.VevoAdoszam = kimeno ? entityBizonylat.UGYFELADOSZAM : szallitoadoszam;

        //    printer.BizonylatKelte = entityBizonylat.BIZONYLATKELTE.ToShortDateString();
        //    printer.TeljesitesKelte = entityBizonylat.TELJESITESKELTE.ToShortDateString();
        //    printer.FizetesiMod = entityBizonylat.FIZETESIMOD;
        //    printer.FizetesiHatarido = entityBizonylat.FIZETESIHATARIDO.ToShortDateString();
        //    printer.Bizonylatszam = entityBizonylat.BIZONYLATSZAM;

        //    printer.MegjegyzesFej = entityBizonylat.MEGJEGYZESFEJ;

        //    printer.Netto = entityBizonylat.NETTO.ToString("#,##0.00");
        //    printer.Afa = entityBizonylat.AFA.ToString("#,##0.00");
        //    printer.Brutto = entityBizonylat.BRUTTO.ToString("#,##0.00");
        //    printer.Termekdij = entityBizonylat.TERMEKDIJ.ToString("#,##0.00");

        //    printer.Penznem = entityBizonylat.PENZNEM;
        //    printer.Arfolyam = entityBizonylat.ARFOLYAM.ToString("#,##0.00");
        //    printer.Azaz = entityBizonylat.AZAZ;

        //    printer.TermekdijNulla = entityBizonylat.TERMEKDIJ == 0;
        //    printer.AfaFt = Calc.RealRound(entityBizonylat.AFA * entityBizonylat.ARFOLYAM, 1).ToString("#,##0.00");

        //    printer.Keszitette = entityBizonylat.LETREHOZTA;

        //    printer.LstTetel.Clear();
        //    foreach (var tetel in entityBizonylat.BIZONYLATTETEL)
        //    {
        //        var t = new BizonylatPrinterTetel
        //        {
        //            Megnevezes = tetel.MEGNEVEZES,
        //            Megjegyzes = tetel.MEGJEGYZES,
        //            Mennyiseg = tetel.MENNYISEG.ToString("#,##0.00"),
        //            Me = tetel.ME,
        //            Egysegar = tetel.EGYSEGAR.ToString("#,##0.00"),

        //            AfaKulcs = tetel.AFAKULCS,
        //            Netto = tetel.NETTO.ToString("#,##0.00"),
        //            Afa = tetel.AFA.ToString("#,##0.00"),
        //            Brutto = tetel.BRUTTO.ToString("#,##0.00"),
        //        };

        //        if (tetel.TERMEKDIJAS)
        //            t.Termekdij =
        //              $"KT {tetel.TERMEKDIJKT} termékdíj a bruttó árból: {tetel.MENNYISEG:#,##0.00} {tetel.ME} x {tetel.TOMEGKG:#,##0.00} kg x {tetel.TERMEKDIJEGYSEGAR:#,##0.00} HUF/kg = {tetel.TERMEKDIJ:#,##0.00} HUF";

        //        printer.LstTetel.Add(t);
        //    }

        //    printer.LstAfa.Clear();
        //    foreach (var afa in entityBizonylat.BIZONYLATAFA)
        //        printer.LstAfa.Add(new BizonylatPrinterAfa
        //        {
        //            AfaKulcs = afa.AFAKULCS,
        //            Netto = afa.NETTO.ToString("#,##0.00"),
        //            Afa = afa.AFA.ToString("#,##0.00"),
        //            Brutto = afa.BRUTTO.ToString("#,##0.00"),
        //        });

        //    printer.LstTermekdij.Clear();
        //    foreach (var termekdij in entityBizonylat.BIZONYLATTERMEKDIJ)
        //    {
        //        printer.LstTermekdij.Add(new BizonylatPrinterTermekdij
        //        {
        //            TermekdijKT = termekdij.TERMEKDIJKT,
        //            OssztomegKg = termekdij.OSSZTOMEGKG.ToString("#,##0.00"),
        //            TermekdijEgysegar = termekdij.TERMEKDIJEGYSEGAR.ToString("#,##0.00"),
        //            Termekdij = termekdij.TERMEKDIJ.ToString("#,##0.00"),
        //        });
        //    }
        //    printer.Szamlakep = szamlakep;
        //}

        //public static byte[] BizonylatNyomtatas(List<SzMT> fi)
        //{
        //    const string minta = "!!! MINTA !!!";

        //    using (var context = OSSContext.NewContext(_sid))
        //        try
        //        {
        //            context.Open(true);

        //            var bizonylatKod = SzMTUtils.GetInt(fi, Szempont.BizonylatKod);

        //            var entityBizonylat = BizonylatDal.GetComplex(context, bizonylatKod);
        //            BizonylatDal.Lock(context, bizonylatKod, entityBizonylat.Modositva);

        //            var entityParticio = ParticioDal.Get(context);
        //            var iratKod = entityParticio.BIZONYLAT_BIZONYLATKEP_IRATKOD != null ?
        //              (int)entityParticio.BIZONYLAT_BIZONYLATKEP_IRATKOD : throw new Exception(string.Format(Messages.ParticioHiba, "BIZONYLAT_BIZONYLATKEP_IRATKOD"));

        //            var szamlakep = IratBll.Letoltes(context, iratKod);
        //            var nyomtatasTipus = SzMTUtils.GetBizonylatNyomtatasTipus(fi, Szempont.NyomtatasTipus);
        //            var v = VerzioDal.Get(context);

        //            var printer = new BizonylatPrinter();
        //            BizonylatPrinterSetup(printer, entityBizonylat, szamlakep.b);
        //            printer.BizonylatFejlec = Bl[entityBizonylat.Bizonylattipuskod].BizonylatFejlec;
        //            printer.Verzio = v;
        //            if (nyomtatasTipus == BizonylatNyomtatasTipus.Minta)
        //                printer.BizonylatFejlec = minta + " - " + printer.BizonylatFejlec;

        //            int peldanyszam;
        //            BIZONYLAT entity;

        //            switch (nyomtatasTipus)
        //            {
        //                case BizonylatNyomtatasTipus.Minta:
        //                    printer.UjPeldany("1", minta);
        //                    break;
        //                case BizonylatNyomtatasTipus.Eredeti:
        //                    peldanyszam = GetBizonylatEredetiPeldany(context);
        //                    for (var i = 1; i <= peldanyszam; i++)
        //                        printer.UjPeldany(i.ToString(), "Eredeti");

        //                    if (entityBizonylat.Nyomtatottpeldanyokszama == 0)
        //                    {
        //                        entity = BizonylatDal.Get(context, bizonylatKod);
        //                        entity.NYOMTATOTTPELDANYOKSZAMA = peldanyszam;
        //                        BizonylatDal.Update(context, entity);
        //                    }
        //                    break;
        //                case BizonylatNyomtatasTipus.Másolat:
        //                    if (entityBizonylat.Nyomtatottpeldanyokszama == 0)
        //                        throw new Exception("Még nem készült eredeti példány!");
        //                    peldanyszam = GetBizonylatMasolat(context);
        //                    var sorszamTol = entityBizonylat.Nyomtatottpeldanyokszama + 1;
        //                    var sorszamIg = sorszamTol + peldanyszam - 1;
        //                    for (var i = sorszamTol; i <= sorszamIg; i++)
        //                        printer.UjPeldany(i.ToString(), "Másolat");

        //                    entity = BizonylatDal.Get(context, bizonylatKod);
        //                    entity.NYOMTATOTTPELDANYOKSZAMA = sorszamIg;
        //                    BizonylatDal.Update(context, entity);
        //                    break;
        //            }

        //            context.Commit();

        //            return printer.Print();
        //        }
        //        catch (Exception ex)
        //        {
        //            context.Rollback();
        //            throw OSSContext.FaultException(ex);
        //        }
        //        finally
        //        {
        //            context.Close();
        //        }
        //}

        public static string SzamlaFormaiEllenorzese(ossContext context, string sid, int bizonylatKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLAT);

            var dto = GetComplex(context, sid, bizonylatKod);
            return OnlineszamlaBll.SzamlaFormaiEllenorzese(dto);
        }

        public static string LetoltesOnlineszamlaFormatumban(ossContext context, string sid, 
            int bizonylatKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLAT);

            var dto = GetComplex(context, sid, bizonylatKod);
            return OnlineszamlaBll.LetoltesOnlineszamlaFormatumban(dto);
        }

        public static BizonylatTetelDto BizonylattetelCalc(ossContext context, string sid, 
            BizonylatTetelDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            BizonylatUtils.BizonylattetelCalc(dto);
            return dto;
        }

        public static BizonylatTetelDto Bruttobol(ossContext context, string sid, 
            BizonylatTetelDto dto, decimal brutto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            BizonylatUtils.Bruttobol(dto, brutto);
            return dto;
        }

        public static BizonylatComplexDto SumEsAfaEsTermekdij(ossContext context, string sid, 
            BizonylatComplexDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            BizonylatUtils.SumEsAfaEsTermekdij(dto.Dto, dto.LstTetelDto, dto.LstAfaDto, dto.LstTermekdijDto);
            return dto;
        }
    }
}
