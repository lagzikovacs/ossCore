using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Helyseg
{
    public class HelysegBll
    {
        public int Add(ossContext context, string sid, HelysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<HelysegDto, Models.Helyseg>(dto);
            HelysegDal.Exists(context, entity);
            return HelysegDal.Add(context, entity);
        }

        public HelysegDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);
            return new HelysegDto();
        }

        public void Delete(ossContext context, string sid, HelysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            HelysegDal.Lock(context, dto.Helysegkod, dto.Modositva);
            HelysegDal.CheckReferences(context, dto.Helysegkod);
            var entity = HelysegDal.Get(context, dto.Helysegkod);
            HelysegDal.Delete(context, entity);
        }

        public HelysegDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = HelysegDal.Get(context, key);
            return ObjectUtils.Convert<Models.Helyseg, HelysegDto>(entity);
        }

        public List<HelysegDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = HelysegDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Helyseg, HelysegDto>(entities);
        }

        public int Update(ossContext context, string sid, HelysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            HelysegDal.Lock(context, dto.Helysegkod, dto.Modositva);
            var entity = HelysegDal.Get(context, dto.Helysegkod);
            ObjectUtils.Update(dto, entity);
            HelysegDal.ExistsAnother(context, entity);
            return HelysegDal.Update(context, entity);
        }

        public void ZoomCheck(ossContext context, string sid, int helysegkod, string helysegnev)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            HelysegDal.ZoomCheck(context, helysegkod, helysegnev);
        }
    }
}
