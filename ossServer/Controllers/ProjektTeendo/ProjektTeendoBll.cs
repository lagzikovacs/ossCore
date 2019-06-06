using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;

namespace ossServer.Controllers.ProjektTeendo
{
    public class ProjektTeendoBll
    {
        public static ProjektTeendoDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            var entity = ProjektTeendoDal.Get(context, key);
            var result = ObjectUtils.Convert<Projektteendo, ProjektTeendoDto>(entity);

            result.Teendo = entity.TeendokodNavigation.Teendo1;

            return result;
        }

        public static ProjektTeendoDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            return new ProjektTeendoDto
            {
                Hatarido = DateTime.Now.Date
            };
        }

        public static int Add(ossContext context, string sid, ProjektTeendoDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            var entity = ObjectUtils.Convert<ProjektTeendoDto, Projektteendo>(dto);
            return ProjektTeendoDal.Add(context, entity);
        }

        public static List<ProjektTeendoDto> Select(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            var entities = ProjektTeendoDal.Select(context, projektKod);
            var result = new List<ProjektTeendoDto>();

            foreach (var entity in entities)
            {
                var dto = ObjectUtils.Convert<Projektteendo, ProjektTeendoDto>(entity);
                dto.Teendo = entity.TeendokodNavigation.Teendo1;

                result.Add(dto);
            }

            return result;
        }

        public static void Delete(ossContext context, string sid, ProjektTeendoDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            ProjektTeendoDal.Lock(context, dto.Projektteendokod, dto.Modositva);
            var entity = ProjektTeendoDal.Get(context, dto.Projektteendokod);
            ProjektTeendoDal.Delete(context, entity);
        }

        public static int Update(ossContext context, string sid, ProjektTeendoDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            ProjektTeendoDal.Lock(context, dto.Projektteendokod, dto.Modositva);
            var entity = ProjektTeendoDal.Get(context, dto.Projektteendokod);
            ObjectUtils.Update(dto, entity);
            return ProjektTeendoDal.Update(context, entity);
        }

        private static List<ColumnSettings> BaseColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Projektteendokod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Dedikalva", Title = "Dedikálva", Type = ColumnType.STRING },
                new ColumnSettings {Name="Hatarido", Title = "Határidő", Type = ColumnType.DATE },
                new ColumnSettings {Name="Elvegezve", Title = "Elvégezve", Type = ColumnType.DATE },
                new ColumnSettings {Name="Teendo", Title = "Teendő", Type = ColumnType.STRING },
                new ColumnSettings {Name="Leiras", Title = "Leirás", Type = ColumnType.STRING },
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
