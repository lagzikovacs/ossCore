using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Cikk
{
    public class CikkDal
    {
        public static IOrderedQueryable<Models.Cikk> GetQuery(ossContext context, List<SzMT> szmt)
        {
            var qry = context.Cikk.AsNoTracking()
              .Include(r => r.AfakulcskodNavigation)
              .Include(r1 => r1.MekodNavigation)
              .Include(r2 => r2.TermekdijkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod);

            foreach (var f in szmt)
            {
                switch (f.Szempont)
                {
                    case Szempont.Megnevezes:
                        qry = qry.Where(s => s.Megnevezes.Contains((string)f.Minta));
                        break;
                    case Szempont.Kod:
                        qry = int.TryParse((string)f.Minta, out var cikkKod)
                          ? qry.Where(s => s.Cikkkod <= cikkKod)
                          : qry.Where(s => s.Cikkkod >= 0);
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }
            }

            var elsoSorrend = true;
            foreach (var f in szmt)
            {
                switch (f.Szempont)
                {
                    case Szempont.Megnevezes:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.Megnevezes)
                          : ((IOrderedQueryable<Models.Cikk>)qry).ThenBy(s => s.Megnevezes);
                        break;
                    case Szempont.Kod:
                        qry = elsoSorrend
                          ? (qry).OrderByDescending(s => s.Cikkkod)
                          : ((IOrderedQueryable<Models.Cikk>)qry).ThenByDescending(s => s.Cikkkod);
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }
                elsoSorrend = false;
            }

            return (IOrderedQueryable<Models.Cikk>)qry;
        }

        public static async Task ExistsAsync(ossContext context, Models.Cikk entity)
        {
            if (await context.Cikk.AnyAsync(s => s.Particiokod == entity.Particiokod && s.Megnevezes == entity.Megnevezes))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Megnevezes));
        }

        public static async Task<int> AddAsync(ossContext context, Models.Cikk entity)
        {
            Register.Creation(context, entity);
            await context.Cikk.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Cikkkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockcikk", "cikkkod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Cikk> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Cikk
              .Include(r => r.AfakulcskodNavigation)
              .Include(r1 => r1.MekodNavigation)
              .Include(r2 => r2.TermekdijkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Cikkkod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Cikk.Cikkkod)}={pKey}"));

            return result.First();
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Bizonylattetel.CountAsync(s => s.Cikkkod == pKey);
            if (n > 0)
                result.Add("BIZONYLATTETEL.CIKKKOD", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static async Task DeleteAsync(ossContext context, Models.Cikk entity)
        {
            context.Cikk.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task ExistsAnotherAsync(ossContext context, Models.Cikk entity)
        {
            if (await context.Cikk.AnyAsync(s =>
                s.Particiokod == entity.Particiokod && s.Megnevezes == entity.Megnevezes && 
                s.Cikkkod != entity.Cikkkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Megnevezes));
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Cikk entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Cikkkod;
        }

        public static async Task<List<Models.Cikk>> ReadAsync(ossContext context, string maszk)
        {
            return await context.Cikk.AsNoTracking()
              .Include(r => r.AfakulcskodNavigation)
              .Include(r1 => r1.MekodNavigation)
              .Include(r2 => r2.TermekdijkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod && s.Megnevezes.Contains(maszk))
              .OrderBy(s => s.Megnevezes)
              .ToListAsync();
        }

        public static async Task<List<CikkMozgasTetelDto>> MozgasAsync(ossContext context, int cikkKod, BizonylatTipus bizonylatTipus)
        {
            var qry = context.Bizonylattetel.AsNoTracking().Include(r => r.BizonylatkodNavigation)
              .Where(s => s.Cikkkod == cikkKod && s.BizonylatkodNavigation.Bizonylattipuskod == (int)bizonylatTipus)
              .OrderByDescending(s => s.BizonylatkodNavigation.Teljesiteskelte)
              .ThenByDescending(s => s.Bizonylattetelkod);

            return await qry.Select(s => new CikkMozgasTetelDto
            {
                Bizonylatszam = s.BizonylatkodNavigation.Bizonylatszam,
                Ugyfelnev = s.BizonylatkodNavigation.Ugyfelnev,
                Teljesiteskelte = s.BizonylatkodNavigation.Teljesiteskelte,
                Mennyiseg = s.Mennyiseg,
                Me = s.Me,
                Egysegar = s.Egysegar,
                Penznem = s.BizonylatkodNavigation.Penznem,
                Netto = s.Netto,
                Nettoft = s.Netto * s.BizonylatkodNavigation.Arfolyam
            }).ToListAsync();
        }

        public static async Task ZoomCheckAsync(ossContext context, int CikkKod, string Cikk)
        {
            if (!await context.Cikk.AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Cikkkod == CikkKod && s.Megnevezes == Cikk))
                throw new Exception(string.Format(Messages.HibasZoom, "Cikk"));
        }
    }
}
