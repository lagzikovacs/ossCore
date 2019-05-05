using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Termekdij
{
    public class TermekdijBll
    {
        public int Add(ossContext context, string sid, TermekdijDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<TermekdijDto, Models.Termekdij>(dto);
            TermekdijDal.Exists(context, entity);
            return TermekdijDal.Add(context, entity);
        }

        public TermekdijDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);
            return new TermekdijDto { Termekdijegysegar = 0 };
        }

        public void Delete(ossContext context, string sid, TermekdijDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            TermekdijDal.Lock(context, dto.Termekdijkod, dto.Modositva);
            TermekdijDal.CheckReferences(context, dto.Termekdijkod);
            var entity = TermekdijDal.Get(context, dto.Termekdijkod);
            TermekdijDal.Delete(context, entity);
        }

        public TermekdijDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = TermekdijDal.Get(context, key);
            return ObjectUtils.Convert<Models.Termekdij, TermekdijDto>(entity);
        }

        public List<TermekdijDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = TermekdijDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Termekdij, TermekdijDto>(entities);
        }

        public int Update(ossContext context, string sid, TermekdijDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            TermekdijDal.Lock(context, dto.Termekdijkod, dto.Modositva);
            var entity = TermekdijDal.Get(context, dto.Termekdijkod);
            ObjectUtils.Update(dto, entity);
            TermekdijDal.ExistsAnother(context, entity);
            return TermekdijDal.Update(context, entity);
        }

        public void ZoomCheck(ossContext context, string sid, int termekdijkod, string termekdijkt)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            TermekdijDal.ZoomCheck(context, termekdijkod, termekdijkt);
        }
    }
}
