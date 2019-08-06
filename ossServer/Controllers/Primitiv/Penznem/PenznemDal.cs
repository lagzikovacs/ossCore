using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Penznem
{
    public class PenznemDal
    {
        public static void Exists(ossContext context, Models.Penznem entity)
        {
            if (context.Penznem.Any(s => s.Particiokod == entity.Particiokod && 
                s.Penznem1 == entity.Penznem1))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Penznem1));
        }

        public static int Add(ossContext context, Models.Penznem entity)
        {
            Register.Creation(context, entity);
            context.Penznem.Add(entity);
            context.SaveChanges();

            return entity.Penznemkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockpenznem", "penznemkod", pKey, utoljaraModositva);
        }

        public static Models.Penznem Get(ossContext context, int pKey)
        {
            var result = context.Penznem
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Penznemkod == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Penznem.Penznemkod)}={pKey}"));
            return result.First();
        }

        public static void CheckReferences(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = context.Bizonylat.Count(s => s.Penznemkod == pKey);
            if (n > 0)
                result.Add("BIZONYLAT.PENZNEM", n);

            n = context.Kifizetes.Count(s => s.Penznemkod == pKey);
            if (n > 0)
                result.Add("KIFIZETES.PENZNEM", n);

            n = context.Projekt.Count(s => s.Penznemkod == pKey);
            if (n > 0)
                result.Add("PROJEKT.PENZNEM", n);

            n = context.Penztar.Count(s => s.Penznemkod == pKey);
            if (n > 0)
                result.Add("PENZTAR.PENZNEM", n);

            n = context.Szamlazasirend.Count(s => s.Penznemkod == pKey);
            if (n > 0)
                result.Add("SZAMLAZASIREND.PENZNEM", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static void Delete(ossContext context, Models.Penznem entity)
        {
            context.Penznem.Remove(entity);
            context.SaveChanges();
        }

        public static void ExistsAnother(ossContext context, Models.Penznem entity)
        {
            if (context.Penznem.Any(s => s.Particiokod == entity.Particiokod && 
                s.Penznem1 == entity.Penznem1 && s.Penznemkod != entity.Penznemkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Penznem1));
        }

        public static int Update(ossContext context, Models.Penznem entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Penznemkod;
        }

        public static List<Models.Penznem> Read(ossContext context, string maszk)
        {
            return context.Penznem.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Penznem1.Contains(maszk))
              .OrderBy(s => s.Penznem1)
              .ToList();
        }

        public static void ZoomCheck(ossContext context, int penznemkod, string penznem)
        {
            if (!context.Penznem.Any(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Penznemkod == penznemkod && s.Penznem1 == penznem))
                throw new Exception(string.Format(Messages.HibasZoom, "pénznem"));
        }
    }
}
