using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Onlineszamla
{
    public class OnlineszamlaDal
    {
        public static async System.Threading.Tasks.Task<int> AddAsync(ossContext context, int bizonylatkod)
        {
            var most = DateTime.Now;
            var entity = new Models.Navfeltoltes
            {
                Idopont = most,
                Bizonylatkod = bizonylatkod,
                Statusz = 0,
                Kovetkezoteendoidopont = most,
                Tokenkeresszamlalo = 0,
                Feltoltesszamlalo = 0,
                Feltoltesellenorzesszamlalo = 0,
                Emailszamlalo = 0
            };
            Register.Creation(context, entity);
            await context.Navfeltoltes.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Navfeltolteskod;
        }

        internal static IOrderedQueryable<Models.Navfeltoltes> GetQuery(ossContext context, List<SzMT> szmt)
        {
            var qry = context.Navfeltoltes.AsNoTracking()
                .Include(r => r.BizonylatkodNavigation)
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod);

            foreach (var f in szmt)
                switch (f.Szempont)
                {
                    case Szempont.Kod:
                        qry = int.TryParse((string)f.Minta, out var kod)
                          ? qry.Where(s => s.Navfeltolteskod <= kod)
                          : qry.Where(s => s.Navfeltolteskod >= 0);
                        break;
                    case Szempont.BizonylatKod:
                        qry = int.TryParse((string)f.Minta, out var bizonylatKod)
                          ? qry.Where(s => s.Bizonylatkod <= bizonylatKod)
                          : qry.Where(s => s.Bizonylatkod >= 0);
                        break;
                    case Szempont.Bizonylatszam:
                        qry = qry.Where(s => s.BizonylatkodNavigation.Bizonylatszam.Contains((string)f.Minta));
                        break;
                    case Szempont.Ugyfel:
                        qry = qry.Where(s => s.BizonylatkodNavigation.Ugyfelnev.Contains((string)f.Minta));
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
                          ? qry.OrderByDescending(s => s.Navfeltolteskod)
                          : ((IOrderedQueryable<Models.Navfeltoltes>)qry).ThenByDescending(s => s.Navfeltolteskod);
                        elsoSorrend = false;
                        break;
                    case Szempont.BizonylatKod:
                        qry = elsoSorrend
                          ? qry.OrderByDescending(s => s.Bizonylatkod)
                          : ((IOrderedQueryable<Models.Navfeltoltes>)qry).ThenByDescending(s => s.Bizonylatkod);
                        elsoSorrend = false;
                        break;
                    case Szempont.Bizonylatszam:
                        qry = elsoSorrend
                          ? qry.OrderByDescending(s => s.BizonylatkodNavigation.Bizonylatszam)
                          : ((IOrderedQueryable<Models.Navfeltoltes>)qry).ThenByDescending(s => s.BizonylatkodNavigation.Bizonylatszam);
                        elsoSorrend = false;
                        break;
                    case Szempont.Ugyfel:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.BizonylatkodNavigation.Ugyfelnev)
                          : ((IOrderedQueryable<Models.Navfeltoltes>)qry).ThenBy(s => s.BizonylatkodNavigation.Ugyfelnev);
                        elsoSorrend = false;
                        qry = ((IOrderedQueryable<Models.Navfeltoltes>)qry).ThenByDescending(s => s.Bizonylatkod);
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }

            return (IOrderedQueryable<Models.Navfeltoltes>)qry;
        }
    }
}
