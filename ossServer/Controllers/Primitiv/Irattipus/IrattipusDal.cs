using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Primitiv.Irattipus
{
    public class IrattipusDal
    {
        public static void Exists(ossContext context, Models.Irattipus entity)
        {
            if (context.Irattipus.Any(s => s.Particiokod == entity.Particiokod && 
                s.Irattipus1 == entity.Irattipus1))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Irattipus1));
        }

        public static int Add(ossContext context, Models.Irattipus entity)
        {
            Register.Creation(context, entity);
            context.Irattipus.Add(entity);
            context.SaveChanges();

            return entity.Irattipuskod;
        }

        public async static void Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockirattipus", "irattipuskod", pKey, utoljaraModositva);
        }

        public static Models.Irattipus Get(ossContext context, int pKey)
        {
            var result = context.Irattipus
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Irattipuskod == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Models.Irattipus.Irattipuskod)}={pKey}"));
            return result.First();
        }

        public static void CheckReferences(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = context.Irattipus.Count(s => s.Irattipuskod == pKey);
            if (n > 0)
                result.Add("IRAT.IRATTIPUS", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static void Delete(ossContext context, Models.Irattipus entity)
        {
            context.Irattipus.Remove(entity);
            context.SaveChanges();
        }

        public static void ExistsAnother(ossContext context, Models.Irattipus entity)
        {
            if (context.Irattipus.Any(s =>
                s.Particiokod == entity.Particiokod && s.Irattipus1 == entity.Irattipus1 &&
                 s.Irattipuskod != entity.Irattipuskod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Irattipus1));
        }

        public static int Update(ossContext context, Models.Irattipus entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Irattipuskod;
        }

        public static List<Models.Irattipus> Read(ossContext context, string maszk)
        {
            return context.Irattipus.AsNoTracking()
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
                .Where(s => s.Irattipus1.Contains(maszk))
                .OrderBy(s => s.Irattipus1)
                .ToList();
        }

        public static void ZoomCheck(ossContext context, int irattipuskod, string irattipus)
        {
            if (!context.Irattipus.Any(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Irattipuskod == irattipuskod && s.Irattipus1 == irattipus))
                throw new Exception(string.Format(Messages.HibasZoom, "irattipus"));
        }
    }
}
