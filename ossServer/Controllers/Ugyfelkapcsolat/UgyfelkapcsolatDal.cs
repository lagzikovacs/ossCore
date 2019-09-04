using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ugyfelkapcsolat
{
    public class UgyfelkapcsolatDal
    {
        public static async Task ExistsAsync(ossContext context, Models.Ugyfelkapcsolat entity)
        {
            if (await context.Ugyfelkapcsolat.AnyAsync(s => 
                (s.Fromugyfelkod == entity.Fromugyfelkod && s.Tougyfelkod == entity.Tougyfelkod) ||
                (s.Fromugyfelkod == entity.Tougyfelkod && s.Tougyfelkod == entity.Fromugyfelkod) ))

                throw new Exception(string.Format(Messages.MarLetezoTetel, "ügyfélkapcsolat"));
        }

        public static async Task<int> AddAsync(ossContext context, Models.Ugyfelkapcsolat entity)
        {
            Register.Creation(context, entity);
            await context.Ugyfelkapcsolat.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Ugyfelkapcsolatkod;
        }

        public static async Task DeleteAsync(ossContext context, Models.Ugyfelkapcsolat entity)
        {
            context.Ugyfelkapcsolat.Remove(entity);
            await context.SaveChangesAsync();
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockugyfelkapcsolat", "ugyfelkapcsolatkod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Ugyfelkapcsolat> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Ugyfelkapcsolat
              .Include(r => r.FromugyfelkodNavigation).ThenInclude(r1 => r1.HelysegkodNavigation)
              .Include(r => r.FromugyfelkodNavigation).ThenInclude(r1 => r1.TevekenysegkodNavigation)
              .Include(r => r.TougyfelkodNavigation).ThenInclude(r1 => r1.HelysegkodNavigation)
              .Include(r => r.TougyfelkodNavigation).ThenInclude(r1 => r1.TevekenysegkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Ugyfelkapcsolatkod == pKey).ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato,
                    $"{nameof(Models.Ugyfelkapcsolat.Ugyfelkapcsolatkod)}={pKey}"));

            return result.First();
        }

        public static async Task ExistsAnotherAsync(ossContext context, Models.Ugyfelkapcsolat entity)
        {
            if (await context.Ugyfelkapcsolat.AnyAsync(s => s.Particiokod == entity.Particiokod &&
                ((s.Fromugyfelkod == entity.Fromugyfelkod && s.Tougyfelkod == entity.Tougyfelkod) ||
                (s.Fromugyfelkod == entity.Tougyfelkod && s.Tougyfelkod == entity.Fromugyfelkod)) &&
                s.Ugyfelkapcsolatkod != entity.Ugyfelkapcsolatkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, "ügyfélkapcsolat"));
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Ugyfelkapcsolat entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Ugyfelkapcsolatkod;
        }

        public static IOrderedQueryable<Models.Ugyfelkapcsolat> GetQuery(ossContext context)
        {
            var qry = context.Ugyfelkapcsolat.AsNoTracking()
                .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
                .OrderBy(s => s.Ugyfelkapcsolatkod);

            return qry;
        }

        public static IOrderedQueryable<Models.Ugyfelkapcsolat> GetQuery(ossContext context, 
            int Ugyfelkod, List<SzMT> szmt, FromTo FromTo)
        {
            var qry = context.Ugyfelkapcsolat.AsNoTracking()
              .Include(r => r.FromugyfelkodNavigation).ThenInclude(r1 => r1.HelysegkodNavigation)
              .Include(r => r.FromugyfelkodNavigation).ThenInclude(r1 => r1.TevekenysegkodNavigation)
              .Include(r => r.TougyfelkodNavigation).ThenInclude(r1 => r1.HelysegkodNavigation)
              .Include(r => r.TougyfelkodNavigation).ThenInclude(r1 => r1.TevekenysegkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod);

            foreach (var f in szmt)
            {
                switch (f.Szempont)
                {
                    case Szempont.Nev:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = qry.Where(s => s.Fromugyfelkod == Ugyfelkod && 
                                    s.TougyfelkodNavigation.Nev.Contains((string)f.Minta));
                                break;
                            case FromTo.HozzaEr:
                                qry = qry.Where(s => s.FromugyfelkodNavigation.Nev.Contains((string)f.Minta) &&
                                    s.Tougyfelkod == Ugyfelkod);
                                break;
                        }
                        break;
                    case Szempont.Ceg:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = qry.Where(s => s.Fromugyfelkod == Ugyfelkod && 
                                    s.TougyfelkodNavigation.Ceg.Contains((string)f.Minta));
                                break;
                            case FromTo.HozzaEr:
                                qry = qry.Where(s => s.FromugyfelkodNavigation.Ceg.Contains((string)f.Minta) &&
                                    s.Tougyfelkod == Ugyfelkod);
                                break;
                        }
                        break;
                    case Szempont.Beosztas:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = qry.Where(s => s.Fromugyfelkod == Ugyfelkod && 
                                    s.TougyfelkodNavigation.Beosztas.Contains((string)f.Minta));
                                break;
                            case FromTo.HozzaEr:
                                qry = qry.Where(s => s.FromugyfelkodNavigation.Beosztas.Contains((string)f.Minta) &&
                                    s.Tougyfelkod == Ugyfelkod);
                                break;
                        }
                        break;
                    case Szempont.Telepules:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = qry.Where(s => s.Fromugyfelkod == Ugyfelkod && 
                                    s.TougyfelkodNavigation.HelysegkodNavigation.Helysegnev.Contains((string)f.Minta));
                                break;
                            case FromTo.HozzaEr:
                                qry = qry.Where(s => s.FromugyfelkodNavigation.HelysegkodNavigation.Helysegnev.Contains((string)f.Minta) &&
                                    s.Tougyfelkod == Ugyfelkod);
                                break;
                        }
                        break;
                    case Szempont.UgyfelTelefonszam:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = qry.Where(s => s.Fromugyfelkod == Ugyfelkod && 
                                    s.TougyfelkodNavigation.Telefon.Contains((string)f.Minta));
                                break;
                            case FromTo.HozzaEr:
                                qry = qry.Where(s => s.FromugyfelkodNavigation.Telefon.Contains((string)f.Minta) &&
                                    s.Tougyfelkod == Ugyfelkod);
                                break;
                        }
                        break;
                    case Szempont.UgyfelEmail:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = qry.Where(s => s.Fromugyfelkod == Ugyfelkod && 
                                    s.TougyfelkodNavigation.Email.Contains((string)f.Minta));
                                break;
                            case FromTo.HozzaEr:
                                qry = qry.Where(s => s.FromugyfelkodNavigation.Email.Contains((string)f.Minta) &&
                                    s.Tougyfelkod == Ugyfelkod);
                                break;
                        }
                        break;
                    case Szempont.Egyeblink:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = qry.Where(s => s.Fromugyfelkod == Ugyfelkod && 
                                    s.TougyfelkodNavigation.Egyeblink.Contains((string)f.Minta));
                                break;
                            case FromTo.HozzaEr:
                                qry = qry.Where(s => s.FromugyfelkodNavigation.Egyeblink.Contains((string)f.Minta) &&
                                    s.Tougyfelkod == Ugyfelkod);
                                break;
                        }
                        break;
                    case Szempont.Ajanlo:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = qry.Where(s => s.Fromugyfelkod == Ugyfelkod && 
                                    s.TougyfelkodNavigation.Ajanlotta.Contains((string)f.Minta));
                                break;
                            case FromTo.HozzaEr:
                                qry = qry.Where(s => s.FromugyfelkodNavigation.Ajanlotta.Contains((string)f.Minta) &&
                                    s.Tougyfelkod == Ugyfelkod);
                                break;
                        }
                        break;
                    case Szempont.Kod:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = int.TryParse((string)f.Minta, out var ugyfelKod)
                                    ? qry.Where(s => s.Fromugyfelkod == Ugyfelkod && s.Tougyfelkod <= ugyfelKod)
                                    : qry.Where(s => s.Fromugyfelkod == Ugyfelkod && s.Tougyfelkod >= 0);
                                break;
                            case FromTo.HozzaEr:
                                qry = int.TryParse((string)f.Minta, out var ugyfelKod1)
                                    ? qry.Where(s => s.Fromugyfelkod <= ugyfelKod1 && s.Tougyfelkod == Ugyfelkod)
                                    : qry.Where(s => s.Fromugyfelkod >= 0 && s.Tougyfelkod == Ugyfelkod);
                                break;
                        }
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
                    case Szempont.Nev:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = elsoSorrend 
                                    ? qry.OrderBy(s => s.TougyfelkodNavigation.Nev) 
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.TougyfelkodNavigation.Nev);
                                break;
                            case FromTo.HozzaEr:
                                qry = elsoSorrend
                                    ? qry.OrderBy(s => s.FromugyfelkodNavigation.Nev)
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.FromugyfelkodNavigation.Nev);
                                break;
                        }
                        break;
                    case Szempont.Ceg:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = elsoSorrend 
                                    ? qry.OrderBy(s => s.TougyfelkodNavigation.Ceg) 
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.TougyfelkodNavigation.Ceg);
                                break;
                            case FromTo.HozzaEr:
                                qry = elsoSorrend
                                    ? qry.OrderBy(s => s.FromugyfelkodNavigation.Ceg)
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.FromugyfelkodNavigation.Ceg);
                                break;
                        }
                        break;
                    case Szempont.Beosztas:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = elsoSorrend 
                                    ? qry.OrderBy(s => s.TougyfelkodNavigation.Beosztas) 
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.TougyfelkodNavigation.Beosztas);
                                break;
                            case FromTo.HozzaEr:
                                qry = elsoSorrend
                                    ? qry.OrderBy(s => s.FromugyfelkodNavigation.Beosztas)
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.FromugyfelkodNavigation.Beosztas);
                                break;
                        }
                        break;
                    case Szempont.Telepules:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = elsoSorrend 
                                    ? qry.OrderBy(s => s.TougyfelkodNavigation.HelysegkodNavigation.Helysegnev) 
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.TougyfelkodNavigation.HelysegkodNavigation.Helysegnev);
                                break;
                            case FromTo.HozzaEr:
                                qry = elsoSorrend
                                    ? qry.OrderBy(s => s.FromugyfelkodNavigation.HelysegkodNavigation.Helysegnev)
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.FromugyfelkodNavigation.HelysegkodNavigation.Helysegnev);
                                break;
                        }                        
                        break;
                    case Szempont.UgyfelTelefonszam:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = elsoSorrend 
                                    ? qry.OrderBy(s => s.TougyfelkodNavigation.Telefon) 
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.TougyfelkodNavigation.Telefon);
                                break;
                            case FromTo.HozzaEr:
                                qry = elsoSorrend
                                    ? qry.OrderBy(s => s.FromugyfelkodNavigation.Telefon)
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.FromugyfelkodNavigation.Telefon);
                                break;
                        }
                        break;
                    case Szempont.UgyfelEmail:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = elsoSorrend 
                                    ? qry.OrderBy(s => s.TougyfelkodNavigation.Email) 
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.TougyfelkodNavigation.Email);
                                break;
                            case FromTo.HozzaEr:
                                qry = elsoSorrend
                                    ? qry.OrderBy(s => s.FromugyfelkodNavigation.Email)
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.FromugyfelkodNavigation.Email);
                                break;
                        }                        
                        break;
                    case Szempont.Egyeblink:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = elsoSorrend 
                                    ? qry.OrderBy(s => s.TougyfelkodNavigation.Egyeblink) 
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.TougyfelkodNavigation.Egyeblink);
                                break;
                            case FromTo.HozzaEr:
                                qry = elsoSorrend
                                    ? qry.OrderBy(s => s.FromugyfelkodNavigation.Egyeblink)
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.FromugyfelkodNavigation.Egyeblink);
                                break;
                        }
                        break;
                    case Szempont.Ajanlo:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = elsoSorrend 
                                    ? qry.OrderBy(s => s.TougyfelkodNavigation.Ajanlotta) 
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.TougyfelkodNavigation.Ajanlotta);
                                break;
                            case FromTo.HozzaEr:
                                qry = elsoSorrend
                                    ? qry.OrderBy(s => s.FromugyfelkodNavigation.Ajanlotta)
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenBy(s => s.FromugyfelkodNavigation.Ajanlotta);
                                break;
                        }
                        break;
                    case Szempont.Kod:
                        switch (FromTo)
                        {
                            case FromTo.ToleIndul:
                                qry = elsoSorrend
                                    ? (qry).OrderByDescending(s => s.Tougyfelkod)
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenByDescending(s => s.Tougyfelkod);
                                break;
                            case FromTo.HozzaEr:
                                qry = elsoSorrend
                                    ? (qry).OrderByDescending(s => s.Fromugyfelkod)
                                    : ((IOrderedQueryable<Models.Ugyfelkapcsolat>)qry).ThenByDescending(s => s.Fromugyfelkod);
                                break;
                        }
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }
                elsoSorrend = false;
            }

            return (IOrderedQueryable<Models.Ugyfelkapcsolat>)qry;
        }
    }
}
