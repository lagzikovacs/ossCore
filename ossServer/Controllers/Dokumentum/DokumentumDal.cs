using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Dokumentum
{
    public class DokumentumDal
    {
        public static async Task<int> AddAsync(ossContext context, Models.Dokumentum entity)
        {
            Register.Creation(context, entity);
            await context.Dokumentum.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Dokumentumkod;
        }

        public static async Task<Models.Dokumentum> GetAsync(ossContext context, int key)
        {
            var result = await context.Dokumentum
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Dokumentumkod == key)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, 
                    $"{nameof(Models.Dokumentum.Dokumentumkod)}={key}"));

            return result.First();
        }

        public static async Task<Models.Dokumentum> GetWithVolumeAsync(ossContext context, int key)
        {
            var result = await context.Dokumentum.Include(r => r.VolumekodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Dokumentumkod == key)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, 
                    $"{nameof(Models.Dokumentum.Dokumentumkod)}={key}"));

            return result.First();
        }

        public static async Task<List<int>> DokumentumkodByVolumeAsync(ossContext context, int volumeKod)
        {
            return await context.Dokumentum.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod && s.Volumekod == volumeKod)
              .OrderBy(s => s.Dokumentumkod)
              .Select(s => s.Dokumentumkod)
              .ToListAsync();
        }

        public static async Task<List<Models.Dokumentum>> SelectAsync(ossContext context, int iratKod)
        {
            return await context.Dokumentum
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Iratkod == iratKod)
              .OrderByDescending(s => s.Dokumentumkod)
              .ToListAsync();
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockdokumentum", "dokumentumkod", pKey, utoljaraModositva);
        }

        public static async Task DeleteAsync(ossContext context, Models.Dokumentum entity)
        {
            context.Dokumentum.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
