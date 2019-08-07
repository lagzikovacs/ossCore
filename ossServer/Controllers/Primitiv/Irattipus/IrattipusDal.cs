using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Irattipus
{
    public class IrattipusDal
    {
        public static async Task ExistsAsync(ossContext context, Models.Irattipus entity)
        {
            if (await context.Irattipus.AnyAsync(s => s.Particiokod == entity.Particiokod && 
                s.Irattipus1 == entity.Irattipus1))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Irattipus1));
        }

        public static async Task<int> AddAsync(ossContext context, Models.Irattipus entity)
        {
            Register.Creation(context, entity);
            await context.Irattipus.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Irattipuskod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockirattipus", "irattipuskod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Irattipus> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Irattipus
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Irattipuskod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Models.Irattipus.Irattipuskod)}={pKey}"));

            return result.First();
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Irat.CountAsync(s => s.Irattipuskod == pKey);
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

        public static async Task DeleteAsync(ossContext context, Models.Irattipus entity)
        {
            context.Irattipus.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task ExistsAnotherAsync(ossContext context, Models.Irattipus entity)
        {
            if (await context.Irattipus.AnyAsync(s =>
                s.Particiokod == entity.Particiokod && s.Irattipus1 == entity.Irattipus1 &&
                 s.Irattipuskod != entity.Irattipuskod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Irattipus1));
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Irattipus entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Irattipuskod;
        }

        public static async Task<List<Models.Irattipus>> ReadAsync(ossContext context, string maszk)
        {
            return await context.Irattipus.AsNoTracking()
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
                .Where(s => s.Irattipus1.Contains(maszk))
                .OrderBy(s => s.Irattipus1)
                .ToListAsync();
        }

        public static async Task ZoomCheckAsync(ossContext context, int irattipuskod, string irattipus)
        {
            if (!await context.Irattipus.AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Irattipuskod == irattipuskod && s.Irattipus1 == irattipus))
                throw new Exception(string.Format(Messages.HibasZoom, "irattipus"));
        }
    }
}
