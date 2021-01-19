using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatDal
    {
        public static async Task<int> AddAsync(ossContext context, Models.Bizonylat entity)
        {
            Register.Creation(context, entity);
            await context.Bizonylat.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Bizonylatkod;
        }

        public static async Task<Models.Bizonylat> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Bizonylat
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
                .Where(s => s.Bizonylatkod == pKey).ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Bizonylat.Bizonylatkod)}={pKey}"));

            return result.First();
        }

        public static async Task<Models.Bizonylat> GetComplexAsync(ossContext context, int pKey)
        {
            var result = await context.Bizonylat
              .Include(r => r.Bizonylattetel)
              .Include(r => r.Bizonylatafa)
              .Include(r => r.Bizonylattermekdij)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Bizonylatkod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Bizonylat.Bizonylatkod)}={pKey}"));

            return result.First();
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockbizonylat", "bizonylatkod", pKey, utoljaraModositva);
        }

        public static async Task DeleteAsync(ossContext context, Models.Bizonylat entity)
        {
            context.Bizonylat.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Bizonylat entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Bizonylatkod;
        }

        public static IOrderedQueryable<Models.Bizonylat> GetQuery(ossContext context, 
            BizonylatTipus bizonylatTipus, List<SzMT> szmt)
        {
            var bizonylattipusKod = bizonylatTipus.GetHashCode();
            var qry = context.Bizonylat.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Bizonylattipuskod == bizonylattipusKod);

            foreach (var f in szmt)
                switch (f.Szempont)
                {
                    case Szempont.Kod:
                        qry = int.TryParse((string)f.Minta, out var bizonylatKod)
                          ? qry.Where(s => s.Bizonylatkod <= bizonylatKod)
                          : qry.Where(s => s.Bizonylatkod >= 0);
                        break;
                    case Szempont.Bizonylatszam:
                        qry = qry.Where(s => s.Bizonylatszam.Contains((string)f.Minta));
                        break;
                    case Szempont.Ugyfel:
                        qry = qry.Where(s => s.Ugyfelnev.Contains((string)f.Minta));
                        break;
                    case Szempont.Minta:
                        //qry = qry.Where(s => s.MINTA.Contains((string)F.Minta));
                        break;
                    case Szempont.NincsKiszallitva:
                        qry = qry.Where(s => s.Kiszallitva == false);
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }

            var elsoSorrend = true;
            foreach (var f in szmt)
                switch (f.Szempont)
                {
                    case Szempont.Kod:
                        qry = elsoSorrend
                          ? qry.OrderByDescending(s => s.Bizonylatkod)
                          : ((IOrderedQueryable<Models.Bizonylat>)qry).ThenByDescending(s => s.Bizonylatkod);
                        elsoSorrend = false;
                        break;
                    case Szempont.Bizonylatszam:
                        qry = elsoSorrend
                          ? qry.OrderByDescending(s => s.Bizonylatszam)
                          : ((IOrderedQueryable<Models.Bizonylat>)qry).ThenByDescending(s => s.Bizonylatszam);
                        elsoSorrend = false;
                        break;
                    case Szempont.Ugyfel:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.Ugyfelnev)
                          : ((IOrderedQueryable<Models.Bizonylat>)qry).ThenBy(s => s.Ugyfelnev);
                        elsoSorrend = false;
                        qry = ((IOrderedQueryable<Models.Bizonylat>)qry).ThenByDescending(s => s.Bizonylatkod);
                        break;
                    case Szempont.Minta:
                        //qry = elsoSorrend ? qry.OrderBy(s => s.MINTA) : ((IOrderedQueryable<BIZONYLAT>)qry).ThenBy(s => s.MINTA);
                        //elsoSorrend = false;
                        break;
                    case Szempont.NincsKiszallitva:
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }

            return (IOrderedQueryable<Models.Bizonylat>)qry;
        }

        public static async Task<List<Models.Bizonylat>> Select_SzamlaKelteAsync(ossContext context, DateTime tol, DateTime ig)
        {
            var bizonylattipusKod = BizonylatTipus.Szamla.GetHashCode();

            return await context.Bizonylat.AsNoTracking()
              .Include(r => r.Bizonylattetel)
              .Include(r => r.Bizonylatafa)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Bizonylattipuskod == bizonylattipusKod)
              .Where(s => s.Bizonylatkelte >= tol.Date)
              .Where(s => s.Bizonylatkelte <= ig.Date)
              .ToListAsync();
        }

        public static async Task<List<Models.Bizonylat>> Select_SzamlaSzamAsync(ossContext context, string tol, string ig)
        {
            var bizonylattipusKod = BizonylatTipus.Szamla.GetHashCode();

            return await context.Bizonylat.AsNoTracking()
              .Include(r => r.Bizonylattetel)
              .Include(r => r.Bizonylatafa)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Bizonylattipuskod == bizonylattipusKod)
              .Where(s => s.Bizonylatszam.CompareTo(tol) >= 0)
              .Where(s => s.Bizonylatszam.CompareTo(ig) <= 0)
              .ToListAsync();
        }

        public static async Task ZoomCheckAsync(ossContext context, int bizonylatkod, string bizonylatszam)
        {
            if (!await context.Bizonylat.AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Bizonylatkod == bizonylatkod && s.Bizonylatszam == bizonylatszam))
                throw new Exception(string.Format(Messages.HibasZoom, "bizonylat"));
        }
    }
}
