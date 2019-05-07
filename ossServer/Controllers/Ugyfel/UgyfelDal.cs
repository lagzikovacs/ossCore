using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ugyfel
{
    public class UgyfelDal
    {
        public static IOrderedQueryable<Models.Ugyfel> GetQuery(ossContext context, List<SzMT> szmt)
        {
            var qry = context.Ugyfel.AsNoTracking()
              .Include(r => r.HelysegkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod);

            foreach (var f in szmt)
            {
                switch (f.Szempont)
                {
                    case Szempont.Hirlevel:
                        qry = qry.Where(s => s.Hirlevel);
                        break;
                    case Szempont.Nev:
                        qry = qry.Where(s => s.Nev.Contains((string)f.Minta));
                        break;
                    case Szempont.UgyfelEmail:
                        qry = qry.Where(s => s.Email.Contains((string)f.Minta));
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
                    case Szempont.Hirlevel:
                        break;
                    case Szempont.Nev:
                        qry = elsoSorrend ? qry.OrderBy(s => s.Nev) : ((IOrderedQueryable<Models.Ugyfel>)qry).ThenBy(s => s.Nev);
                        break;
                    case Szempont.UgyfelEmail:
                        qry = elsoSorrend ? qry.OrderBy(s => s.Email) : ((IOrderedQueryable<Models.Ugyfel>)qry).ThenBy(s => s.Email);
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }
                elsoSorrend = false;
            }

            return (IOrderedQueryable<Models.Ugyfel>)qry;
        }

        public static void Exists(ossContext context, Models.Ugyfel entity)
        {
            if (context.Ugyfel.Any(s => s.Particiokod == entity.Particiokod &&
                s.Nev == entity.Nev && s.Helysegkod == entity.Helysegkod && s.Kozterulet == entity.Kozterulet && 
                s.Kozterulettipus == entity.Kozterulettipus && s.Hazszam == entity.Hazszam))
                throw new Exception(string.Format(Messages.MarLetezoTetel, entity.Nev));
        }

        public static int Add(ossContext context, Models.Ugyfel entity)
        {
            Register.Creation(context, entity);
            context.Ugyfel.Add(entity);
            context.SaveChanges();

            return entity.Ugyfelkod;
        }

        public async static void Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockugyfel", "ugyfelkod", pKey, utoljaraModositva);
        }

        public static Models.Ugyfel Get(ossContext context, int pKey)
        {
            var result = context.Ugyfel
              .Include("HELYSEG")
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Ugyfelkod == pKey).ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, 
                    $"{nameof(Models.Ugyfel.Ugyfelkod)}={pKey}"));
            return result.First();
        }

        public static void CheckReferences(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = context.Irat.Count(s => s.Ugyfelkod == pKey);
            if (n > 0)
                result.Add("IRAT.UGYFELKOD", n);

            n = context.Projekt.Count(s => s.Ugyfelkod == pKey);
            if (n > 0)
                result.Add("PROJEKT.UGYFELKOD", n);

            n = context.Bizonylat.Count(s => s.Ugyfelkod == pKey);
            if (n > 0)
                result.Add("BIZONYLAT.UGYFELKOD", n);

            n = context.Penztartetel.Count(s => s.Ugyfelkod == pKey);
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

        public static void Delete(ossContext context, Models.Ugyfel entity)
        {
            context.Ugyfel.Remove(entity);
            context.SaveChanges();
        }

        public static void ExistsAnother(ossContext context, Models.Ugyfel entity)
        {
            if (context.Ugyfel.Any(s => s.Particiokod == entity.Particiokod &&
                s.Nev == entity.Nev && s.Helysegkod == entity.Helysegkod &&
                s.Kozterulet == entity.Kozterulet && s.Kozterulettipus == entity.Kozterulettipus &&
                s.Hazszam == entity.Hazszam && s.Ugyfelkod != entity.Ugyfelkod))
                throw new Exception(string.Format(Messages.NemMenthetoMarLetezik, entity.Nev));
        }

        public static int Update(ossContext context, Models.Ugyfel entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Ugyfelkod;
        }

        public static List<Models.Ugyfel> Read(ossContext context, string maszk)
        {
            return context.Ugyfel.AsNoTracking()
              .Include("HELYSEG")
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod && s.Nev.Contains(maszk))
              .OrderBy(s => s.Nev)
              .ToList();
        }

        public static void ZoomCheck(ossContext context, int ugyfelkod, string ugyfel)
        {
            if (!context.Ugyfel.Any(s => s.Particiokod == context.CurrentSession.Particiokod &&
                s.Ugyfelkod == ugyfelkod && s.Nev == ugyfel))
                throw new Exception(string.Format(Messages.HibasZoom, "ügyfél"));
        }

        public static void UgyfelterCheck(ossContext context, int particiokod, int ugyfelkod, string kikuldesikod)
        {
            var list = context.Ugyfel.Where(s => s.Particiokod == particiokod &&
                s.Ugyfelkod == ugyfelkod && s.Kikuldesikod == kikuldesikod).ToList();
            if (list.Count != 1)
                throw new Exception("Nem juthat be az ügyféltérbe - hibás paraméterek!");
        }
    }
}
