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
    }
}
