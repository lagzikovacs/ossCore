﻿using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Penztar
{
    public class PenztarBll
    {
        public static int Add(ossContext context, string sid, PenztarDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PENZTARMOD);

            var entity = ObjectUtils.Convert<PenztarDto, Models.Penztar>(dto);
            PenztarDal.Exists(context, entity);
            return PenztarDal.Add(context, entity);
        }

        public static PenztarDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PENZTARMOD);

            return new PenztarDto { Nyitva = true };
        }

        public static void Delete(ossContext context, string sid, PenztarDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PENZTARMOD);

            PenztarDal.Lock(context, dto.Penztarkod, dto.Modositva);
            PenztarDal.CheckReferences(context, dto.Penztarkod);
            var entity = PenztarDal.Get(context, dto.Penztarkod);
            PenztarDal.Delete(context, entity);
        }

        public static PenztarDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PENZTAR);

            var entity = PenztarDal.Get(context, key);
            return ObjectUtils.Convert<Models.Penztar, PenztarDto>(entity);
        }

        public static List<PenztarDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PENZTAR);

            return PenztarDal.Read(context, maszk);
        }
        public static List<PenztarDto> Read(ossContext context, string sid, int penztarkod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PENZTAR);

            return PenztarDal.Read(context, penztarkod);
        }
        public static List<PenztarDto> ReadByCurrencyOpened(ossContext context, string sid, int penznemkod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PENZTAR);

            return PenztarDal.ReadByCurrencyOpened(context, penznemkod);
        }

        public static int Update(ossContext context, string sid, PenztarDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PENZTARMOD);

            PenztarDal.Lock(context, dto.Penztarkod, dto.Modositva);
            var entity = PenztarDal.Get(context, dto.Penztarkod);
            ObjectUtils.Update(dto, entity);
            PenztarDal.ExistsAnother(context, entity);
            return PenztarDal.Update(context, entity);
        }
    }
}