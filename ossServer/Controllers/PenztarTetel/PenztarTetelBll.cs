using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.PenztarTetel
{
    public class PenztarTetelBll
    {
        public static PenztarTetelDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PENZTAR);

            return new PenztarTetelDto { Jogcim = "Pénzfelvét bankból" };
        }

        public static int Add(ossContext context, string sid, PenztarTetelDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PENZTAR);

            dto.Penztarbizonylatszam =
                context.KodGen(KodNev.Penztar.ToString() + dto.Penztarkod).ToString("00000") + "/" +
                dto.Penztarkod;

            var entity = ObjectUtils.Convert<PenztarTetelDto, Models.Penztartetel>(dto);
            return PenztarTetelDal.Add(context, entity);
        }

        public static PenztarTetelDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PENZTAR);

            var entity = PenztarTetelDal.Get(context, key);
            return ObjectUtils.Convert<Models.Penztartetel, PenztarTetelDto>(entity);
        }

        public static List<PenztarTetelDto> Select(ossContext context, string sid, int rekordTol, 
            int lapMeret, List<SzMT> szmt, out int osszesRekord)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PENZTAR);

            var qry = PenztarTetelDal.GetQuery(context, szmt);
            osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
            return ObjectUtils.Convert<Models.Penztartetel, PenztarTetelDto>(entities);
        }
    }
}
