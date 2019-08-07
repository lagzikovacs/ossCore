using Microsoft.EntityFrameworkCore;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ajanlatkeres
{
    public class AjanlatkeresDal
    {
        internal static IOrderedQueryable<Models.Ajanlatkeres> GetQuery(ossContext context, List<SzMT> szmt)
        {
            var qry = context.Ajanlatkeres.AsNoTracking()
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod);

            foreach (var f in szmt)
            {
                switch (f.Szempont)
                {
                    case Szempont.Kod:
                        qry = int.TryParse((string)f.Minta, out var ugynokkod)
                          ? qry.Where(s => s.Ajanlatkereskod <= ugynokkod)
                          : qry.Where(s => s.Ajanlatkereskod >= 0);
                        break;
                    case Szempont.Ugynoknev:
                        qry = qry.Where(s => s.Ugynoknev.Contains((string)f.Minta));
                        break;
                    case Szempont.Cim:
                        qry = qry.Where(s => s.Nev.Contains((string)f.Minta));
                        break;
                    case Szempont.Telepules:
                        qry = qry.Where(s => s.Cim.Contains((string)f.Minta));
                        break;
                    case Szempont.Email:
                        qry = qry.Where(s => s.Email.Contains((string)f.Minta));
                        break;
                    case Szempont.Telefonszam:
                        qry = qry.Where(s => s.Telefonszam.Contains((string)f.Minta));
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
                          ? qry.OrderByDescending(s => s.Ajanlatkereskod)
                          : ((IOrderedQueryable<Models.Ajanlatkeres>)qry).ThenByDescending(s => s.Ajanlatkereskod);
                        break;
                    case Szempont.Ugynoknev:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.Ugynoknev)
                          : ((IOrderedQueryable<Models.Ajanlatkeres>)qry).ThenBy(s => s.Ugynoknev);
                        break;
                    case Szempont.Nev:
                        qry = elsoSorrend ? qry.OrderBy(s => s.Nev) : ((IOrderedQueryable<Models.Ajanlatkeres>)qry).ThenBy(s => s.Nev);
                        break;
                    case Szempont.Cim:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.Cim)
                          : ((IOrderedQueryable<Models.Ajanlatkeres>)qry).ThenBy(s => s.Cim);
                        break;
                    case Szempont.Email:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.Email)
                          : ((IOrderedQueryable<Models.Ajanlatkeres>)qry).ThenBy(s => s.Email);
                        break;
                    case Szempont.Telefonszam:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.Telefonszam)
                          : ((IOrderedQueryable<Models.Ajanlatkeres>)qry).ThenBy(s => s.Telefonszam);
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }
                elsoSorrend = false;
            }

            return (IOrderedQueryable<Models.Ajanlatkeres>)qry;
        }
        internal static async Task<int> AddWebAsync(ossContext context, Models.Ajanlatkeres entity)
        {
            await context.Ajanlatkeres.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Ajanlatkereskod;
        }

        internal static async Task<int> AddAsync(ossContext context, Models.Ajanlatkeres entity)
        {
            Register.Creation(context, entity);
            await context.Ajanlatkeres.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Ajanlatkereskod;
        }

        public async static Task Lock(ossContext context, int pKey, DateTime utoljaraModositva)
        {
            await context.ExecuteLockFunction("lockajanlatkeres", "ajanlatkereskod", pKey, utoljaraModositva);
        }

        public static async Task<Models.Ajanlatkeres> GetAsync(ossContext context, int pKey)
        {
            var result = await context.Ajanlatkeres
              .Where(s => s.Particiokod == context.CurrentSession.Particiokod)
              .Where(s => s.Ajanlatkereskod == pKey)
              .ToListAsync();

            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(Models.Ajanlatkeres.Ajanlatkereskod)}={pKey}"));

            return result.First();
        }

        public static async Task DeleteAsync(ossContext context, Models.Ajanlatkeres entity)
        {
            context.Ajanlatkeres.Remove(entity);
            await context.SaveChangesAsync();
        }

        public static async Task<int> UpdateAsync(ossContext context, Models.Ajanlatkeres entity)
        {
            Register.Modification(context, entity);
            await context.SaveChangesAsync();

            return entity.Ajanlatkereskod;
        }
    }
}
