using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Projekt
{
    public class ProjektDal
    {
        public static IOrderedQueryable<Models.Projekt> GetQuery(ossContext context, int statusz, 
            List<SzMT> szmt)
        {
            var qry = context.Projekt.AsNoTracking()
              .Include(r => r.PenznemkodNavigation)
              .Include(r1 => r1.UgyfelkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod);

            if (statusz != 0)
                qry = qry.Where(s => s.Statusz == statusz);

            foreach (var f in szmt)
            {
                switch (f.Szempont)
                {
                    case Szempont.Kod:
                        qry = int.TryParse((string)f.Minta, out var projektKod)
                          ? qry.Where(s => s.Projektkod <= projektKod)
                          : qry.Where(s => s.Projektkod >= 0);
                        break;
                    case Szempont.Keletkezett:
                        qry = DateTime.TryParse((string)f.Minta, out var keletkezett)
                          ? qry.Where(s => s.Keletkezett >= keletkezett)
                          : qry.Where(s => s.Keletkezett >= DateTime.MinValue);
                        break;
                    case Szempont.MuszakiAllapot:
                        qry = qry.Where(s => s.Muszakiallapot.Contains((string)f.Minta));
                        break;
                    case Szempont.Ugyfel:
                        qry = qry.Where(s => s.UgyfelkodNavigation.Nev.Contains((string)f.Minta));
                        break;
                    case Szempont.UgyfelCim:
                        qry = qry.Where(s => s.UgyfelkodNavigation.HelysegkodNavigation.Helysegnev.Contains((string)f.Minta));
                        break;
                    case Szempont.UgyfelEmail:
                        qry = qry.Where(s => s.UgyfelkodNavigation.Email.Contains((string)f.Minta));
                        break;
                    case Szempont.UgyfelTelefonszam:
                        qry = qry.Where(s => s.UgyfelkodNavigation.Telefon.Contains((string)f.Minta));
                        break;
                    case Szempont.TelepitesiCim:
                        qry = qry.Where(s => s.Telepitesicim.Contains((string)f.Minta));
                        break;
                    case Szempont.UgyfelKod:
                        if (int.TryParse((string)f.Minta, out var ugyfelKod))
                            qry = qry.Where(s => s.Ugyfelkod == ugyfelKod);
                        else
                            throw new Exception("Érvénytelen ügyfélkód!");
                        break;

                    case Szempont.Null:
                        break;

                    case Szempont.Megrendelve:
                        qry = qry.Where(s => s.Megrendelve != null);
                        break;
                    case Szempont.Kivitelezve:
                        break;
                    case Szempont.Keszrejelentve:
                        break;

                    case Szempont.CsakHaTeendo:
                        qry = qry.Where(p => p.Projektteendo.Any(pt => (pt.Elvegezve == (DateTime?)null)));
                        break;
                    case Szempont.CsakHaTeendoSajat:
                        var hataridoIg = Convert.ToDateTime(f.Minta).Date;
                        qry = qry.Where(p => p.Projektteendo.Any(pt =>
                          (pt.Elvegezve == null & pt.Dedikalva == context.CurrentSession.Felhasznalo &
                           pt.Hatarido <= hataridoIg)));
                        break;
                    case Szempont.CsakHaLejartTeendo:
                        qry = qry.Where(p =>
                          p.Projektteendo.Any(pt => (pt.Elvegezve == null & DateTime.Today > pt.Hatarido)));
                        break;
                    case Szempont.SajatTeendo:
                        var ss = f.Minta.ToString().Split(';');
                        var sHataridoIg = Convert.ToDateTime(ss[0]).Date;
                        var tl = new List<string>();
                        if (ss.Length > 1)
                            for (var i = 1; i < ss.Length; i++)
                                tl.Add(ss[i]);
                        qry = qry.Where(p => p.Projektteendo.Any(pt =>
                          (pt.Elvegezve == null & pt.Dedikalva == context.CurrentSession.Felhasznalo &
                           pt.Hatarido <= sHataridoIg & tl.Contains(pt.TeendokodNavigation.Teendo1))));
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
                          ? qry.OrderByDescending(s => s.Projektkod)
                          : ((IOrderedQueryable<Models.Projekt>)qry).ThenByDescending(s => s.Projektkod);
                        elsoSorrend = false;
                        break;
                    case Szempont.Keletkezett:
                        qry = elsoSorrend
                          ? qry.OrderByDescending(s => s.Keletkezett)
                          : ((IOrderedQueryable<Models.Projekt>)qry).ThenByDescending(s => s.Keletkezett);
                        elsoSorrend = false;
                        break;
                    case Szempont.MuszakiAllapot:
                        qry = elsoSorrend
                          ? qry.OrderByDescending(s => s.Projektkod)
                          : ((IOrderedQueryable<Models.Projekt>)qry).ThenByDescending(s => s.Projektkod);
                        elsoSorrend = false;
                        break;
                    case Szempont.Ugyfel:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.UgyfelkodNavigation.Nev)
                          : ((IOrderedQueryable<Models.Projekt>)qry).ThenBy(s => s.UgyfelkodNavigation.Nev);
                        elsoSorrend = false;
                        break;
                    case Szempont.UgyfelCim:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.UgyfelkodNavigation.HelysegkodNavigation.Helysegnev)
                          : ((IOrderedQueryable<Models.Projekt>)qry).ThenBy(s => s.UgyfelkodNavigation.HelysegkodNavigation.Helysegnev);
                        elsoSorrend = false;
                        break;
                    case Szempont.UgyfelEmail:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.UgyfelkodNavigation.Email)
                          : ((IOrderedQueryable<Models.Projekt>)qry).ThenBy(s => s.UgyfelkodNavigation.Email);
                        elsoSorrend = false;
                        break;
                    case Szempont.UgyfelTelefonszam:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.UgyfelkodNavigation.Telefon)
                          : ((IOrderedQueryable<Models.Projekt>)qry).ThenBy(s => s.UgyfelkodNavigation.Telefon);
                        elsoSorrend = false;
                        break;
                    case Szempont.TelepitesiCim:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.Telepitesicim)
                          : ((IOrderedQueryable<Models.Projekt>)qry).ThenBy(s => s.Telepitesicim);
                        elsoSorrend = false;
                        break;
                    case Szempont.UgyfelKod:
                        qry = elsoSorrend
                          ? qry.OrderByDescending(s => s.Projektkod)
                          : ((IOrderedQueryable<Models.Projekt>)qry).ThenByDescending(s => s.Projektkod);
                        elsoSorrend = false;
                        break;

                    case Szempont.Null:
                    case Szempont.Fut:

                    case Szempont.CsakHaTeendo:
                    case Szempont.CsakHaTeendoSajat:
                    case Szempont.CsakHaLejartTeendo:
                    case Szempont.SajatTeendo:

                    case Szempont.Megrendelve:
                    case Szempont.Kivitelezve:
                    case Szempont.Keszrejelentve:
                        break;
                }
            }

            return (IOrderedQueryable<Models.Projekt>)qry;
        }

        public static int Add(ossContext context, Models.Projekt entity)
        {
            Register.Creation(context, entity);
            context.Projekt.Add(entity);
            context.SaveChanges();

            return entity.Projektkod;
        }

        public async static void Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockprojekt", "projektkod", pKey, utoljaraModositva);
        }

        public static Models.Projekt Get(ossContext context, int pKey)
        {
            var result = context.Projekt
              .Include(r => r.PenznemkodNavigation)
              .Include(r1 => r1.UgyfelkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Projektkod == pKey).ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Projekt.Projektkod)}={pKey}"));
            return result.First();
        }

        public static void CheckReferences(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = context.Projektkapcsolat.Count(s => s.Projektkod == pKey);
            if (n > 0)
                result.Add("PROJEKTKAPCSOLAT.PROJEKTKOD", n);

            n = context.Projektteendo.Count(s => s.Projektkod == pKey);
            if (n > 0)
                result.Add("PROJEKTTEENDO.PROJEKTKOD", n);

            n = context.Szamlazasirend.Count(s => s.Projektkod == pKey);
            if (n > 0)
                result.Add("SZAMLAZASIREND.PROJEKTKOD", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static void Delete(ossContext context, Models.Projekt entity)
        {
            context.Projekt.Remove(entity);
            context.SaveChanges();
        }

        public static int Update(ossContext context, Models.Projekt entity)
        {
            Register.Modification(context, entity);
            context.SaveChanges();

            return entity.Projektkod;
        }
    }
}
