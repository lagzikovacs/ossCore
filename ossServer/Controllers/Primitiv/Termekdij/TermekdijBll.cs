using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Termekdij
{
    public class TermekdijBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, TermekdijDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<TermekdijDto, Models.Termekdij>(dto);
            await TermekdijDal.ExistsAsync(context, entity);
            return await TermekdijDal.AddAsync(context, entity);
        }

        public static TermekdijDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            return new TermekdijDto { Termekdijegysegar = 0 };
        }

        public static async Task DeleteAsync(ossContext context, string sid, TermekdijDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            await TermekdijDal.Lock(context, dto.Termekdijkod, dto.Modositva);
            await TermekdijDal.CheckReferencesAsync(context, dto.Termekdijkod);
            var entity = await TermekdijDal.GetAsync(context, dto.Termekdijkod);
            await TermekdijDal.DeleteAsync(context, entity);
        }

        public static async Task<TermekdijDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = await TermekdijDal.GetAsync(context, key);
            return ObjectUtils.Convert<Models.Termekdij, TermekdijDto>(entity);
        }

        public static async Task<List<TermekdijDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = await TermekdijDal.ReadAsync(context, maszk);
            return ObjectUtils.Convert<Models.Termekdij, TermekdijDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, TermekdijDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            await TermekdijDal.Lock(context, dto.Termekdijkod, dto.Modositva);
            var entity = await TermekdijDal.GetAsync(context, dto.Termekdijkod);
            ObjectUtils.Update(dto, entity);
            await TermekdijDal.ExistsAnotherAsync(context, entity);
            return await TermekdijDal.UpdateAsync(context, entity);
        }

        public static async Task ZoomCheckAsync(ossContext context, string sid, int termekdijkod, string termekdijkt)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            await TermekdijDal.ZoomCheckAsync(context, termekdijkod, termekdijkt);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Termekdijkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Termekdijkt", Title = "Termékdíj KT", Type = ColumnType.STRING },
                new ColumnSettings {Name="Termekdijmegnevezes", Title = "Termékdíj megnevezés", Type = ColumnType.STRING },
                new ColumnSettings {Name="Termekdijegysegar", Title = "Termékdíj egységár", Type = ColumnType.NUMBER },
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
