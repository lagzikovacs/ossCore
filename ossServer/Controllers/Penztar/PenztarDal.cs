using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Penztar
{
    public class PenztarDal
    {
        public static void Exists(ossContext context, Models.Penztar entity)
        {
            if (context.Penztar.Any(s => s.Particiokod == context.CurrentSession.Particiokod && 
                s.Penztar1 == entity.Penztar1 && s.Penznem == entity.Penznem))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Penztar1));
        }

        public static int Add(ossContext context, Models.Penztar entity)
        {
            Register.Creation(context, entity);
            context.Penztar.Add(entity);
            context.SaveChanges();

            return entity.Penztarkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockpenztar", "penztarkod", pKey, utoljaraModositva);
        }

        public static void CheckReferences(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = context.Penztartetel.Count(s => s.Penztarkod == pKey);
            if (n > 0)
                result.Add("PENZTARTETEL.PENZTARKOD", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static void Delete(ossContext context, Models.Penztar entity)
        {
            context.Penztar.Remove(entity);
            context.SaveChanges();
        }

        public static void ExistsAnother(ossContext context, Models.Penztar entity)
        {
            if (context.Penztar.Any(s => s.Particiokod == context.CurrentSession.Particiokod && 
                s.Penztar1 == entity.Penztar1 && s.Penztarkod != entity.Penztarkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Penztar1));
        }

        public static int Update(ossContext context, Models.Penztar entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Penztarkod;
        }

        public static Models.Penztar Get(ossContext context, int pKey)
        {
            var result = context.Penztar
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Penztarkod == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Penztar.Penztarkod)}={pKey}"));
            return result.First();
        }

        //egyből dto-t állít elő
        public static List<PenztarDto> Read(IOrderedQueryable<Models.Penztar> qry)
        {
            return qry.Select(s => new PenztarDto
            {
                Penztarkod = s.Penztarkod,
                Penztar1 = s.Penztar1,
                Penznemkod = s.Penznemkod,
                Penznem = s.Penznem,
                Nyitva = s.Nyitva,
                Egyenleg = s.Penztartetel.Count == 0
                ? 0
                : (s.Penztartetel.Sum(t => t.Bevetel) - s.Penztartetel.Sum(t => t.Kiadas)),
                Letrehozva = s.Letrehozva,
                Letrehozta = s.Letrehozta,
                Modositva = s.Modositva,
                Modositotta = s.Modositotta
            }).ToList();
        }
        public static List<PenztarDto> Read(ossContext context, string maszk)
        {
            var qry = context.Penztar
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod && 
                    s.Penztar1.Contains(maszk))
                .OrderBy(s => s.Penztar1);

            return Read(qry);
        }
        public static List<PenztarDto> Read(ossContext context, int penztarkod)
        {
            var qry = context.Penztar
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod && 
                    s.Penztarkod == penztarkod)
                .OrderBy(s => s.Penztar1);

            return Read(qry);
        }
        public static List<PenztarDto> ReadByCurrencyOpened(ossContext context, int penznemkod)
        {
            var qry = context.Penztar
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod && 
                    s.Penznemkod == penznemkod && s.Nyitva)
                .OrderBy(s => s.Penztar1);

            return Read(qry);
        }
    }
}
