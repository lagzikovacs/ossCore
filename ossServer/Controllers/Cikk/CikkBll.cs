using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Cikk
{
    public class CikkBll
    {
        public static int Add(ossContext context, string sid, CikkDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CIKK);

            var entity = ObjectUtils.Convert<CikkDto, Models.Cikk>(dto);
            CikkDal.Exists(context, entity);
            return CikkDal.Add(context, entity);
        }

        public static CikkDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            return new CikkDto();
        }

        public static void Delete(ossContext context, string sid, CikkDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CIKK);

            CikkDal.Lock(context, dto.Cikkkod, dto.Modositva);
            CikkDal.CheckReferences(context, dto.Cikkkod);
            var entity = CikkDal.Get(context, dto.Cikkkod);
            CikkDal.Delete(context, entity);
        }

        public static CikkDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CIKK);

            var entity = CikkDal.Get(context, key);
            var result = ObjectUtils.Convert<Models.Cikk, CikkDto>(entity);

            result.Afakulcs = entity.AFAKULCS.AFAKULCS1;
            result.Afamerteke = entity.AFAKULCS.AFAMERTEKE;
            result.Me = entity.MENNYISEGIEGYSEG.ME;

            if (entity.TERMEKDIJ != null)
            {
                result.TERMEKDIJKT = entity.TERMEKDIJ.TERMEKDIJKT;
                result.TERMEKDIJMEGNEVEZES = entity.TERMEKDIJ.TERMEKDIJMEGNEVEZES;
                result.TERMEKDIJEGYSEGAR = entity.TERMEKDIJ.TERMEKDIJEGYSEGAR;
            }

            return result;
        }

        public static List<CikkDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CIKK);

            var entities = CikkDal.Read(context, maszk);
            var result = new List<CikkDto>();

            foreach (var entity in entities)
            {
                var dto = ObjectUtils.Convert<Cikk, CikkDto>(entity);
                dto.AFAKULCS = entity.AFAKULCS.AFAKULCS1;
                dto.AFAMERTEKE = entity.AFAKULCS.AFAMERTEKE;
                dto.ME = entity.MENNYISEGIEGYSEG.ME;

                if (entity.TERMEKDIJ != null)
                {
                    dto.TERMEKDIJKT = entity.TERMEKDIJ.TERMEKDIJKT;
                    dto.TERMEKDIJMEGNEVEZES = entity.TERMEKDIJ.TERMEKDIJMEGNEVEZES;
                    dto.TERMEKDIJEGYSEGAR = entity.TERMEKDIJ.TERMEKDIJEGYSEGAR;
                }

                result.Add(dto);
            }

            return result;
        }

        public static int Update(ossContext context, string sid, CikkDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CIKK);

            CikkDal.Lock(context, dto.Cikkkod, dto.Modositva);
            var entity = CikkDal.Get(context, dto.Cikkkod);

            ObjectUtils.Update(dto, entity);
            CikkDal.ExistsAnother(context, entity);
            return CikkDal.Update(context, entity);
        }

        public static List<CikkDto> Select(ossContext context, string sid, int rekordTol, int lapMeret, 
            List<SzMT> szmt, out int osszesRekord)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CIKK);

            var qry = CikkDal.GetQuery(context, szmt);
            osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
            var result = new List<CikkDto>();

            foreach (var entity in entities)
            {
                var dto = ObjectUtils.Convert<CIKK, CikkDto>(entity);
                dto.AFAKULCS = entity.AFAKULCS.AFAKULCS1;
                dto.AFAMERTEKE = entity.AFAKULCS.AFAMERTEKE;
                dto.ME = entity.MENNYISEGIEGYSEG.ME;

                if (entity.TERMEKDIJ != null)
                {
                    dto.TERMEKDIJKT = entity.TERMEKDIJ.TERMEKDIJKT;
                    dto.TERMEKDIJMEGNEVEZES = entity.TERMEKDIJ.TERMEKDIJMEGNEVEZES;
                    dto.TERMEKDIJEGYSEGAR = entity.TERMEKDIJ.TERMEKDIJEGYSEGAR;
                }

                result.Add(dto);
            }

            return result;
        }

        public static List<CikkMozgasTetelDto> Mozgas(ossContext context, string sid, int cikkKod, 
            BizonylatTipus bizonylatTipus)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CIKK);

            return CikkDal.Mozgas(context, cikkKod, bizonylatTipus);
        }

        public static void ZoomCheck(ossContext context, string sid, int Cikkkod, string Cikk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.CIKK);

            CikkDal.ZoomCheck(context, Cikkkod, Cikk);
        }
    }
}
