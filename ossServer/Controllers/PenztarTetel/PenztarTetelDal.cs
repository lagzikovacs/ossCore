using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.PenztarTetel
{
    public class PenztarTetelDal
    {
        public static async Task<int> AddAsync(ossContext context, Models.Penztartetel entity)
        {
            Register.Creation(context, entity);
            await context.Penztartetel.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Penztartetelkod;
        }

        public static async Task<Penztartetel> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Penztartetel
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Penztartetelkod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Penztartetel.Penztartetelkod)}={pKey}"));

            return result.First();
        }

        public static IOrderedQueryable<Models.Penztartetel> GetQuery(ossContext context, List<SzMT> szmt)
        {
            var qry = context.Penztartetel.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Select(s => s);

            foreach (var f in szmt)
            {
                switch (f.Szempont)
                {
                    case Szempont.SzuloKod:
                        var penztarKod = int.Parse((string)f.Minta);
                        qry = qry.Where(s => s.Penztarkod == penztarKod);
                        break;
                    case Szempont.Kod:
                        qry = int.TryParse((string)f.Minta, out var penztartetelKod)
                          ? qry.Where(s => s.Penztartetelkod <= penztartetelKod)
                          : qry.Where(s => s.Penztartetelkod >= 0);
                        break;
                    case Szempont.PenztarBizonylatszam:
                        qry = qry.Where(s => s.Penztarbizonylatszam.Contains((string)f.Minta));
                        break;
                    case Szempont.Ugyfel:
                        qry = qry.Where(s => s.Ugyfelnev.Contains((string)f.Minta));
                        break;
                    case Szempont.Bizonylatszam:
                        qry = qry.Where(s => s.Bizonylatszam.Contains((string)f.Minta));
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
                    case Szempont.SzuloKod:
                        break;
                    case Szempont.Kod:
                        qry = elsoSorrend
                          ? qry.OrderByDescending(s => s.Penztartetelkod)
                          : ((IOrderedQueryable<Models.Penztartetel>)qry).ThenByDescending(s => s.Penztartetelkod);
                        elsoSorrend = false;
                        break;
                    case Szempont.PenztarBizonylatszam:
                        qry = elsoSorrend
                          ? qry.OrderByDescending(s => s.Penztarbizonylatszam)
                          : ((IOrderedQueryable<Models.Penztartetel>)qry).ThenByDescending(s => s.Penztarbizonylatszam);
                        elsoSorrend = false;
                        break;
                    case Szempont.Ugyfel:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.Ugyfelnev)
                          : ((IOrderedQueryable<Models.Penztartetel>)qry).ThenBy(s => s.Ugyfelnev);
                        elsoSorrend = false;
                        qry = ((IOrderedQueryable<Models.Penztartetel>)qry).ThenByDescending(s => s.Datum);
                        break;
                    case Szempont.Bizonylatszam:
                        qry = elsoSorrend
                          ? qry.OrderByDescending(s => s.Bizonylatszam)
                          : ((IOrderedQueryable<Models.Penztartetel>)qry).ThenByDescending(s => s.Bizonylatszam);
                        elsoSorrend = false;
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }
            }

            return (IOrderedQueryable<Models.Penztartetel>)qry;
        }
    }
}
