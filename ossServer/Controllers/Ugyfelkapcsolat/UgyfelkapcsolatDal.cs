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
            if (await context.Ugyfelkapcsolat.AnyAsync(s => s.Particiokod == entity.Particiokod &&
                ((s.Fromugyfelkod == entity.Fromugyfelkod && s.Tougyfelkod == entity.Tougyfelkod) ||
                (s.Fromugyfelkod == entity.Tougyfelkod && s.Tougyfelkod == entity.Fromugyfelkod)) ))

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



        public static IOrderedQueryable<Models.Ugyfel> GetQuery(ossContext context, 
            int csoport, List<SzMT> szmt)
        {
            var qry = context.Ugyfel.AsNoTracking()
              .Include(r => r.HelysegkodNavigation)
              .Include(r => r.TevekenysegkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod);

            if (csoport != 0)
                qry = qry.Where(s => s.Csoport == csoport);

            foreach (var f in szmt)
            {
                switch (f.Szempont)
                {
                    case Szempont.Nev:
                        qry = qry.Where(s => s.Nev.Contains((string)f.Minta));
                        break;
                    case Szempont.Ceg:
                        qry = qry.Where(s => s.Ceg.Contains((string)f.Minta));
                        break;
                    case Szempont.Beosztas:
                        qry = qry.Where(s => s.Beosztas.Contains((string)f.Minta));
                        break;
                    case Szempont.Telepules:
                        qry = qry.Where(s => s.HelysegkodNavigation.Helysegnev.Contains((string)f.Minta));
                        break;
                    case Szempont.UgyfelTelefonszam:
                        qry = qry.Where(s => s.Telefon.Contains((string)f.Minta));
                        break;
                    case Szempont.UgyfelEmail:
                        qry = qry.Where(s => s.Email.Contains((string)f.Minta));
                        break;
                    case Szempont.Egyeblink:
                        qry = qry.Where(s => s.Egyeblink.Contains((string)f.Minta));
                        break;
                    case Szempont.Ajanlo:
                        qry = qry.Where(s => s.Ajanlotta.Contains((string)f.Minta));
                        break;
                    case Szempont.Kod:
                        qry = int.TryParse((string)f.Minta, out var ugyfelKod)
                          ? qry.Where(s => s.Ugyfelkod <= ugyfelKod)
                          : qry.Where(s => s.Ugyfelkod >= 0);
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
                        qry = elsoSorrend ? qry.OrderBy(s => s.Nev) : ((IOrderedQueryable<Models.Ugyfel>)qry).ThenBy(s => s.Nev);
                        break;
                    case Szempont.Ceg:
                        qry = elsoSorrend ? qry.OrderBy(s => s.Ceg) : ((IOrderedQueryable<Models.Ugyfel>)qry).ThenBy(s => s.Ceg);
                        break;
                    case Szempont.Beosztas:
                        qry = elsoSorrend ? qry.OrderBy(s => s.Beosztas) : ((IOrderedQueryable<Models.Ugyfel>)qry).ThenBy(s => s.Beosztas);
                        break;
                    case Szempont.Telepules:
                        qry = elsoSorrend ? qry.OrderBy(s => s.HelysegkodNavigation.Helysegnev) : ((IOrderedQueryable<Models.Ugyfel>)qry).ThenBy(s => s.HelysegkodNavigation.Helysegnev);
                        break;
                    case Szempont.UgyfelTelefonszam:
                        qry = elsoSorrend ? qry.OrderBy(s => s.Telefon) : ((IOrderedQueryable<Models.Ugyfel>)qry).ThenBy(s => s.Telefon);
                        break;
                    case Szempont.UgyfelEmail:
                        qry = elsoSorrend ? qry.OrderBy(s => s.Email) : ((IOrderedQueryable<Models.Ugyfel>)qry).ThenBy(s => s.Email);
                        break;
                    case Szempont.Egyeblink:
                        qry = elsoSorrend ? qry.OrderBy(s => s.Egyeblink) : ((IOrderedQueryable<Models.Ugyfel>)qry).ThenBy(s => s.Egyeblink);
                        break;
                    case Szempont.Ajanlo:
                        qry = elsoSorrend ? qry.OrderBy(s => s.Ajanlotta) : ((IOrderedQueryable<Models.Ugyfel>)qry).ThenBy(s => s.Ajanlotta);
                        break;
                    case Szempont.Kod:
                        qry = elsoSorrend
                          ? (qry).OrderByDescending(s => s.Ugyfelkod)
                          : ((IOrderedQueryable<Models.Ugyfel>)qry).ThenByDescending(s => s.Ugyfelkod);
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }
                elsoSorrend = false;
            }

            return (IOrderedQueryable<Models.Ugyfel>)qry;
        }





        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockugyfel", "ugyfelkod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Ugyfel> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Ugyfel
              .Include(r => r.HelysegkodNavigation)
              .Include(r => r.TevekenysegkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Ugyfelkod == pKey).ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, 
                    $"{nameof(Models.Ugyfel.Ugyfelkod)}={pKey}"));

            return result.First();
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Irat.CountAsync(s => s.Ugyfelkod == pKey);
            if (n > 0)
                result.Add("IRAT.UGYFELKOD", n);

            n = await context.Projekt.CountAsync(s => s.Ugyfelkod == pKey);
            if (n > 0)
                result.Add("PROJEKT.UGYFELKOD", n);

            n = await context.Bizonylat.CountAsync(s => s.Ugyfelkod == pKey);
            if (n > 0)
                result.Add("BIZONYLAT.UGYFELKOD", n);

            n = await context.Penztartetel.CountAsync(s => s.Ugyfelkod == pKey);
            if (n > 0)
                result.Add("PENZTARTETEL.UGYFELKOD", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }



        public static async Task ExistsAnotherAsync(ossContext context, Models.Ugyfel entity)
        {
            if (await context.Ugyfel.AnyAsync(s => s.Particiokod == entity.Particiokod &&
                s.Nev == entity.Nev && s.Helysegkod == entity.Helysegkod &&
                s.Kozterulet == entity.Kozterulet && s.Kozterulettipus == entity.Kozterulettipus &&
                s.Hazszam == entity.Hazszam && s.Ugyfelkod != entity.Ugyfelkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Nev));
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Ugyfel entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Ugyfelkod;
        }

        public static async Task<List<Models.Ugyfel>> ReadAsync(ossContext context, string maszk)
        {
            return await context.Ugyfel.AsNoTracking()
              .Include(r => r.HelysegkodNavigation)
              .Include(r => r.TevekenysegkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod && s.Nev.Contains(maszk))
              .OrderBy(s => s.Nev)
              .ToListAsync();
        }

        public static async Task ZoomCheckAsync(ossContext context, int ugyfelkod, string ugyfel)
        {
            if (!await context.Ugyfel.AnyAsync(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Ugyfelkod == ugyfelkod && s.Nev == ugyfel))
                throw new Exception(string.Format(Messages.HibasZoom, "ügyfél"));
        }

        public static async Task UgyfelterCheckAsync(ossContext context, int particiokod, int ugyfelkod, string kikuldesikod)
        {
            var list = await context.Ugyfel.Where(s => s.Particiokod == particiokod &&
                s.Ugyfelkod == ugyfelkod && s.Kikuldesikod == kikuldesikod).ToListAsync();
            if (list.Count != 1)
                throw new Exception("Nem juthat be az ügyféltérbe - hibás paraméterek!");
        }
    }
}
