using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Penztar
{
    public class PenztarBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, PenztarDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PENZTARMOD);

            var entity = ObjectUtils.Convert<PenztarDto, Models.Penztar>(dto);
            await PenztarDal.ExistsAsync(context, entity);
            return await PenztarDal.AddAsync(context, entity);
        }

        public static async Task<PenztarDto> CreateNewAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PENZTARMOD);

            return new PenztarDto { Nyitva = true };
        }

        public static async Task DeleteAsync(ossContext context, string sid, PenztarDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PENZTARMOD);

            await PenztarDal.Lock(context, dto.Penztarkod, dto.Modositva);
            await PenztarDal.CheckReferencesAsync(context, dto.Penztarkod);
            var entity = await PenztarDal.GetAsync(context, dto.Penztarkod);
            await PenztarDal.DeleteAsync(context, entity);
        }

        public static async Task<PenztarDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PENZTAR);

            var entity = await PenztarDal.GetAsync(context, key);
            return ObjectUtils.Convert<Models.Penztar, PenztarDto>(entity);
        }

        public static async Task<List<PenztarDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PENZTAR);

            return await PenztarDal.ReadAsync(context, maszk);
        }
        public static async Task<List<PenztarDto>> ReadAsync(ossContext context, string sid, int penztarkod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PENZTAR);

            return await PenztarDal.ReadAsync(context, penztarkod);
        }
        public static async Task<List<PenztarDto>> ReadByCurrencyOpenedAsync(ossContext context, string sid, int penznemkod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PENZTAR);

            return await PenztarDal.ReadByCurrencyOpenedAsync(context, penznemkod);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, PenztarDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PENZTARMOD);

            await PenztarDal.Lock(context, dto.Penztarkod, dto.Modositva);
            var entity = await PenztarDal.GetAsync(context, dto.Penztarkod);
            ObjectUtils.Update(dto, entity);
            await PenztarDal.ExistsAnotherAsync(context, entity);
            return await PenztarDal.UpdateAsync(context, entity);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Penztarkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Penztar1", Title = "Pénztár", Type = ColumnType.STRING },
                new ColumnSettings {Name="Penznem", Title = "Pénznem", Type = ColumnType.STRING },
                new ColumnSettings {Name="Egyenleg", Title = "Egyenleg", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Nyitva", Title = "Nyitva", Type = ColumnType.BOOL },
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
