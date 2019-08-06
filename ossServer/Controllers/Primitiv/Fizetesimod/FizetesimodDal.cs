using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Fizetesimod
{
    public class FizetesimodDal
    {
        public static void Exists(ossContext context, Models.Fizetesimod entity)
        {
            if (context.Fizetesimod.Any(s => s.Particiokod == entity.Particiokod && 
                s.Fizetesimod1 == entity.Fizetesimod1))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Fizetesimod1));
        }

        public static int Add(ossContext context, Models.Fizetesimod entity)
        {
            Register.Creation(context, entity);
            context.Fizetesimod.Add(entity);
            context.SaveChanges();

            return entity.Fizetesimodkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockfizetesimod", "fizetesimodkod", pKey, utoljaraModositva);
        }

        public static Models.Fizetesimod Get(ossContext context, int pKey)
        {
            var result = context.Fizetesimod
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Fizetesimodkod == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Models.Fizetesimod.Fizetesimodkod)}={pKey}"));
            return result.First();
        }

        public static void CheckReferences(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = context.Bizonylat.Count(s => s.Fizetesimodkod == pKey);
            if (n > 0)
                result.Add("BIZONYLAT.FIZETESIMOD", n);

            n = context.Kifizetes.Count(s => s.Fizetesimodkod == pKey);
            if (n > 0)
                result.Add("KIFIZETES.FIZETESIMOD", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static void Delete(ossContext context, Models.Fizetesimod entity)
        {
            context.Fizetesimod.Remove(entity);
            context.SaveChanges();
        }

        public static void ExistsAnother(ossContext context, Models.Fizetesimod entity)
        {
            if (context.Fizetesimod.Any(s => s.Particiokod == entity.Particiokod && 
                s.Fizetesimod1 == entity.Fizetesimod1 && s.Fizetesimodkod != entity.Fizetesimodkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Fizetesimod1));
        }

        public static int Update(ossContext context, Models.Fizetesimod entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Fizetesimodkod;
        }

        public static List<Models.Fizetesimod> Read(ossContext context, string maszk)
        {
            return context.Fizetesimod.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Fizetesimod1.Contains(maszk))
              .OrderBy(s => s.Fizetesimod1)
              .ToList();
        }

        public static void ZoomCheck(ossContext context, int fizetesimodKod, string fizetesimod)
        {
            if (!context.Fizetesimod.Any(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Fizetesimodkod == fizetesimodKod && s.Fizetesimod1 == fizetesimod))
                throw new Exception(string.Format(Messages.HibasZoom, "fizetési mód"));
        }
    }
}
