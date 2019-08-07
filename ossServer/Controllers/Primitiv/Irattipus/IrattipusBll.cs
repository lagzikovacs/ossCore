using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Irattipus
{
    public class IrattipusBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, IrattipusDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<IrattipusDto, Models.Irattipus>(dto);
            await IrattipusDal.ExistsAsync(context, entity);
            return await IrattipusDal.AddAsync(context, entity);
        }

        public static IrattipusDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);
            return new IrattipusDto();
        }

        public static async Task DeleteAsync(ossContext context, string sid, IrattipusDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await IrattipusDal.Lock(context, dto.Irattipuskod, dto.Modositva);
            await IrattipusDal.CheckReferencesAsync(context, dto.Irattipuskod);
            var entity = await IrattipusDal.GetAsync(context, dto.Irattipuskod);
            await IrattipusDal.DeleteAsync(context, entity);
        }

        public static async Task<IrattipusDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entity = await IrattipusDal.GetAsync(context, key);
            return ObjectUtils.Convert<Models.Irattipus, IrattipusDto>(entity);
        }

        public static async Task<List<IrattipusDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            var entities = await IrattipusDal.ReadAsync(context, maszk);
            return ObjectUtils.Convert<Models.Irattipus, IrattipusDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, IrattipusDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);

            await IrattipusDal.Lock(context, dto.Irattipuskod, dto.Modositva);
            var entity = await IrattipusDal.GetAsync(context, dto.Irattipuskod);
            ObjectUtils.Update(dto, entity);
            await IrattipusDal.ExistsAnotherAsync(context, entity);
            return await IrattipusDal.UpdateAsync(context, entity);
        }

        public static async Task ZoomCheckAsync(ossContext context, string sid, int irattipuskod, string irattipus)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PRIMITIVEK);

            await IrattipusDal.ZoomCheckAsync(context, irattipuskod, irattipus);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Irattipuskod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Irattipus1", Title = "Irattipus", Type = ColumnType.STRING },
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
