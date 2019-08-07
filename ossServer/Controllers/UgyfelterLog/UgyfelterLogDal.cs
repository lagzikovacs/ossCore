using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.UgyfelterLog
{
    public class UgyfelterLogDal
    {
        internal static IOrderedQueryable<Ugyfelterlog> GetQuery(ossContext model, List<SzMT> szmt)
        {
            var qry = model.Ugyfelterlog.AsNoTracking()
              .Include(r => r.UgyfelkodNavigation).ThenInclude(r => r.HelysegkodNavigation)
              .Where(s => s.Particiokod == model.CurrentSession.Particiokod);

            foreach (var f in szmt)
            {
                switch (f.Szempont)
                {
                    case Szempont.Kod:
                        qry = int.TryParse((string)f.Minta, out var ugyfelterlogkod)
                          ? qry.Where(s => s.Ugyfelterlogkod <= ugyfelterlogkod)
                          : qry.Where(s => s.Ugyfelterlogkod >= 0);
                        break;
                    case Szempont.Nev:
                        qry = qry.Where(s => s.UgyfelkodNavigation.Nev.Contains((string)f.Minta));
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
                    case Szempont.Kod:
                        qry = elsoSorrend
                          ? qry.OrderByDescending(s => s.Ugyfelterlogkod)
                          : ((IOrderedQueryable<Ugyfelterlog>)qry).ThenByDescending(s => s.Ugyfelterlogkod);
                        break;
                    case Szempont.Nev:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.UgyfelkodNavigation.Nev)
                          : ((IOrderedQueryable<Ugyfelterlog>)qry).ThenBy(s => s.UgyfelkodNavigation.Nev);
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }
                elsoSorrend = false;
            }

            return (IOrderedQueryable<Ugyfelterlog>)qry;
        }

        public static async System.Threading.Tasks.Task<int> AddAsync(ossContext model, Models.Ugyfelterlog entity)
        {
            Register.Creation(model, entity);
            await model.Ugyfelterlog.AddAsync(entity);
            await model.SaveChangesAsync();

            return entity.Ugyfelterlogkod;
        }
    }
}
