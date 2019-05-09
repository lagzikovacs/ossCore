using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ajanlatkeres
{
    public class AjanlatkeresDal
    {
        internal static IOrderedQueryable<AJANLATKERES> GetQuery(OSSContext model, List<SzMT> szmt)
        {
            var qry = model.AJANLATKERES.AsNoTracking()
              .Where(s => s.PARTICIOKOD == model.Session.PARTICIOKOD);

            foreach (var f in szmt)
            {
                switch (f.Szempont)
                {
                    case Szempont.Kod:
                        qry = int.TryParse((string)f.Minta, out var ugynokkod)
                          ? qry.Where(s => s.AJANLATKERESKOD <= ugynokkod)
                          : qry.Where(s => s.AJANLATKERESKOD >= 0);
                        break;
                    case Szempont.Ugynoknev:
                        qry = qry.Where(s => s.UGYNOKNEV.Contains((string)f.Minta));
                        break;
                    case Szempont.Cim:
                        qry = qry.Where(s => s.NEV.Contains((string)f.Minta));
                        break;
                    case Szempont.Telepules:
                        qry = qry.Where(s => s.CIM.Contains((string)f.Minta));
                        break;
                    case Szempont.Email:
                        qry = qry.Where(s => s.EMAIL.Contains((string)f.Minta));
                        break;
                    case Szempont.Telefonszam:
                        qry = qry.Where(s => s.TELEFONSZAM.Contains((string)f.Minta));
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
                          ? qry.OrderByDescending(s => s.AJANLATKERESKOD)
                          : ((IOrderedQueryable<AJANLATKERES>)qry).ThenByDescending(s => s.AJANLATKERESKOD);
                        break;
                    case Szempont.Ugynoknev:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.UGYNOKNEV)
                          : ((IOrderedQueryable<AJANLATKERES>)qry).ThenBy(s => s.UGYNOKNEV);
                        break;
                    case Szempont.Nev:
                        qry = elsoSorrend ? qry.OrderBy(s => s.NEV) : ((IOrderedQueryable<AJANLATKERES>)qry).ThenBy(s => s.NEV);
                        break;
                    case Szempont.Cim:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.CIM)
                          : ((IOrderedQueryable<AJANLATKERES>)qry).ThenBy(s => s.CIM);
                        break;
                    case Szempont.Email:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.EMAIL)
                          : ((IOrderedQueryable<AJANLATKERES>)qry).ThenBy(s => s.EMAIL);
                        break;
                    case Szempont.Telefonszam:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.TELEFONSZAM)
                          : ((IOrderedQueryable<AJANLATKERES>)qry).ThenBy(s => s.TELEFONSZAM);
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }
                elsoSorrend = false;
            }

            return (IOrderedQueryable<AJANLATKERES>)qry;
        }
        internal static int AddWeb(OSSContext model, AJANLATKERES entity)
        {
            model.AJANLATKERES.Add(entity);
            model.SaveChanges();

            return entity.AJANLATKERESKOD;
        }

        internal static int Add(OSSContext model, AJANLATKERES entity)
        {
            Register.Creation(model, entity);
            model.AJANLATKERES.Add(entity);
            model.SaveChanges();

            return entity.AJANLATKERESKOD;
        }

        public static void Lock(OSSContext model, int pKey, DateTime utoljaraModositva)
        {
            if (!model.LockAJANLATKERES(pKey, utoljaraModositva))
                throw new Exception(Messages.AdatMegvaltozottNemLehetModositani);
        }

        public static AJANLATKERES Get(OSSContext model, int pKey)
        {
            var result = model.AJANLATKERES
              .Where(s => s.PARTICIOKOD == model.Session.PARTICIOKOD)
              .Where(s => s.AJANLATKERESKOD == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(AJANLATKERES.AJANLATKERESKOD)}={pKey}"));
            return result.First();
        }

        public static void Delete(OSSContext model, AJANLATKERES entity)
        {
            model.AJANLATKERES.Remove(entity);
            model.SaveChanges();
        }

        public static int Update(OSSContext model, AJANLATKERES entity)
        {
            Register.Modification(model, entity);
            model.SaveChanges();

            return entity.AJANLATKERESKOD;
        }
    }
}
