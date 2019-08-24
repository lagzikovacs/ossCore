using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Tevekenyseg
{
    public class TevekenysegDal
    {
        public static async Task ExistsAsync(ossContext context, Models.Tevekenyseg entity)
        {
            if (await context.Tevekenyseg.AnyAsync(s => s.Particiokod == entity.Particiokod && 
                s.Tevekenyseg1 == entity.Tevekenyseg1))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Tevekenyseg1));
        }

        public static async Task<int> AddAsync(ossContext context, Models.Tevekenyseg entity)
        {
            Register.Creation(context, entity);
            await context.Tevekenyseg.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Tevekenysegkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("locktevekenyseg", "tevekenysegkod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Tevekenyseg> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Tevekenyseg
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Tevekenysegkod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Tevekenyseg.Tevekenysegkod)}={pKey}"));

            return result.First();
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Ugyfel.CountAsync(s => s.Tevekenysegkod == pKey);
            if (n > 0)
                result.Add("UGYFEL.TEVEKENYSEG", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static async Task DeleteAsync(ossContext context, Models.Tevekenyseg entity)
        {
            context.Tevekenyseg.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task ExistsAnotherAsync(ossContext context, Models.Tevekenyseg entity)
        {
            if (await context.Tevekenyseg.AnyAsync(s =>
                s.Particiokod == entity.Particiokod && s.Tevekenyseg1 == entity.Tevekenyseg1))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Tevekenyseg1));
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Tevekenyseg entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Tevekenysegkod;
        }

        public static async Task<List<Models.Tevekenyseg>> ReadAsync(ossContext context, string maszk)
        {
            return await context.Tevekenyseg.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Tevekenyseg1.Contains(maszk))
              .OrderBy(s => s.Tevekenyseg1)
              .ToListAsync();
        }

        public static async Task ZoomCheckAsync(ossContext context, int tevekenysegkod, string tevekenyseg)
        {
            if (!await context.Tevekenyseg.AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Tevekenysegkod == tevekenysegkod && s.Tevekenyseg1 == tevekenyseg))
                throw new Exception(string.Format(Messages.HibasZoom, "tevékenység"));
        }
    }
}
