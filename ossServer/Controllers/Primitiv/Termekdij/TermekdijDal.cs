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
        public static async Task ExistsAsync(ossContext context, Models.Termekdij entity)
        {
            if (await context.Termekdij.AnyAsync(s => s.Particiokod == entity.Particiokod && 
                s.Termekdijkt == entity.Termekdijkt))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Termekdijkt));
        }

        public static async Task<int> AddAsync(ossContext context, Models.Termekdij entity)
        {
            Register.Creation(context, entity);
            await context.Termekdij.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Termekdijkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("locktermekdij", "termekdijkod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Termekdij> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Termekdij
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Termekdijkod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Termekdij.Termekdijkod)}={pKey}"));

            return result.First();
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Cikk.CountAsync(s => s.Termekdijkod == pKey);
            if (n > 0)
                result.Add("CIKK.TERMEKDIJ", n);

            n = await context.Bizonylattetel.CountAsync(s => s.Termekdijkod == pKey);
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

        public static async Task DeleteAsync(ossContext context, Models.Termekdij entity)
        {
            context.Termekdij.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task ExistsAnotherAsync(ossContext context, Models.Termekdij entity)
        {
            if (await context.Termekdij.AnyAsync(s =>
                s.Particiokod == entity.Particiokod && s.Termekdijkt == entity.Termekdijkt && 
                s.Termekdijkod != entity.Termekdijkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Termekdijkt));
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Termekdij entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Termekdijkod;
        }

        public static async Task<List<Models.Termekdij>> ReadAsync(ossContext context, string maszk)
        {
            return await context.Termekdij.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Termekdijkt.Contains(maszk))
              .OrderBy(s => s.Termekdijkt)
              .ToListAsync();
        }

        public static async Task ZoomCheckAsync(ossContext context, int termekdijkod, string termekdijkt)
        {
            if (!await context.Termekdij.AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Termekdijkod == termekdijkod && s.Termekdijkt == termekdijkt))
                throw new Exception(string.Format(Messages.HibasZoom, "termékdíj"));
        }
    }
}
