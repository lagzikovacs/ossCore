using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Csoport
{
    public class CsoportDal
    {
        public static async Task ExistsAsync(ossContext context, Models.Csoport entity)
        {
            if (await context.Csoport.AnyAsync(s => s.Particiokod == entity.Particiokod && s.Csoport1 == entity.Csoport1))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Csoport1));
        }

        public static async Task<int> AddAsync(ossContext context, Models.Csoport entity)
        {
            Register.Creation(context, entity);
            await context.Csoport.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Csoportkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockcsoport", "csoportkod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Csoport> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Csoport
              .Where(s => s.Csoportkod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Csoport.Csoportkod)}={pKey}"));

            return result.First();
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Csoportfelhasznalo.CountAsync(s => s.Csoportkod == pKey);
            if (n > 0)
                result.Add("CSOPORTFELHASZNALO.CSOPORTKOD", n);

            n = await context.Csoportjog.CountAsync(s => s.Csoportkod == pKey);
            if (n > 0)
                result.Add("CSOPORTJOG.CSOPORTKOD", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static async Task DeleteAsync(ossContext context, Models.Csoport entity)
        {
            context.Csoport.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task ExistsAnotherAsync(ossContext context, Models.Csoport entity)
        {
            if (await context.Csoport.AnyAsync(s =>
              s.Particiokod == entity.Particiokod && s.Csoport1 == entity.Csoport1 && s.Csoportkod != entity.Csoportkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Csoport1));
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Csoport entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Csoportkod;
        }

        public static async Task<List<Models.Csoport>> ReadAsync(ossContext context, string maszk)
        {
            return await context.Csoport.Include(r => r.ParticiokodNavigation).AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod && s.Csoport1.Contains(maszk))
              .OrderBy(s => s.Csoport1).ToListAsync();
        }


        public static async Task<List<Models.Csoport>> GetSzerepkorokAsync(ossContext context)
        {
            var qryCsf = context.Csoportfelhasznalo
              .Where(s => s.Felhasznalokod == context.CurrentSession.Felhasznalokod)
              .Select(s => s.Csoportkod);

            return await context.Csoport.Include(r => r.ParticiokodNavigation)
              .Where(s => qryCsf.Contains(s.Csoportkod))
              .OrderBy(s => s.ParticiokodNavigation.Megnevezes)
              .ThenBy(s => s.Csoport1)
              .ToListAsync();
        }

        public static async Task CheckSzerepkorAsync(ossContext context, int particioKod, int csoportKod)
        {
            var qryCsf = context.Csoportfelhasznalo
              .Where(s => s.Particiokod == particioKod)
              .Where(s => s.Csoportkod == csoportKod)
              .Where(s => s.Felhasznalokod == context.CurrentSession.Felhasznalokod);

            if ((await qryCsf.ToListAsync()).Count != 1)
                throw new Exception("A kiválasztott szerepkör nem létezik!");
        }

        public static async Task<int> JogeAsync(ossContext context, string jogKod)
        {
            return await context.Csoportjog
              .Include(r => r.LehetsegesjogkodNavigation)
              .Where(s => s.Csoportkod == context.CurrentSession.Csoportkod)
              .CountAsync(s => s.LehetsegesjogkodNavigation.Jogkod == jogKod);
        }

        public static async Task JogeAsync(ossContext context, JogKod jogKod)
        {
            // TODO upgrade után kivenni
            if (jogKod == JogKod.BIZONYLAT)
                return;

            if (await JogeAsync(context, jogKod.ToString()) == 0)
                throw new Exception("Hm... acces denied!");
        }

        public static async Task<List<string>> JogaimAsync(ossContext context)
        {
            return await context.Csoportjog
                .Include(r => r.LehetsegesjogkodNavigation)
                .Where(s => s.Csoportkod == context.CurrentSession.Csoportkod)
                .Select(s => s.LehetsegesjogkodNavigation.Jogkod)
                .ToListAsync();
        }

        public static async Task<List<int>> SelectCsoportFelhasznaloAsync(ossContext context, int csoportKod)
        {
            return await context.Csoportfelhasznalo
              .Where(s => s.Csoportkod == csoportKod)
              .Select(s => s.Felhasznalokod)
              .ToListAsync();
        }

        public static async Task<List<int>> SelectCsoportJogAsync(ossContext context, int csoportKod)
        {
            return await context.Csoportjog
              .Where(s => s.Csoportkod == csoportKod)
              .Select(s => s.Lehetsegesjogkod)
              .ToListAsync();
        }

        public static async Task CsoportFelhasznaloBeAsync(ossContext context, int particioKod, int csoportKod, int felhasznaloKod)
        {
            var entity = new Csoportfelhasznalo
            {
                Particiokod = particioKod,
                Csoportkod = csoportKod,
                Felhasznalokod = felhasznaloKod
            };

            Register.Creation(context, entity);

            await context.Csoportfelhasznalo.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public static async Task CsoportFelhasznaloBeAsync(ossContext context, int csoportKod, int felhasznaloKod)
        {
            var entity = new Csoportfelhasznalo
            {
                Csoportkod = csoportKod,
                Felhasznalokod = felhasznaloKod
            };

            Register.Creation(context, entity);

            await context.Csoportfelhasznalo.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public static async Task CsoportFelhasznaloKiAsync(ossContext context, int csoportKod, int felhasznaloKod)
        {
            var lst = await context.Csoportfelhasznalo
              .Where(s => s.Csoportkod == csoportKod && s.Felhasznalokod == felhasznaloKod)
              .ToListAsync();

            if (lst.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Csoportfelhasznalo.Csoportkod)}={csoportKod}, {nameof(Csoportfelhasznalo.Felhasznalokod)}={felhasznaloKod}"));

            context.Csoportfelhasznalo.Remove(lst[0]);
            await context.SaveChangesAsync();
        }

        public static async Task CsoportJogBeAsync(ossContext context, int csoportKod, int lehetsegesJogKod)
        {
            var entity = new Csoportjog
            {
                Csoportkod = csoportKod,
                Lehetsegesjogkod = lehetsegesJogKod
            };

            Register.Creation(context, entity);

            await context.Csoportjog.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public static async Task CsoportJogKiAsync(ossContext context, int csoportKod, int lehetsegesJogKod)
        {
            var lst = await context.Csoportjog
              .Where(s => s.Csoportkod == csoportKod && s.Lehetsegesjogkod == lehetsegesJogKod)
              .ToListAsync();

            if (lst.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                  $"{nameof(Csoportjog.Csoportkod)}={csoportKod}, {nameof(Csoportjog.Lehetsegesjogkod)}={lehetsegesJogKod}"));

            context.Csoportjog.Remove(lst[0]);
            await context.SaveChangesAsync();
        }
    }
}
