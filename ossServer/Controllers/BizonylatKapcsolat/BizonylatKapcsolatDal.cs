using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.BizonylatKapcsolat
{
    public class BizonylatKapcsolatDal
    {
        internal static async Task<List<Bizonylatkapcsolat>> SelectAsync(ossContext context, int bizonylatKod)
        {
            return await context.Bizonylatkapcsolat
                .Include(r => r.IratkodNavigation).ThenInclude(r => r.IrattipuskodNavigation)
                .Where(s => s.Bizonylatkod == bizonylatKod)
                .OrderByDescending(s => s.Bizonylatkapcsolatkod).ToListAsync();
        }

        internal static async Task<int> AddAsync(ossContext context, Models.Bizonylatkapcsolat entity)
        {
            Register.Creation(context, entity);
            await context.Bizonylatkapcsolat.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Bizonylatkapcsolatkod;
        }

        internal static async Task<Bizonylatkapcsolat> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Bizonylatkapcsolat
                .Include(r => r.IratkodNavigation).ThenInclude(r => r.IrattipuskodNavigation)
                .Where(s => s.Bizonylatkapcsolatkod == pKey)
                .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Models.Bizonylatkapcsolat.Bizonylatkapcsolatkod)}={pKey}"));

            return result.First();
        }

        internal static async Task DeleteAsync(ossContext context, Models.Bizonylatkapcsolat entity)
        {
            context.Bizonylatkapcsolat.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
