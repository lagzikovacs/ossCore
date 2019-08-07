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
        public static async Task ExistsAsync(ossContext context, Models.Fizetesimod entity)
        {
            if (await context.Fizetesimod.AnyAsync(s => s.Particiokod == entity.Particiokod && 
                s.Fizetesimod1 == entity.Fizetesimod1))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Fizetesimod1));
        }

        public static async Task<int> AddAsync(ossContext context, Models.Fizetesimod entity)
        {
            Register.Creation(context, entity);
            await context.Fizetesimod.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Fizetesimodkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockfizetesimod", "fizetesimodkod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Fizetesimod> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Fizetesimod
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Fizetesimodkod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Models.Fizetesimod.Fizetesimodkod)}={pKey}"));

            return result.First();
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Bizonylat.CountAsync(s => s.Fizetesimodkod == pKey);
            if (n > 0)
                result.Add("BIZONYLAT.FIZETESIMOD", n);

            n = await context.Kifizetes.CountAsync(s => s.Fizetesimodkod == pKey);
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

        public static async Task DeleteAsync(ossContext context, Models.Fizetesimod entity)
        {
            context.Fizetesimod.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task ExistsAnotherAsync(ossContext context, Models.Fizetesimod entity)
        {
            if (await context.Fizetesimod.AnyAsync(s => s.Particiokod == entity.Particiokod && 
                s.Fizetesimod1 == entity.Fizetesimod1 && s.Fizetesimodkod != entity.Fizetesimodkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Fizetesimod1));
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Fizetesimod entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Fizetesimodkod;
        }

        public static async Task<List<Models.Fizetesimod>> ReadAsync(ossContext context, string maszk)
        {
            return await context.Fizetesimod.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Fizetesimod1.Contains(maszk))
              .OrderBy(s => s.Fizetesimod1)
              .ToListAsync();
        }

        public static async Task ZoomCheckAsync(ossContext context, int fizetesimodKod, string fizetesimod)
        {
            if (!await context.Fizetesimod.AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Fizetesimodkod == fizetesimodKod && s.Fizetesimod1 == fizetesimod))
                throw new Exception(string.Format(Messages.HibasZoom, "fizetési mód"));
        }
    }
}
