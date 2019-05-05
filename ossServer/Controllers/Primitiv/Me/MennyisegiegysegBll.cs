using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Me
{
    public class MennyisegiegysegBll
    {
        public int Add(ossContext context, string sid, MennyisegiegysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<MennyisegiegysegDto, Mennyisegiegyseg>(dto);
            MennyisegiegysegDal.Exists(context, entity);
            return MennyisegiegysegDal.Add(context, entity);
        }

        public MennyisegiegysegDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);
            return new MennyisegiegysegDto();
        }

        public void Delete(ossContext context, string sid, MennyisegiegysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            MennyisegiegysegDal.Lock(context, dto.Mekod, dto.Modositva);
            MennyisegiegysegDal.CheckReferences(context, dto.Mekod);
            var entity = MennyisegiegysegDal.Get(context, dto.Mekod);
            MennyisegiegysegDal.Delete(context, entity);
        }

        public MennyisegiegysegDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = MennyisegiegysegDal.Get(context, key);
            return ObjectUtils.Convert<Models.Mennyisegiegyseg, MennyisegiegysegDto>(entity);
        }

        public List<MennyisegiegysegDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = MennyisegiegysegDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Mennyisegiegyseg, MennyisegiegysegDto>(entities);
        }

        public int Update(ossContext context, string sid, MennyisegiegysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            MennyisegiegysegDal.Lock(context, dto.Mekod, dto.Modositva);
            var entity = MennyisegiegysegDal.Get(context, dto.Mekod);
            ObjectUtils.Update(dto, entity);
            MennyisegiegysegDal.ExistsAnother(context, entity);
            return MennyisegiegysegDal.Update(context, entity);
        }

        public void ZoomCheck(ossContext context, string sid, int mekod, string me)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            MennyisegiegysegDal.ZoomCheck(context, mekod, me);
        }
    }
}
