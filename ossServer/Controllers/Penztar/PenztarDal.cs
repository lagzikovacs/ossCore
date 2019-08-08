using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Penztar
{
    public class PenztarDal
    {
        public static async Task ExistsAsync(ossContext context, Models.Penztar entity)
        {
            if (await context.Penztar.AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod && 
                s.Penztar1 == entity.Penztar1 && s.Penznem == entity.Penznem))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Penztar1));
        }

        public static async Task<int> AddAsync(ossContext context, Models.Penztar entity)
        {
            Register.Creation(context, entity);
            await context.Penztar.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Penztarkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockpenztar", "penztarkod", pKey, utoljaraModositva);
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Penztartetel.CountAsync(s => s.Penztarkod == pKey);
            if (n > 0)
                result.Add("PENZTARTETEL.PENZTARKOD", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static async Task DeleteAsync(ossContext context, Models.Penztar entity)
        {
            context.Penztar.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task ExistsAnotherAsync(ossContext context, Models.Penztar entity)
        {
            if (await context.Penztar
                .AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod && 
                s.Penztar1 == entity.Penztar1 && s.Penztarkod != entity.Penztarkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Penztar1));
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Penztar entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Penztarkod;
        }

        public static async Task<Models.Penztar> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Penztar
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Penztarkod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Penztar.Penztarkod)}={pKey}"));

            return result.First();
        }

        //egyből dto-t állít elő
        public static async Task<List<PenztarDto>> ReadAsync(IOrderedQueryable<Models.Penztar> qry)
        {
            return await qry.Select(s => new PenztarDto
            {
                Penztarkod = s.Penztarkod,
                Penztar1 = s.Penztar1,
                Penznemkod = s.Penznemkod,
                Penznem = s.Penznem,
                Nyitva = s.Nyitva,
                Egyenleg = s.Penztartetel.Count == 0
                ? 0
                : (s.Penztartetel.Sum(t => t.Bevetel) - s.Penztartetel.Sum(t => t.Kiadas)),
                Letrehozva = s.Letrehozva,
                Letrehozta = s.Letrehozta,
                Modositva = s.Modositva,
                Modositotta = s.Modositotta
            }).ToListAsync();
        }
        public static async Task<List<PenztarDto>> ReadAsync(ossContext context, string maszk)
        {
            var qry = context.Penztar
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod && 
                    s.Penztar1.Contains(maszk))
                .OrderBy(s => s.Penztar1);

            return await ReadAsync(qry);
        }
        public static async Task<List<PenztarDto>> ReadAsync(ossContext context, int penztarkod)
        {
            var qry = context.Penztar
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod && 
                    s.Penztarkod == penztarkod)
                .OrderBy(s => s.Penztar1);

            return await ReadAsync(qry);
        }
        public static async Task<List<PenztarDto>> ReadByCurrencyOpenedAsync(ossContext context, int penznemkod)
        {
            var qry = context.Penztar
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod && 
                    s.Penznemkod == penznemkod && s.Nyitva)
                .OrderBy(s => s.Penztar1);

            return await ReadAsync(qry);
        }
    }
}
