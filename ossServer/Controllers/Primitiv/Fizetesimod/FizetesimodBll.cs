using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Fizetesimod
{
    public class FizetesimodBll
    {
        public static int Add(ossContext context, string sid, FizetesimodDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<FizetesimodDto, Models.Fizetesimod>(dto);
            FizetesimodDal.Exists(context, entity);
            return FizetesimodDal.Add(context, entity);
        }

        public static FizetesimodDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);
            return new FizetesimodDto();
        }

        public static async Task DeleteAsync(ossContext context, string sid, FizetesimodDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            await FizetesimodDal.Lock(context, dto.Fizetesimodkod, dto.Modositva);
            FizetesimodDal.CheckReferences(context, dto.Fizetesimodkod);
            var entity = FizetesimodDal.Get(context, dto.Fizetesimodkod);
            FizetesimodDal.Delete(context, entity);
        }

        public static FizetesimodDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = FizetesimodDal.Get(context, key);
            return ObjectUtils.Convert<Models.Fizetesimod, FizetesimodDto>(entity);
        }

        public static List<FizetesimodDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = FizetesimodDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Fizetesimod, FizetesimodDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, FizetesimodDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            await FizetesimodDal.Lock(context, dto.Fizetesimodkod, dto.Modositva);
            var entity = FizetesimodDal.Get(context, dto.Fizetesimodkod);
            ObjectUtils.Update(dto, entity);
            FizetesimodDal.ExistsAnother(context, entity);
            return FizetesimodDal.Update(context, entity);
        }

        public static void ZoomCheck(ossContext context, string sid, int fizetesimodKod, string fizetesimod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            FizetesimodDal.ZoomCheck(context, fizetesimodKod, fizetesimod);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Fizetesimodkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Fizetesimod1", Title = "Fizetési mód", Type = ColumnType.STRING },
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
