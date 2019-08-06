using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Afakulcs
{
    public class AfakulcsDal
    {
        public static async Task ExistsAsync(ossContext context, Models.Afakulcs entity)
        {
            if (await context.Afakulcs.AnyAsync(s => s.Particiokod == entity.Particiokod && s.Afakulcs1 == entity.Afakulcs1))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Afakulcs1));
        }

        public static async Task<int> AddAsync(ossContext context, Models.Afakulcs entity)
        {
            Register.Creation(context, entity);
            await context.Afakulcs.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Afakulcskod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockafakulcs", "afakulcskod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Afakulcs> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Afakulcs
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Afakulcskod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Afakulcs.Afakulcskod)}={pKey}"));

            return result.First();
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Cikk.CountAsync(s => s.Afakulcskod == pKey);
            if (n > 0)
                result.Add("CIKK.AFAKULCS", n);

            n = await context.Bizonylattetel.CountAsync(s => s.Afakulcskod == pKey);
            if (n > 0)
                result.Add("BIZONYLATTETEL.AFAKULCS", n);

            n = await context.Bizonylatafa.CountAsync(s => s.Afakulcskod == pKey);
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

        public static async Task DeleteAsync(ossContext context, Models.Afakulcs entity)
        {
            context.Afakulcs.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task ExistsAnotherAsync(ossContext context, Models.Afakulcs entity)
        {
            if (await context.Afakulcs
                .AnyAsync(s => s.Particiokod == entity.Particiokod && s.Afakulcs1 == entity.Afakulcs1 && 
                    s.Afakulcskod != entity.Afakulcskod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Afakulcs1));
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Afakulcs entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Afakulcskod;
        }

        public static async Task<List<Models.Afakulcs>> ReadAsync(ossContext context, string maszk)
        {
            return await context.Afakulcs.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Afakulcs1.Contains(maszk))
              .OrderBy(s => s.Afakulcs1)
              .ToListAsync();
        }

        public static async Task ZoomCheckAsync(ossContext context, int afakulcskod, string afakulcs)
        {
            if (!await context.Afakulcs
                .AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod &&
                          s.Afakulcskod == afakulcskod && s.Afakulcs1 == afakulcs))
                throw new Exception(string.Format(Messages.HibasZoom, "ÁFA kulcs"));
        }
    }
}
