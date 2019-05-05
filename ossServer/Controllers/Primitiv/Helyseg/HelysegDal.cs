using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Primitiv.Helyseg
{
    public class HelysegDal
    {
        public static void Exists(ossContext context, Models.Helyseg entity)
        {
            if (context.Helyseg.Any(s => s.Particiokod == entity.Particiokod && 
                s.Helysegnev == entity.Helysegnev))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Helysegnev));
        }

        public static int Add(ossContext context, Models.Helyseg entity)
        {
            Register.Creation(context, entity);
            context.Helyseg.Add(entity);
            context.SaveChanges();

            return entity.Helysegkod;
        }

        public async static void Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockhelyseg", "helysegkod", pKey, utoljaraModositva);
        }

        public static Models.Helyseg Get(ossContext context, int pKey)
        {
            var result = context.Helyseg
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Helysegkod == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Helyseg.Helysegkod)}={pKey}"));
            return result.First();
        }

        public static void CheckReferences(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = context.Ugyfel.Count(s => s.Helysegkod == pKey);
            if (n > 0)
                result.Add("UGYFEL.HELYSEG", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static void Delete(ossContext context, Models.Helyseg entity)
        {
            context.Helyseg.Remove(entity);
            context.SaveChanges();
        }

        public static void ExistsAnother(ossContext context, Models.Helyseg entity)
        {
            if (context.Helyseg.Any(s => s.Particiokod == entity.Particiokod && 
                s.Helysegnev == entity.Helysegnev && s.Helysegkod != entity.Helysegkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Helysegnev));
        }

        public static int Update(ossContext context, Models.Helyseg entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Helysegkod;
        }

        public static List<Models.Helyseg> Read(ossContext context, string maszk)
        {
            return context.Helyseg.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Helysegnev.Contains(maszk))
              .OrderBy(s => s.Helysegnev)
              .ToList();
        }

        public static void ZoomCheck(ossContext context, int helysegkod, string helysegnev)
        {
            if (!context.Helyseg.Any(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Helysegkod == helysegkod && s.Helysegnev == helysegnev))
                throw new Exception(string.Format(Messages.HibasZoom, "helység"));
        }
    }
}
