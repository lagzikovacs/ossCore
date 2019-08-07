using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Penznem
{
    public class PenznemDal
    {
        public static async Task ExistsAsync(ossContext context, Models.Penznem entity)
        {
            if (await context.Penznem.AnyAsync(s => s.Particiokod == entity.Particiokod && 
                s.Penznem1 == entity.Penznem1))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Penznem1));
        }

        public static async Task<int> AddAsync(ossContext context, Models.Penznem entity)
        {
            Register.Creation(context, entity);
            await context.Penznem.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Penznemkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockpenznem", "penznemkod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Penznem> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Penznem
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Penznemkod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Penznem.Penznemkod)}={pKey}"));

            return result.First();
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Bizonylat.CountAsync(s => s.Penznemkod == pKey);
            if (n > 0)
                result.Add("BIZONYLAT.PENZNEM", n);

            n = await context.Kifizetes.CountAsync(s => s.Penznemkod == pKey);
            if (n > 0)
                result.Add("KIFIZETES.PENZNEM", n);

            n = await context.Projekt.CountAsync(s => s.Penznemkod == pKey);
            if (n > 0)
                result.Add("PROJEKT.PENZNEM", n);

            n = await context.Penztar.CountAsync(s => s.Penznemkod == pKey);
            if (n > 0)
                result.Add("PENZTAR.PENZNEM", n);

            n = await context.Szamlazasirend.CountAsync(s => s.Penznemkod == pKey);
            if (n > 0)
                result.Add("SZAMLAZASIREND.PENZNEM", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static async Task DeleteAsync(ossContext context, Models.Penznem entity)
        {
            context.Penznem.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task ExistsAnotherAsync(ossContext context, Models.Penznem entity)
        {
            if (await context.Penznem.AnyAsync(s => s.Particiokod == entity.Particiokod && 
                s.Penznem1 == entity.Penznem1 && s.Penznemkod != entity.Penznemkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Penznem1));
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Penznem entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Penznemkod;
        }

        public static async Task<List<Models.Penznem>> ReadAsync(ossContext context, string maszk)
        {
            return await context.Penznem.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Penznem1.Contains(maszk))
              .OrderBy(s => s.Penznem1)
              .ToListAsync();
        }

        public static async Task ZoomCheckAsync(ossContext context, int penznemkod, string penznem)
        {
            if (!await context.Penznem.AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Penznemkod == penznemkod && s.Penznem1 == penznem))
                throw new Exception(string.Format(Messages.HibasZoom, "pénznem"));
        }
    }
}
