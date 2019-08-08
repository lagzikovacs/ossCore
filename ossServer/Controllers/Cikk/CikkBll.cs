using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Cikk
{
    public class CikkBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, CikkDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CIKK);

            var entity = ObjectUtils.Convert<CikkDto, Models.Cikk>(dto);
            await CikkDal.ExistsAsync(context, entity);
            return await CikkDal.AddAsync(context, entity);
        }

        public static CikkDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            return new CikkDto();
        }

        public static async Task DeleteAsync(ossContext context, string sid, CikkDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CIKK);

            await CikkDal.Lock(context, dto.Cikkkod, dto.Modositva);
            await CikkDal.CheckReferencesAsync(context, dto.Cikkkod);
            var entity = await CikkDal.GetAsync(context, dto.Cikkkod);
            await CikkDal.DeleteAsync(context, entity);
        }

        private static CikkDto Calc(Models.Cikk entity)
        {
            var dto = ObjectUtils.Convert<Models.Cikk, CikkDto>(entity);

            dto.Afakulcs = entity.AfakulcskodNavigation.Afakulcs1;
            dto.Afamerteke = entity.AfakulcskodNavigation.Afamerteke;
            dto.Me = entity.MekodNavigation.Me;

            if (entity.TermekdijkodNavigation != null)
            {
                dto.Termekdijkt = entity.TermekdijkodNavigation.Termekdijkt;
                dto.Termekdijmegnevezes = entity.TermekdijkodNavigation.Termekdijmegnevezes;
                dto.Termekdijegysegar = entity.TermekdijkodNavigation.Termekdijegysegar;
            }

            return dto;
        }

        public static async Task<CikkDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CIKK);

            var entity = await CikkDal.GetAsync(context, key);
            return Calc(entity);
        }

        public static async Task<List<CikkDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CIKK);

            var entities = await CikkDal.ReadAsync(context, maszk);

            var result = new List<CikkDto>();
            foreach (var entity in entities)
                result.Add(Calc(entity));

            return result;
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, CikkDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CIKK);

            await CikkDal.Lock(context, dto.Cikkkod, dto.Modositva);
            var entity = await CikkDal.GetAsync(context, dto.Cikkkod);

            ObjectUtils.Update(dto, entity);
            await CikkDal.ExistsAnotherAsync(context, entity);
            return await CikkDal.UpdateAsync(context, entity);
        }

        public static async Task<Tuple<List<CikkDto>, int>> SelectAsync(ossContext context, string sid, int rekordTol, int lapMeret, 
            List<SzMT> szmt)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CIKK);

            var qry = CikkDal.GetQuery(context, szmt);
            var osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();

            var result = new List<CikkDto>();
            foreach (var entity in entities)
                result.Add(Calc(entity));

            return new Tuple<List<CikkDto>, int>(result, osszesRekord);
        }

        public static async Task<List<CikkMozgasTetelDto>> MozgasAsync(ossContext context, string sid, int cikkKod, 
            BizonylatTipus bizonylatTipus)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CIKK);

            return await CikkDal.MozgasAsync(context, cikkKod, bizonylatTipus);
        }

        public static async Task ZoomCheckAsync(ossContext context, string sid, int Cikkkod, string Cikk)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CIKK);

            await CikkDal.ZoomCheckAsync(context, Cikkkod, Cikk);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Cikkkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Megnevezes", Title = "Megnevezés", Type = ColumnType.STRING },
                new ColumnSettings {Name="Me", Title = "Me", Type = ColumnType.STRING },
                new ColumnSettings {Name="Afakulcs", Title = "ÁFA kulcs", Type = ColumnType.STRING },
                new ColumnSettings {Name="Egysegar", Title = "Egységár", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Keszletetkepez", Title = "Készletet képez", Type = ColumnType.BOOL },
                new ColumnSettings {Name="Tomegkg", Title = "Tömeg, kg", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Termekdijkt", Title = "Termékdíj KT", Type = ColumnType.STRING },
                new ColumnSettings {Name="Termekdijmegnevezes", Title = "Termékdíj megnevezés", Type = ColumnType.STRING },
                new ColumnSettings {Name="Termekdijegysegar", Title = "Termékdíj egységár", Type = ColumnType.NUMBER },
            };
        }

        public static List<ColumnSettings> BeszerzesKivetGridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Bizonylatszam", Title = "Bizonylatszám", Type = ColumnType.STRING },
                new ColumnSettings {Name="Ugyfelnev", Title = "Ügyfél", Type = ColumnType.STRING },
                new ColumnSettings {Name="Teljesiteskelte", Title = "A teljesítés kelte", Type = ColumnType.DATE },
                new ColumnSettings {Name="Mennyiseg", Title = "Mennyiség", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Me", Title = "Me", Type = ColumnType.STRING },
                new ColumnSettings {Name="Egysegar", Title = "Egységár", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Penznem", Title = "Pénznem", Type = ColumnType.STRING },
                new ColumnSettings {Name="Netto", Title = "Netto", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Nettoft", Title = "Netto Ft", Type = ColumnType.NUMBER },
            };
        }

        public static List<ColumnSettings> ReszletekColumns()
        {
            return ColumnSettingsUtil.AddIdobelyeg(GridColumns());
        }

        public static List<ColumnSettings> GridSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            return GridColumns();
        }

        public static List<ColumnSettings> ReszletekSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            return ReszletekColumns();
        }
    }
}
