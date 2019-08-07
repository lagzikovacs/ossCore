using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Helyseg
{
    public class HelysegDal
    {
        public static async Task ExistsAsync(ossContext context, Models.Helyseg entity)
        {
            if (await context.Helyseg.AnyAsync(s => s.Particiokod == entity.Particiokod && 
                s.Helysegnev == entity.Helysegnev))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Helysegnev));
        }

        public static async Task<int> AddAsync(ossContext context, Models.Helyseg entity)
        {
            Register.Creation(context, entity);
            await context.Helyseg.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Helysegkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockhelyseg", "helysegkod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Helyseg> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Helyseg
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Helysegkod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Helyseg.Helysegkod)}={pKey}"));

            return result.First();
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Ugyfel.CountAsync(s => s.Helysegkod == pKey);
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

        public static async Task DeleteAsync(ossContext context, Models.Helyseg entity)
        {
            context.Helyseg.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task ExistsAnotherAsync(ossContext context, Models.Helyseg entity)
        {
            if (await context.Helyseg.AnyAsync(s => s.Particiokod == entity.Particiokod && 
                s.Helysegnev == entity.Helysegnev && s.Helysegkod != entity.Helysegkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Helysegnev));
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Helyseg entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Helysegkod;
        }

        public static async Task<List<Models.Helyseg>> ReadAsync(ossContext context, string maszk)
        {
            return await context.Helyseg.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Helysegnev.Contains(maszk))
              .OrderBy(s => s.Helysegnev)
              .ToListAsync();
        }

        public static async Task ZoomCheckAsync(ossContext context, int helysegkod, string helysegnev)
        {
            if (!await context.Helyseg.AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Helysegkod == helysegkod && s.Helysegnev == helysegnev))
                throw new Exception(string.Format(Messages.HibasZoom, "helység"));
        }
    }
}
