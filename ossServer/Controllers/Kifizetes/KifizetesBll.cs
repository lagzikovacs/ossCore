using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;

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

        public static void Delete(ossContext context, string sid, KifizetesDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            KifizetesDal.Lock(context, dto.Kifizeteskod, dto.Modositva);
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

        public static int Update(ossContext context, string sid, KifizetesDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.BIZONYLATMOD);

            KifizetesDal.Lock(context, dto.Kifizeteskod, dto.Modositva);
            var entity = KifizetesDal.Get(context, dto.Kifizeteskod);
            ObjectUtils.Update(dto, entity);
            return KifizetesDal.Update(context, entity);
        }
    }
}
