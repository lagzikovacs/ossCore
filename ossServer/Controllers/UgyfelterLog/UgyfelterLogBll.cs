using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Controllers.Ugyfel;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.UgyfelterLog
{
    public class UgyfelterLogBll
    {
        private static UgyfelterLogDto Calc(Ugyfelterlog entity)
        {
            var result = ObjectUtils.Convert<Ugyfelterlog, UgyfelterLogDto>(entity);
            result.Ugyfelnev = entity.UgyfelkodNavigation.Nev;
            result.Ugyfelcim = UgyfelBll.Cim(entity.UgyfelkodNavigation);

            return result;
        }
        public static async Task<Tuple<List<UgyfelterLogDto>, int>> SelectAsync(ossContext context, string sid, int rekordTol, int lapMeret, 
            List<SzMT> szmt)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELTERLOG);

            var qry = UgyfelterLogDal.GetQuery(context, szmt);
            var osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
            var result = new List<UgyfelterLogDto>();

            foreach (var entity in entities)
                result.Add(Calc(entity));

            return new Tuple<List<UgyfelterLogDto>, int>(result, osszesRekord);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Ugyfelterlogkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Ugyfelnev", Title = "Név", Type = ColumnType.STRING },
                new ColumnSettings {Name="Ugyfelcim", Title = "Cím", Type = ColumnType.STRING },
                new ColumnSettings {Name="Letrehozta", Title = "Létrehozta", Type = ColumnType.STRING },
                new ColumnSettings {Name="Letrehozva", Title = "Létrehozva", Type = ColumnType.DATETIME },
            };
        }

        public static List<ColumnSettings> ReszletekColumns()
        {
            return GridColumns();
        }
    }
}
