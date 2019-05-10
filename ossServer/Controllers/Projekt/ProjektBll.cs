using ossServer.Controllers.Csoport;
using ossServer.Controllers.Primitiv.Penznem;
using ossServer.Controllers.Session;
using ossServer.Controllers.Ugyfel;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Projekt
{
    public class ProjektBll
    {
        public static int Add(ossContext context, string sid, ProjektDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKTMOD);

            var entity = ObjectUtils.Convert<ProjektDto, Models.Projekt>(dto);
            return ProjektDal.Add(context, entity);
        }

        public static ProjektDto CreateNew(ossContext context, string sid)
        {
            const string minta = "HUF";

            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKTMOD);

            var result = new ProjektDto
            {
                Keletkezett = DateTime.Now.Date,
                Vallalasiarnetto = 0,
                Muszakiallapot = "",
                Inverterallapot = "",
                Napelemallapot = ""
            };
            var lst = PenznemDal.Read(context, minta);
            if (lst.Count == 1)
            {
                result.Penznemkod = lst[0].Penznemkod;
                result.Penznem = lst[0].Penznem1;
            }

            return result;
        }

        public static void Delete(ossContext context, string sid, ProjektDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKTMOD);

            ProjektDal.Lock(context, dto.Projektkod, dto.Modositva);
            ProjektDal.CheckReferences(context, dto.Projektkod);
            var entity = ProjektDal.Get(context, dto.Projektkod);
            ProjektDal.Delete(context, entity);
        }

        private static ProjektDto Calc(Models.Projekt entity)
        {
            var result = ObjectUtils.Convert<Models.Projekt, ProjektDto>(entity);

            result.Penznem = entity.PenznemkodNavigation.Penznem1;
            result.Ugyfelnev = entity.UgyfelkodNavigation.Nev;
            result.Ugyfelcim = UgyfelBll.Cim(entity.UgyfelkodNavigation);
            result.Ugyfeltelefonszam = entity.UgyfelkodNavigation.Telefon;
            result.Ugyfelemail = entity.UgyfelkodNavigation.Email;

            return result;
        }

        public static ProjektDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            var entity = ProjektDal.Get(context, key);
            return Calc(entity);
        }

        public static List<ProjektDto> Select(ossContext context, string sid, int rekordTol, int lapMeret, 
            int statusz, List<SzMT> szmt, out int osszesRekord)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKT);

            var qry = ProjektDal.GetQuery(context, statusz, szmt);
            osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();

            var result = new List<ProjektDto>();
            foreach (var entity in entities)
                result.Add(Calc(entity));

            return result;
        }

        public static int Update(ossContext context, string sid, ProjektDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PROJEKTMOD);

            ProjektDal.Lock(context, dto.Projektkod, dto.Modositva);
            var entity = ProjektDal.Get(context, dto.Projektkod);

            ObjectUtils.Update(dto, entity);
            return ProjektDal.Update(context, entity);
        }
    }
}
