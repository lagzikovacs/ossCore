using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Me
{
    public class MennyisegiegysegDal
    {
        public static void Exists(ossContext context, Mennyisegiegyseg entity)
        {
            if (context.Mennyisegiegyseg.Any(s => s.Particiokod == entity.Particiokod && 
                s.Me == entity.Me))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Me));
        }

        public static int Add(ossContext context, Mennyisegiegyseg entity)
        {
            Register.Creation(context, entity);
            context.Mennyisegiegyseg.Add(entity);
            context.SaveChanges();

            return entity.Mekod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockmennyisegiegyseg", "mekod", pKey, utoljaraModositva);
        }

        public static Mennyisegiegyseg Get(ossContext context, int pKey)
        {
            var result = context.Mennyisegiegyseg
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Mekod == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Mennyisegiegyseg.Mekod)}={pKey}"));
            return result.First();
        }

        public static void CheckReferences(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = context.Cikk.Count(s => s.Mekod == pKey);
            if (n > 0)
                result.Add("CIKK.ME", n);

            n = context.Bizonylattetel.Count(s => s.Mekod == pKey);
            if (n > 0)
                result.Add("BIZONYLATTETEL.ME", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static void Delete(ossContext context, Mennyisegiegyseg entity)
        {
            context.Mennyisegiegyseg.Remove(entity);
            context.SaveChanges();
        }

        public static void ExistsAnother(ossContext context, Mennyisegiegyseg entity)
        {
            if (context.Mennyisegiegyseg.Any(s => s.Particiokod == entity.Particiokod && 
                s.Me == entity.Me && s.Mekod != entity.Mekod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Me));
        }

        public static int Update(ossContext context, Mennyisegiegyseg entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Mekod;
        }

        public static List<Mennyisegiegyseg> Read(ossContext context, string maszk)
        {
            return context.Mennyisegiegyseg.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Me.Contains(maszk))
              .OrderBy(s => s.Me)
              .ToList();
        }

        public static void ZoomCheck(ossContext context, int mekod, string me)
        {
            if (!context.Mennyisegiegyseg.Any(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Mekod == mekod && s.Me == me))
                throw new Exception(string.Format(Messages.HibasZoom, "mennyiségi egység"));
        }
    }
}
