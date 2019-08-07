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
        public static async Task ExistsAsync(ossContext context, Mennyisegiegyseg entity)
        {
            if (await context.Mennyisegiegyseg.AnyAsync(s => s.Particiokod == entity.Particiokod && 
                s.Me == entity.Me))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Me));
        }

        public static async Task<int> AddAsync(ossContext context, Mennyisegiegyseg entity)
        {
            Register.Creation(context, entity);
            await context.Mennyisegiegyseg.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Mekod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockmennyisegiegyseg", "mekod", pKey, utoljaraModositva);
        }

        public static async Task<Mennyisegiegyseg> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Mennyisegiegyseg
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Mekod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Mennyisegiegyseg.Mekod)}={pKey}"));

            return result.First();
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Cikk.CountAsync(s => s.Mekod == pKey);
            if (n > 0)
                result.Add("CIKK.ME", n);

            n = await context.Bizonylattetel.CountAsync(s => s.Mekod == pKey);
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

        public static async Task DeleteAsync(ossContext context, Mennyisegiegyseg entity)
        {
            context.Mennyisegiegyseg.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task ExistsAnotherAsync(ossContext context, Mennyisegiegyseg entity)
        {
            if (await context.Mennyisegiegyseg.AnyAsync(s => s.Particiokod == entity.Particiokod && 
                s.Me == entity.Me && s.Mekod != entity.Mekod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Me));
        }

        public static async Task<int> UpdateAsync(ossContext context, Mennyisegiegyseg entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Mekod;
        }

        public static async Task<List<Mennyisegiegyseg>> ReadAsync(ossContext context, string maszk)
        {
            return await context.Mennyisegiegyseg.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Me.Contains(maszk))
              .OrderBy(s => s.Me)
              .ToListAsync();
        }

        public static async Task ZoomCheckAsync(ossContext context, int mekod, string me)
        {
            if (!await context.Mennyisegiegyseg.AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Mekod == mekod && s.Me == me))
                throw new Exception(string.Format(Messages.HibasZoom, "mennyiségi egység"));
        }
    }
}
