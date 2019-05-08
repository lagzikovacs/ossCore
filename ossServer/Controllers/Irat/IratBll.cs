﻿using ossServer.Controllers.Csoport;
using ossServer.Controllers.Dokumentum;
using ossServer.Controllers.Session;
using ossServer.Controllers.Ugyfel;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Irat
{
    public class IratBll
    {
        public static int Add(ossContext context, string sid, IratDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRAT);

            var entity = ObjectUtils.Convert<IratDto, Models.Irat>(dto);
            return IratDal.Add(context, entity);
        }

        public static IratDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRAT);

            return new IratDto { Keletkezett = DateTime.Now.Date, Irany = "Belső" };
        }

        public static void Delete(ossContext context, string sid, IratDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRATMOD);

            IratDal.Lock(context, dto.Iratkod, dto.Modositva);
            IratDal.CheckReferences(context, dto.Iratkod);
            var entity = IratDal.Get(context, dto.Iratkod);
            IratDal.Delete(context, entity);
        }

        private static IratDto Calc(Models.Irat entity)
        {
            var result = ObjectUtils.Convert<Models.Irat, IratDto>(entity);

            result.Irattipus = entity.IrattipuskodNavigation.Irattipus1;
            if (entity.UgyfelkodNavigation != null)
            {
                result.Ugyfelnev = entity.UgyfelkodNavigation.Nev;
                result.Ugyfelcim = UgyfelBll.Cim(entity.UgyfelkodNavigation);
            }

            return result;
        }

        public static IratDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRAT);

            var entity = IratDal.Get(context, key);
            return Calc(entity);
        }

        public static List<IratDto> Select(ossContext context, string sid, int rekordTol, int lapMeret, 
            List<SzMT> szmt, out int osszesRekord)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRAT);

            var qry = IratDal.GetQuery(context, szmt);
            osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();

            var result = new List<IratDto>();
            foreach (var entity in entities)
                result.Add(Calc(entity));

            return result;
        }

        public static int Update(ossContext context, string sid, IratDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRATMOD);

            IratDal.Lock(context, dto.Iratkod, dto.Modositva);
            var entity = IratDal.Get(context, dto.Iratkod);
            ObjectUtils.Update(dto, entity);
            return IratDal.Update(context, entity);
        }


        //sql tranzakcióban működik, kis fájlok legyenek
        public static FajlBuf Letoltes(ossContext context, string sid, int iratKod)
        {
            IratDal.Get(context, iratKod);
            var lstDokumentum = DokumentumDal.Select(context, iratKod);
            if (lstDokumentum.Count != 1)
                throw new Exception("Nincs pontosan egy dokumentum!");

            var entityDokumentum = DokumentumBll.Letoltes(context, sid, lstDokumentum[0].Dokumentumkod);
            var fb = DokumentumBll.LetoltesFajl(entityDokumentum, 0, lstDokumentum[0].Meret);

            return fb;
        }
    }
}