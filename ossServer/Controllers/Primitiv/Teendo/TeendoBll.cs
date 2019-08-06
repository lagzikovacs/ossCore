using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Teendo
{
    public class TeendoBll
    {
        public static int Add(ossContext context, string sid, TeendoDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<TeendoDto, Models.Teendo>(dto);
            TeendoDal.Exists(context, entity);
            return TeendoDal.Add(context, entity);
        }

        public static TeendoDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);
            return new TeendoDto();
        }

        public static async Task DeleteAsync(ossContext context, string sid, TeendoDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            await TeendoDal.Lock(context, dto.Teendokod, dto.Modositva);
            TeendoDal.CheckReferences(context, dto.Teendokod);
            var entity = TeendoDal.Get(context, dto.Teendokod);
            TeendoDal.Delete(context, entity);
        }

        public static TeendoDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = TeendoDal.Get(context, key);
            return ObjectUtils.Convert<Models.Teendo, TeendoDto>(entity);
        }

        public static List<TeendoDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = TeendoDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Teendo, TeendoDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, TeendoDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            await TeendoDal.Lock(context, dto.Teendokod, dto.Modositva);
            var entity = TeendoDal.Get(context, dto.Teendokod);
            ObjectUtils.Update(dto, entity);
            TeendoDal.ExistsAnother(context, entity);
            return TeendoDal.Update(context, entity);
        }

        public static void ZoomCheck(ossContext context, string sid, int teendokod, string teendo)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            TeendoDal.ZoomCheck(context, teendokod, teendo);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Teendokod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Teendo1", Title = "Teendő", Type = ColumnType.STRING },
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
