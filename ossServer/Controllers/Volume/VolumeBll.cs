using ossServer.Controllers.Csoport;
using ossServer.Controllers.Dokumentum;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Volume
{
    public class VolumeBll
    {
        public static List<VolumeDto> Read(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.VOLUME);

            var entities = VolumeDal.Read(context);
            return ObjectUtils.Convert<Models.Volume, VolumeDto>(entities);
        }

        public static List<int> DokumentumkodByVolume(ossContext context, string sid, int volumeKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.VOLUME);

            return DokumentumDal.DokumentumkodByVolume(context, volumeKod);
        }

        private static List<ColumnSettings> BaseColumns()
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

            return BaseColumns();
        }
    }
}
