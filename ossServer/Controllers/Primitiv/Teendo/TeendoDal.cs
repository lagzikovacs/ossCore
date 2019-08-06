using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Teendo
{
    public class TeendoDal
    {
        public static void Exists(ossContext context, Models.Teendo entity)
        {
            if (context.Teendo.Any(s => s.Particiokod == entity.Particiokod && 
                s.Teendo1 == entity.Teendo1))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Teendo1));
        }

        public static int Add(ossContext context, Models.Teendo entity)
        {
            Register.Creation(context, entity);
            context.Teendo.Add(entity);
            context.SaveChanges();

            return entity.Teendokod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockteendo", "teendokod", pKey, utoljaraModositva);
        }

        public static Models.Teendo Get(ossContext context, int pKey)
        {
            var result = context.Teendo
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Teendokod == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Teendo.Teendokod)}={pKey}"));
            return result.First();
        }

        public static void CheckReferences(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = context.Projektteendo.Count(s => s.Teendokod == pKey);
            if (n > 0)
                result.Add("PROJEKTTEENDO.TEENDO", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static void Delete(ossContext context, Models.Teendo entity)
        {
            context.Teendo.Remove(entity);
            context.SaveChanges();
        }

        public static void ExistsAnother(ossContext context, Models.Teendo entity)
        {
            if (context.Teendo.Any(s =>
                s.Particiokod == entity.Particiokod && s.Teendo1 == entity.Teendo1 && 
                s.Teendokod != entity.Teendokod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Teendo1));
        }

        public static int Update(ossContext context, Models.Teendo entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Teendokod;
        }

        public static List<Models.Teendo> Read(ossContext context, string maszk)
        {
            return context.Teendo.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Teendo1.Contains(maszk))
              .OrderBy(s => s.Teendo1)
              .ToList();
        }

        public static void ZoomCheck(ossContext context, int teendoKod, string teendo)
        {
            if (!context.Teendo.Any(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Teendokod == teendoKod && s.Teendo1 == teendo))
                throw new Exception(string.Format(Messages.HibasZoom, "teendő"));
        }
    }
}
