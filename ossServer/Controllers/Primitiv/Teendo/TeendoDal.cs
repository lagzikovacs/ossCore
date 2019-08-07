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
        public static async Task ExistsAsync(ossContext context, Models.Teendo entity)
        {
            if (await context.Teendo.AnyAsync(s => s.Particiokod == entity.Particiokod && 
                s.Teendo1 == entity.Teendo1))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Teendo1));
        }

        public static async Task<int> AddAsync(ossContext context, Models.Teendo entity)
        {
            Register.Creation(context, entity);
            await context.Teendo.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Teendokod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockteendo", "teendokod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Teendo> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Teendo
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Teendokod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Teendo.Teendokod)}={pKey}"));

            return result.First();
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Projektteendo.CountAsync(s => s.Teendokod == pKey);
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

        public static async Task DeleteAsync(ossContext context, Models.Teendo entity)
        {
            context.Teendo.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task ExistsAnotherAsync(ossContext context, Models.Teendo entity)
        {
            if (await context.Teendo.AnyAsync(s =>
                s.Particiokod == entity.Particiokod && s.Teendo1 == entity.Teendo1 && 
                s.Teendokod != entity.Teendokod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Teendo1));
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Teendo entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Teendokod;
        }

        public static async Task<List<Models.Teendo>> ReadAsync(ossContext context, string maszk)
        {
            return await context.Teendo.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Teendo1.Contains(maszk))
              .OrderBy(s => s.Teendo1)
              .ToListAsync();
        }

        public static async Task ZoomCheckAsync(ossContext context, int teendoKod, string teendo)
        {
            if (!await context.Teendo.AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Teendokod == teendoKod && s.Teendo1 == teendo))
                throw new Exception(string.Format(Messages.HibasZoom, "teendő"));
        }
    }
}
