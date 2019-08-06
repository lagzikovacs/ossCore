using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Termekdij
{
    public class TermekdijDal
    {
        public static void Exists(ossContext context, Models.Termekdij entity)
        {
            if (context.Termekdij.Any(s => s.Particiokod == entity.Particiokod && 
                s.Termekdijkt == entity.Termekdijkt))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Termekdijkt));
        }

        public static int Add(ossContext context, Models.Termekdij entity)
        {
            Register.Creation(context, entity);
            context.Termekdij.Add(entity);
            context.SaveChanges();

            return entity.Termekdijkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("locktermekdij", "termekdijkod", pKey, utoljaraModositva);
        }

        public static Models.Termekdij Get(ossContext context, int pKey)
        {
            var result = context.Termekdij
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Termekdijkod == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Termekdij.Termekdijkod)}={pKey}"));
            return result.First();
        }

        public static void CheckReferences(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = context.Cikk.Count(s => s.Termekdijkod == pKey);
            if (n > 0)
                result.Add("CIKK.TERMEKDIJ", n);

            n = context.Bizonylattetel.Count(s => s.Termekdijkod == pKey);
            if (n > 0)
                result.Add("BIZONYLATTETEL.TERMEKDIJ", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static void Delete(ossContext context, Models.Termekdij entity)
        {
            context.Termekdij.Remove(entity);
            context.SaveChanges();
        }

        public static void ExistsAnother(ossContext context, Models.Termekdij entity)
        {
            if (context.Termekdij.Any(s =>
                s.Particiokod == entity.Particiokod && s.Termekdijkt == entity.Termekdijkt && 
                s.Termekdijkod != entity.Termekdijkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Termekdijkt));
        }

        public static int Update(ossContext context, Models.Termekdij entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Termekdijkod;
        }

        public static List<Models.Termekdij> Read(ossContext context, string maszk)
        {
            return context.Termekdij.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Termekdijkt.Contains(maszk))
              .OrderBy(s => s.Termekdijkt)
              .ToList();
        }

        public static void ZoomCheck(ossContext context, int termekdijkod, string termekdijkt)
        {
            if (!context.Termekdij.Any(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Termekdijkod == termekdijkod && s.Termekdijkt == termekdijkt))
                throw new Exception(string.Format(Messages.HibasZoom, "termékdíj"));
        }
    }
}
