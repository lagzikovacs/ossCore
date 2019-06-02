using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Irattipus
{
    public class IrattipusBll
    {
        public static int Add(ossContext context, string sid, IrattipusDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<IrattipusDto, Models.Irattipus>(dto);
            IrattipusDal.Exists(context, entity);
            return IrattipusDal.Add(context, entity);
        }

        public static IrattipusDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);
            return new IrattipusDto();
        }

        public static void Delete(ossContext context, string sid, IrattipusDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            IrattipusDal.Lock(context, dto.Irattipuskod, dto.Modositva);
            IrattipusDal.CheckReferences(context, dto.Irattipuskod);
            var entity = IrattipusDal.Get(context, dto.Irattipuskod);
            IrattipusDal.Delete(context, entity);
        }

        public static IrattipusDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = IrattipusDal.Get(context, key);
            return ObjectUtils.Convert<Models.Irattipus, IrattipusDto>(entity);
        }

        public static List<IrattipusDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = IrattipusDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Irattipus, IrattipusDto>(entities);
        }

        public static int Update(ossContext context, string sid, IrattipusDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            IrattipusDal.Lock(context, dto.Irattipuskod, dto.Modositva);
            var entity = IrattipusDal.Get(context, dto.Irattipuskod);
            ObjectUtils.Update(dto, entity);
            IrattipusDal.ExistsAnother(context, entity);
            return IrattipusDal.Update(context, entity);
        }

        public static void ZoomCheck(ossContext context, string sid, int irattipuskod, string irattipus)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            IrattipusDal.ZoomCheck(context, irattipuskod, irattipus);
        }

        private static List<ColumnSettings> BaseColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Afakulcskod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Afakulcs1", Title = "ÁFA kulcs", Type = ColumnType.STRING },
                new ColumnSettings {Name="Afamerteke", Title = "ÁFA mértéke, %", Type = ColumnType.NUMBER },
            };
        }

        public static List<ColumnSettings> GridSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            return BaseColumns();
        }

        public static List<ColumnSettings> ReszletekSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            return ColumnSettingsUtil.AddIdobelyeg(BaseColumns());
        }
    }
}
