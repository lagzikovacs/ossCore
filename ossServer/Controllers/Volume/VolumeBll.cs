using ossServer.Controllers.Csoport;
using ossServer.Controllers.Dokumentum;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Volume
{
    public class VolumeBll
    {
        public static async Task<List<VolumeDto>> ReadAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.VOLUME);

            var entities = await VolumeDal.ReadAsync(context);
            return ObjectUtils.Convert<Models.Volume, VolumeDto>(entities);
        }

        public static async Task<List<int>> DokumentumkodByVolumeAsync(ossContext context, string sid, int volumeKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.VOLUME);

            return await DokumentumDal.DokumentumkodByVolumeAsync(context, volumeKod);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Volumekod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Volumeno", Title = "No", Type = ColumnType.INT },
                new ColumnSettings {Name="Maxmeret", Title = "Max. méret", Type = ColumnType.INT },
                new ColumnSettings {Name="Jelenlegimeret", Title = "Jelenlegi méret", Type = ColumnType.INT },
                new ColumnSettings {Name="Allapot", Title = "Állapot", Type = ColumnType.STRING },
                new ColumnSettings {Name="Allapotkelte", Title = "Az állapot kelte", Type = ColumnType.DATETIME },
            };
        }

        public static List<ColumnSettings> ReszletekColumns()
        {
            return GridColumns();
        }

        public static List<ColumnSettings> GridSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            return GridColumns();
        }

        public static List<ColumnSettings> ReszletekSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            return GridColumns();
        }
    }
}
