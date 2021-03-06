﻿using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Irat
{
    public class IratDal
    {
        public static IOrderedQueryable<Models.Irat> GetQuery(ossContext context, List<SzMT> szmt)
        {
            var qry = context.Irat.AsNoTracking()
              .Include(r => r.IrattipuskodNavigation)
              .Include(r => r.UgyfelkodNavigation).ThenInclude(r => r.HelysegkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod);

            foreach (var f in szmt)
            {
                switch (f.Szempont)
                {
                    case Szempont.Kod:
                        qry = int.TryParse((string)f.Minta, out var iratKod)
                          ? qry.Where(s => s.Iratkod <= iratKod)
                          : qry.Where(s => s.Iratkod >= 0);
                        break;
                    case Szempont.Keletkezett:
                        qry = DateTime.TryParse((string)f.Minta, out var keletkezett)
                          ? qry.Where(s => s.Keletkezett >= keletkezett)
                          : qry.Where(s => s.Keletkezett >= DateTime.MinValue);
                        break;
                    case Szempont.Ugyfel:
                        qry = qry.Where(s => s.UgyfelkodNavigation.Nev.Contains((string)f.Minta));
                        break;
                    case Szempont.Targy:
                        qry = qry.Where(s => s.Targy.Contains((string)f.Minta));
                        break;
                    case Szempont.Irattipus:
                        qry = qry.Where(s => s.IrattipuskodNavigation.Irattipus1.Contains((string)f.Minta));
                        break;
                    case Szempont.Kuldo:
                        qry = qry.Where(s => s.Kuldo.Contains((string)f.Minta));
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
                          ? qry.OrderByDescending(s => s.Iratkod)
                          : ((IOrderedQueryable<Models.Irat>)qry).ThenByDescending(s => s.Iratkod);
                        break;
                    case Szempont.Keletkezett:
                        qry = elsoSorrend
                          ? qry.OrderByDescending(s => s.Keletkezett)
                          : ((IOrderedQueryable<Models.Irat>)qry).ThenByDescending(s => s.Keletkezett);
                        break;
                    case Szempont.Ugyfel:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.UgyfelkodNavigation.Nev)
                          : ((IOrderedQueryable<Models.Irat>)qry).ThenBy(s => s.UgyfelkodNavigation.Nev);
                        break;
                    case Szempont.Targy:
                        qry = elsoSorrend 
                          ? qry.OrderBy(s => s.Targy) 
                          : ((IOrderedQueryable<Models.Irat>)qry).ThenBy(s => s.Targy);
                        break;
                    case Szempont.Irattipus:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.IrattipuskodNavigation.Irattipus1)
                          : ((IOrderedQueryable<Models.Irat>)qry).ThenBy(s => s.IrattipuskodNavigation.Irattipus1);
                        break;
                    case Szempont.Kuldo:
                        qry = elsoSorrend 
                          ? qry.OrderBy(s => s.Kuldo) 
                          : ((IOrderedQueryable<Models.Irat>)qry).ThenBy(s => s.Kuldo);
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }
                elsoSorrend = false;
            }

            return (IOrderedQueryable<Models.Irat>)qry;
        }

        public static async Task<int> AddAsync(ossContext context, Models.Irat entity)
        {
            Register.Creation(context, entity);
            await context.Irat.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Iratkod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockirat", "iratkod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Irat> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Irat
              .Include(r => r.IrattipuskodNavigation)
              .Include(r1 => r1.UgyfelkodNavigation).ThenInclude(r => r.HelysegkodNavigation)
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Iratkod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Irat.Iratkod)}={pKey}"));

            return result.First();
        }

        public static async Task CheckReferencesAsync(ossContext context, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = await context.Bizonylatkapcsolat.CountAsync(s => s.Iratkod == pKey);
            if (n > 0)
                result.Add("BIZONYLATKAPCSOLAT.IRATKOD", n);

            n = await context.Projektkapcsolat.CountAsync(s => s.Iratkod == pKey);
            if (n > 0)
                result.Add("PROJEKTKAPCSOLAT.IRATKOD", n);

            n = await context.Dokumentum.CountAsync(s => s.Iratkod == pKey);
            if (n > 0)
                result.Add("DOKUMENTUM.IRATKOD", n);

            if (result.Count > 0)
            {
                string builder = "\r\n";
                foreach (var r in result)
                    builder += "\r\n" + r.Key + " (" + r.Value + ")";

                throw new Exception(Messages.NemTorolhetoReferenciakMiatt + builder);
            }
        }

        public static async Task DeleteAsync(ossContext context, Models.Irat entity)
        {
            context.Irat.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Irat entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Iratkod;
        }

        public static void FotozasCheck(ossContext context, int Particiokod, int Iratkod, 
            string Kikuldesikod)
        {
            var list = context.Irat.Where(s => s.Particiokod == Particiokod &&
                s.Iratkod == Iratkod && s.Kikuldesikod == Kikuldesikod).ToList();
            if (list.Count != 1)
                throw new Exception("Nem fotózhat - hibás paraméterek!");
        }
    }
}
