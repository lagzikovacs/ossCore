using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Kifizetes
{
    public class KifizetesBll
    {
        private static KifizetesDto Calc(Models.Kifizetes entity)
        {
            var result = ObjectUtils.Convert<Models.Kifizetes, KifizetesDto>(entity);

            result.Penznem = entity.PenznemkodNavigation.Penznem1;
            result.Fizetesimod = entity.FizetesimodkodNavigation.Fizetesimod1;

            return result;
        }

        public static KifizetesDto Get(ossContext context, string sid, int kifizetesKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLAT);

            var entity = KifizetesDal.Get(context, kifizetesKod);
            return Calc(entity);
        }

        public static List<KifizetesDto> Select(ossContext context, string sid, int bizonylatKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLAT);

            var entities = KifizetesDal.Read(context, bizonylatKod);

            var result = new List<KifizetesDto>();
            foreach (var entity in entities)
                result.Add(Calc(entity));

            return result;
        }

        public static async Task DeleteAsync(ossContext context, string sid, KifizetesDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            await KifizetesDal.Lock(context, dto.Kifizeteskod, dto.Modositva);
            var entity = KifizetesDal.Get(context, dto.Kifizeteskod);
            KifizetesDal.Delete(context, entity);
        }

        public static int Add(ossContext context, string sid, KifizetesDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            var entity = ObjectUtils.Convert<KifizetesDto, Models.Kifizetes>(dto);
            return KifizetesDal.Add(context, entity);
        }

        public static KifizetesDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            return new KifizetesDto { Datum = DateTime.Today };
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, KifizetesDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            await KifizetesDal.Lock(context, dto.Kifizeteskod, dto.Modositva);
            var entity = KifizetesDal.Get(context, dto.Kifizeteskod);
            ObjectUtils.Update(dto, entity);
            return KifizetesDal.Update(context, entity);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Kifizeteskod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Datum", Title = "Dátum", Type = ColumnType.DATE },
                new ColumnSettings {Name="Osszeg", Title = "Összeg", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Penznem", Title = "Pénznem", Type = ColumnType.STRING },
                new ColumnSettings {Name="Fizetesimod", Title = "Fizetési mód", Type = ColumnType.STRING },
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
