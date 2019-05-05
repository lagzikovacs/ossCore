using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Fizetesimod
{
    public class FizetesimodBll
    {
        public int Add(ossContext context, string sid, FizetesimodDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<FizetesimodDto, Models.Fizetesimod>(dto);
            FizetesimodDal.Exists(context, entity);
            return FizetesimodDal.Add(context, entity);
        }

        public FizetesimodDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);
            return new FizetesimodDto();
        }

        public void Delete(ossContext context, string sid, FizetesimodDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            FizetesimodDal.Lock(context, dto.Fizetesimodkod, dto.Modositva);
            FizetesimodDal.CheckReferences(context, dto.Fizetesimodkod);
            var entity = FizetesimodDal.Get(context, dto.Fizetesimodkod);
            FizetesimodDal.Delete(context, entity);
        }

        public FizetesimodDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = FizetesimodDal.Get(context, key);
            return ObjectUtils.Convert<Models.Fizetesimod, FizetesimodDto>(entity);
        }

        public List<FizetesimodDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = FizetesimodDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Fizetesimod, FizetesimodDto>(entities);
        }

        public int Update(ossContext context, string sid, FizetesimodDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            FizetesimodDal.Lock(context, dto.Fizetesimodkod, dto.Modositva);
            var entity = FizetesimodDal.Get(context, dto.Fizetesimodkod);
            ObjectUtils.Update(dto, entity);
            FizetesimodDal.ExistsAnother(context, entity);
            return FizetesimodDal.Update(context, entity);
        }

        public void ZoomCheck(ossContext context, string sid, int fizetesimodKod, string fizetesimod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            FizetesimodDal.ZoomCheck(context, fizetesimodKod, fizetesimod);
        }
    }
}
