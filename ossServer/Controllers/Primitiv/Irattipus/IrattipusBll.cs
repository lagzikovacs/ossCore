using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Irattipus
{
    public class IrattipusBll
    {
        public int Add(ossContext context, string sid, IrattipusDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<IrattipusDto, Models.Irattipus>(dto);
            IrattipusDal.Exists(context, entity);
            return IrattipusDal.Add(context, entity);
        }

        public IrattipusDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);
            return new IrattipusDto();
        }

        public void Delete(ossContext context, string sid, IrattipusDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            IrattipusDal.Lock(context, dto.Irattipuskod, dto.Modositva);
            IrattipusDal.CheckReferences(context, dto.Irattipuskod);
            var entity = IrattipusDal.Get(context, dto.Irattipuskod);
            IrattipusDal.Delete(context, entity);
        }

        public IrattipusDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = IrattipusDal.Get(context, key);
            return ObjectUtils.Convert<Models.Irattipus, IrattipusDto>(entity);
        }

        public List<IrattipusDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = IrattipusDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Irattipus, IrattipusDto>(entities);
        }

        public int Update(ossContext context, string sid, IrattipusDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            IrattipusDal.Lock(context, dto.Irattipuskod, dto.Modositva);
            var entity = IrattipusDal.Get(context, dto.Irattipuskod);
            ObjectUtils.Update(dto, entity);
            IrattipusDal.ExistsAnother(context, entity);
            return IrattipusDal.Update(context, entity);
        }

        public void ZoomCheck(ossContext context, string sid, int irattipuskod, string irattipus)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            IrattipusDal.ZoomCheck(context, irattipuskod, irattipus);
        }
    }
}
