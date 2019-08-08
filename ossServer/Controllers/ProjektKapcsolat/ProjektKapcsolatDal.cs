using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.ProjektKapcsolat
{
    public class ProjektKapcsolatDal
    {
        public static async Task<List<Projektkapcsolat>> ReadByProjektKodAsync(ossContext context, int projektKod)
        {
            return await context.Projektkapcsolat
                .Include(r => r.IratkodNavigation).ThenInclude(r => r.IrattipuskodNavigation)
                .Include(r => r.BizonylatkodNavigation)
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
                .Where(s => s.Projektkod == projektKod)
                .OrderByDescending(s => s.Projektkapcsolatkod).ToListAsync();
        }

        public static async Task<List<Projektkapcsolat>> ReadByBizonylatKodAsync(ossContext context, int bizonylatKod)
        {
            return await context.Projektkapcsolat
                .Include(r => r.IratkodNavigation).ThenInclude(r => r.IrattipuskodNavigation)
                .Include(r => r.BizonylatkodNavigation)
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
                .Where(s => s.Bizonylatkod == bizonylatKod)
                .OrderByDescending(s => s.Projektkapcsolatkod).ToListAsync();
        }

        public static async Task<List<Projektkapcsolat>> ReadByIratKodAsync(ossContext context, int iratKod)
        {
            return await context.Projektkapcsolat
                .Include(r => r.IratkodNavigation).ThenInclude(r => r.IrattipuskodNavigation)
                .Include(r => r.BizonylatkodNavigation)
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
                .Where(s => s.Iratkod == iratKod)
                .OrderByDescending(s => s.Projektkapcsolatkod).ToListAsync();
        }

        public static async Task<int> AddAsync(ossContext context, Models.Projektkapcsolat entity)
        {
            Register.Creation(context, entity);
            await context.Projektkapcsolat.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Projektkapcsolatkod;
        }

        public static async Task<Projektkapcsolat> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Projektkapcsolat
                .Include(r => r.IratkodNavigation).ThenInclude(r => r.IrattipuskodNavigation)
                .Include(r => r.BizonylatkodNavigation)
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
                .Where(s => s.Projektkapcsolatkod == pKey)
                .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Projektkapcsolat.Projektkapcsolatkod)}={pKey}"));

            return result.First();
        }

        public static async Task DeleteAsync(ossContext context, Models.Projektkapcsolat entity)
        {
            context.Projektkapcsolat.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
