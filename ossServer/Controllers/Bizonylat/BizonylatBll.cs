using Newtonsoft.Json;
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
using System.Threading.Tasks;

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

        public static async Task B(ossContext context)
        {
            await CsoportDal.JogeAsync(context, JogKod.DIJBEKERO);
        }

        public static async Task<BizonylatTipusLeiro> BizonylatLeiroAsync(ossContext context, string sid, 
            BizonylatTipus bizonylatTipus)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeBizonylatAsync(context);

            return Bl[bizonylatTipus.GetHashCode()];
        }

        public static async Task<BizonylatComplexDto> CreateNewComplexAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.BIZONYLATMOD);

            var entityParticio = await ParticioDal.GetAsync(context);
            var szc = JsonConvert.DeserializeObject<SzallitoConf>(entityParticio.Szallito);

            var result = new BizonylatComplexDto
            {
                Dto = new BizonylatDto
                {
                    Szallitonev = szc.Nev,
                    Szallitoiranyitoszam = szc.Iranyitoszam,
                    Szallitohelysegnev = szc.Helysegnev,
                    Szallitoutcahazszam = szc.Utcahazszam,
                    Szallitobankszamla1 = szc.Bankszamla1,
                    Szallitobankszamla2 = szc.Bankszamla2,
                    Szallitoadotorzsszam = szc.Adotorzsszam,
                    Szallitoadoafakod = szc.Adoafakod,
                    Szallitoadomegyekod = szc.Adomegyekod,

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
            var lstPenznem = await PenznemDal.ReadAsync(context, penznem);
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

        public static async Task<BizonylatDto> GetAsync(ossContext context, string sid, int bizonylatKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeBizonylatAsync(context);

            var entity = await BizonylatDal.GetAsync(context, bizonylatKod);
            var dto = ObjectUtils.Convert<Models.Bizonylat, BizonylatDto>(entity);

            return CalcCim(dto);
        }

        public static async Task<BizonylatComplexDto> GetComplexAsync(ossContext context, string sid, int bizonylatKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeBizonylatAsync(context);

            var entitycomplex = await BizonylatDal.GetComplexAsync(context, bizonylatKod);
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

        public static async Task DeleteAsync(ossContext context, string sid, BizonylatDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.BIZONYLATMOD);

            await BizonylatDal.Lock(context, dto.Bizonylatkod, dto.Modositva);
            var entity = await BizonylatDal.GetAsync(context, dto.Bizonylatkod);
            await BizonylatDal.DeleteAsync(context, entity);
        }

        public static async Task<Tuple<List<BizonylatDto>, int>> SelectAsync(ossContext context, string sid, int rekordTol, int lapMeret, 
            BizonylatTipus bizonylatTipus, List<SzMT> szmt)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeBizonylatAsync(context);

            var qry = BizonylatDal.GetQuery(context, bizonylatTipus, szmt);
            var osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
            var result = ObjectUtils.Convert<Models.Bizonylat, BizonylatDto>(entities);
            foreach (var r in result)
                CalcCim(r);

            return new Tuple<List<BizonylatDto>, int>(result, osszesRekord);
        }




        private static string GenerateBizonylatszam(ossContext context, int bizonylattipusKod)
        {
            return Bl[bizonylattipusKod].Elotag +
                context.KodGen(Bl[bizonylattipusKod].KodNev).ToString("00000");
        }

        public static async Task<int> SaveAsync(ossContext context, string sid, BizonylatComplexDto complexDto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.BIZONYLATMOD);

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
                bizonylatKod = await BizonylatDal.AddAsync(context, entity);
            }
            else
            {
                //updateelni a fejet
                bizonylatKod = complexDto.Dto.Bizonylatkod;
                await BizonylatDal.Lock(context, bizonylatKod, complexDto.Dto.Modositva);
                entity = await BizonylatDal.GetAsync(context, bizonylatKod);
                ObjectUtils.Update(complexDto.Dto, entity);
                await BizonylatDal.UpdateAsync(context, entity);

                //törölni az esetleges létező tételt-áfát-
                var entitesTetel = BizonylatTetelDal.Select(context, bizonylatKod);
                foreach (var l in entitesTetel)
                    await BizonylatTetelDal.DeleteAsync(context, l);

                var entitesAfa = BizonylatAfaDal.Select(context, bizonylatKod);
                foreach (var l in entitesAfa)
                    await BizonylatAfaDal.DeleteAsync(context, l);

                var entitesTermekdij = BizonylatTermekdijDal.Select(context, bizonylatKod);
                foreach (var l in entitesTermekdij)
                    await BizonylatTermekdijDal.DeleteAsync(context, l);
            }

            //beírni a bizonylatkódot a tételbe-áfába-termékdíjba
            foreach (var l in complexDto.LstTetelDto)
            {
                l.Bizonylattetelkod = 0;
                l.Bizonylatkod = bizonylatKod;

                var entityTetel = ObjectUtils.Convert<BizonylatTetelDto, Models.Bizonylattetel>(l);
                await BizonylatTetelDal.AddAsync(context, entityTetel);
            }
            foreach (var l in complexDto.LstAfaDto)
            {
                l.Bizonylatafakod = 0;
                l.Bizonylatkod = bizonylatKod;

                var entityAfa = ObjectUtils.Convert<BizonylatAfaDto, Models.Bizonylatafa>(l);
                await BizonylatAfaDal.AddAsync(context, entityAfa);
            }
            foreach (var l in complexDto.LstTermekdijDto)
            {
                l.Bizonylattermekdijkod = 0;
                l.Bizonylatkod = bizonylatKod;

                var entityTermekdij = ObjectUtils.Convert<BizonylatTermekdijDto, Models.Bizonylattermekdij>(l);
                await BizonylatTermekdijDal.AddAsync(context, entityTermekdij);
            }

            return bizonylatKod;
        }

        public static async Task<int> UjBizonylatMintaAlapjanAsync(ossContext context, string sid, 
            int bizonylatKod, BizonylatTipus bizonylatTipus)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.BIZONYLATMOD);

            var entity = await BizonylatDal.GetComplexAsync(context, bizonylatKod);

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
            return await SaveAsync(context, sid, complexDto);
        }

        public static async Task<int> StornoAsync(ossContext context, string sid, BizonylatDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.BIZONYLATMOD);

            await BizonylatDal.Lock(context, dto.Bizonylatkod, dto.Modositva);

            //a fej/tételek/áfa lekérése/módosítása/hozzáadása lépésenként
            var stornozando = await BizonylatDal.GetAsync(context, dto.Bizonylatkod);
            if (stornozando.Bizonylatszam == null)
                throw new Exception("Lezáratlan bizonylatot nem lehet stornozni!");
            if (stornozando.Ezstornozott)
                throw new Exception("A bizonylat már stornozott!");

            //ha díjbekérő -> nem jön létre másik bizonylat
            if (stornozando.Bizonylattipuskod == (int)BizonylatTipus.DijBekero)
            {
                stornozando.Ezstornozott = true;
                await BizonylatDal.UpdateAsync(context, stornozando);

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

            await BizonylatDal.AddAsync(context, stornozo);

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

                await BizonylatTetelDal.AddAsync(context, tetel);
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

                await BizonylatAfaDal.AddAsync(context, afa);
            }

            //termékdíj másolása
            var stornozotermekdij = BizonylatTermekdijDal.Select(context, dto.Bizonylatkod);
            foreach (var t in stornozotermekdij)
            {
                var termekdij = ObjectUtils.Clone(t);

                termekdij.Bizonylatkod = stornozo.Bizonylatkod;
                termekdij.Ossztomegkg = -termekdij.Ossztomegkg;
                termekdij.Termekdij = -termekdij.Termekdij;

                await BizonylatTermekdijDal.AddAsync(context, termekdij);
            }

            //ellenőrizni a tartalmat
            // TODO
            //var invoice = OnlineszamlaBll.GetInvoice(context, stornozo.Bizonylatkod);
            //var hibak = invoice.ValidateErrors();
            //if (hibak.Any())
            //    throw new Exception(string.Join(Environment.NewLine, hibak));

            //hozzáadni a feltöltendők listájához
            await OnlineszamlaDal.AddAsync(context, stornozo.Bizonylatkod);


            //az eredetiben ennyi módosítás
            stornozando.Ezstornozott = true;
            stornozando.Stornozobizonylatkod = stornozo.Bizonylatkod;
            await BizonylatDal.UpdateAsync(context, stornozando);

            return stornozo.Bizonylatkod;
        }

        public static async Task<int> KibocsatasAsync(ossContext context, string sid, 
            BizonylatDto dto, string bizonylatszam)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.BIZONYLATMOD);

            await BizonylatDal.Lock(context, dto.Bizonylatkod, dto.Modositva);
            var entity = await BizonylatDal.GetAsync(context, dto.Bizonylatkod);
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
            var result = await BizonylatDal.UpdateAsync(context, entity);

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
                await OnlineszamlaDal.AddAsync(context, entity.Bizonylatkod);
            }

            return result;
        }

        public static async Task<int> KifizetesRendbenAsync(ossContext context, string sid, BizonylatDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.BIZONYLATMOD);

            await BizonylatDal.Lock(context, dto.Bizonylatkod, dto.Modositva);
            var entity = await BizonylatDal.GetAsync(context, dto.Bizonylatkod);
            if (entity.Kifizetesrendben == true) //null miatt
                entity.Kifizetesrendben = false;
            else
                entity.Kifizetesrendben = true;
            return await BizonylatDal.UpdateAsync(context, entity);
        }

        public static async Task<int> KiszallitvaAsync(ossContext context, string sid, BizonylatDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.BIZONYLATMOD);

            await BizonylatDal.Lock(context, dto.Bizonylatkod, dto.Modositva);
            var entity = await BizonylatDal.GetAsync(context, dto.Bizonylatkod);
            if (entity.Kiszallitva == true) //null miatt
                entity.Kiszallitva = false;
            else
                entity.Kiszallitva = true;
            return await BizonylatDal.UpdateAsync(context, entity);
        }

        public static async Task<string> SzamlaFormaiEllenorzeseAsync(ossContext context, string sid, int bizonylatKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeBizonylatAsync(context);

            var dto = await GetComplexAsync(context, sid, bizonylatKod);
            return OnlineszamlaBll.SzamlaFormaiEllenorzese(dto);
        }

        public static async Task<string> LetoltesOnlineszamlaFormatumbanAsync(ossContext context, string sid, 
            int bizonylatKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeBizonylatAsync(context);

            var dto = await GetComplexAsync(context, sid, bizonylatKod);
            return OnlineszamlaBll.LetoltesOnlineszamlaFormatumban(dto);
        }

        public static async Task<BizonylatTetelDto> BizonylattetelCalcAsync(ossContext context, string sid, 
            BizonylatTetelDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.BIZONYLATMOD);

            BizonylatUtils.BizonylattetelCalc(dto);
            return dto;
        }

        public static async Task<BizonylatTetelDto> BruttobolAsync(ossContext context, string sid, 
            BizonylatTetelDto dto, decimal brutto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.BIZONYLATMOD);

            BizonylatUtils.Bruttobol(dto, brutto);
            return dto;
        }

        public static async Task<BizonylatComplexDto> SumEsAfaEsTermekdijAsync(ossContext context, string sid, 
            BizonylatComplexDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.BIZONYLATMOD);

            BizonylatUtils.SumEsAfaEsTermekdij(dto.Dto, dto.LstTetelDto, dto.LstAfaDto, dto.LstTermekdijDto);
            return dto;
        }
    }
}
