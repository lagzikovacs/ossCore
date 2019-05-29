using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ugyfel
{
    public class UgyfelBll
    {
        public static int Add(ossContext context, string sid, UgyfelDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEKMOD);

            var entity = ObjectUtils.Convert<UgyfelDto, Models.Ugyfel>(dto);
            UgyfelDal.Exists(context, entity);
            return UgyfelDal.Add(context, entity);
        }

        public static UgyfelDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEKMOD);

            return new UgyfelDto { Csoport = 0 };
        }

        public static void Delete(ossContext context, string sid, UgyfelDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEKMOD);

            UgyfelDal.Lock(context, dto.Ugyfelkod, dto.Modositva);
            UgyfelDal.CheckReferences(context, dto.Ugyfelkod);
            var entity = UgyfelDal.Get(context, dto.Ugyfelkod);
            UgyfelDal.Delete(context, entity);
        }

        public static string Cim(Models.Ugyfel entity)
        {
            var result = "";
            if (entity.HelysegkodNavigation != null)
                result = $"{entity.Iranyitoszam} {entity.HelysegkodNavigation.Helysegnev}, {entity.Kozterulet} {entity.Kozterulettipus} {entity.Hazszam}";

            return result;
        }

        private static UgyfelDto Calc(Models.Ugyfel entity)
        {
            var result = ObjectUtils.Convert<Models.Ugyfel, UgyfelDto>(entity);

            if (entity.HelysegkodNavigation != null)
            {
                result.Helysegnev = entity.HelysegkodNavigation.Helysegnev;
                result.Cim = Cim(entity);
            }

            return result;
        }

        public static UgyfelDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEK);

            var entity = UgyfelDal.Get(context, key);
            return Calc(entity);
        }

        public static List<UgyfelDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEK);

            var entities = UgyfelDal.Read(context, maszk);
            var result = new List<UgyfelDto>();

            foreach (var entity in entities)
                result.Add(Calc(entity));
            return result;
        }

        public static List<UgyfelDto> Select(ossContext context, string sid, int rekordTol, int lapMeret, 
            List<SzMT> szmt, out int osszesRekord)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEK);

            var qry = UgyfelDal.GetQuery(context, szmt);
            osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
            var result = new List<UgyfelDto>();

            foreach (var entity in entities)
                result.Add(Calc(entity));

            return result;
        }

        public static int Update(ossContext context, string sid, UgyfelDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEKMOD);

            UgyfelDal.Lock(context, dto.Ugyfelkod, dto.Modositva);
            var entity = UgyfelDal.Get(context, dto.Ugyfelkod);

            ObjectUtils.Update(dto, entity);
            UgyfelDal.ExistsAnother(context, entity);
            return UgyfelDal.Update(context, entity);
        }

        public static void ZoomCheck(ossContext context, string sid, int ugyfelkod, string ugyfel)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEK);

            UgyfelDal.ZoomCheck(context, ugyfelkod, ugyfel);
        }
    }
}
