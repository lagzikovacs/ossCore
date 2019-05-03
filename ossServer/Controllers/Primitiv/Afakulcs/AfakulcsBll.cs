using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Afakulcs
{
    public class AfakulcsBll
    {
        public int Add(ossContext context, string sid, AfakulcsDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<AfakulcsDto, Models.Afakulcs>(dto);
            AfakulcsDal.Exists(context, entity);

            return AfakulcsDal.Add(context, entity);
        }

        public AfakulcsDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            return new AfakulcsDto();
        }

        public void Delete(ossContext context, string sid, AfakulcsDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            AfakulcsDal.Lock(context, dto.Afakulcskod, dto.Modositva);
            AfakulcsDal.CheckReferences(context, dto.Afakulcskod);
            var entity = AfakulcsDal.Get(context, dto.Afakulcskod);
            AfakulcsDal.Delete(context, entity);
        }

        public AfakulcsDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = AfakulcsDal.Get(context, key);
            return ObjectUtils.Convert<Models.Afakulcs, AfakulcsDto>(entity);
        }

        public static List<AfakulcsDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = AfakulcsDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Afakulcs, AfakulcsDto>(entities);
        }

        public int Update(ossContext context, string sid, AfakulcsDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            AfakulcsDal.Lock(context, dto.Afakulcskod, dto.Modositva);
            var entity = AfakulcsDal.Get(context, dto.Afakulcskod);
            ObjectUtils.Update(dto, entity);
            AfakulcsDal.ExistsAnother(context, entity);

            return AfakulcsDal.Update(context, entity);
        }

        public void ZoomCheck(ossContext context, string sid, int afakulcskod, string afakulcs)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);
            AfakulcsDal.ZoomCheck(context, afakulcskod, afakulcs);
        }
    }
}
