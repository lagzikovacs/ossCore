using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Primitiv.Afakulcs
{
    public class AfakulcsDal
    {
        public static void Exists(ossContext context, Models.Afakulcs entity)
        {
            if (context.Afakulcs.Any(s => s.Particiokod == entity.Particiokod && s.Afakulcs1 == entity.Afakulcs1))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Afakulcs1));
        }

        public static int Add(ossContext context, Models.Afakulcs entity)
        {
            Register.Creation(context, entity);
            context.Afakulcs.Add(entity);
            context.SaveChanges();

            return entity.Afakulcskod;
        }

        public async static void Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockafakulcs", "felhasznalokod", pKey, utoljaraModositva);
        }

        public static Models.Afakulcs Get(ossContext context, int pKey)
        {
            var result = context.Afakulcs
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Afakulcskod == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Afakulcs.Afakulcskod)}={pKey}"));
            return result.First();
        }

        public static void CheckReferences(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = context.Cikk.Count(s => s.Afakulcskod == pKey);
            if (n > 0)
                result.Add("CIKK.AFAKULCS", n);

            n = context.Bizonylattetel.Count(s => s.Afakulcskod == pKey);
            if (n > 0)
                result.Add("BIZONYLATTETEL.AFAKULCS", n);

            n = context.Bizonylatafa.Count(s => s.Afakulcskod == pKey);
            if (n > 0)
                result.Add("BIZONYLATAFA.AFAKULCS", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static void Delete(ossContext context, Models.Afakulcs entity)
        {
            context.Afakulcs.Remove(entity);
            context.SaveChanges();
        }

        public static void ExistsAnother(ossContext context, Models.Afakulcs entity)
        {
            if (context.Afakulcs
                .Any(s => s.Particiokod == entity.Particiokod && s.Afakulcs1 == entity.Afakulcs1 && 
                    s.Afakulcskod != entity.Afakulcskod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Afakulcs1));
        }

        public static int Update(ossContext context, Models.Afakulcs entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Afakulcskod;
        }

        public static List<Models.Afakulcs> Read(ossContext context, string maszk)
        {
            return context.Afakulcs.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Afakulcs1.Contains(maszk))
              .OrderBy(s => s.Afakulcs1)
              .ToList();
        }

        public static void ZoomCheck(ossContext context, int afakulcskod, string afakulcs)
        {
            if (!context.Afakulcs
                .Any(s => s.Particiokod == context.CurrentSession.Particiokod &&
                          s.Afakulcskod == afakulcskod && s.Afakulcs1 == afakulcs))
                throw new Exception(string.Format(Messages.HibasZoom, "ÁFA kulcs"));
        }
    }
}
