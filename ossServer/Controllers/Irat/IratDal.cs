using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Irat
{
    public class IratDal
    {
        internal static IOrderedQueryable<IRAT> GetQuery(OSSContext model, List<SzMT> szmt)
        {
            var qry = model.IRAT.AsNoTracking()
              .Include("IRATTIPUS").Include("UGYFEL")
              .Where(s => s.PARTICIOKOD == model.Session.PARTICIOKOD);

            foreach (var f in szmt)
            {
                switch (f.Szempont)
                {
                    case Szempont.Kod:
                        qry = int.TryParse((string)f.Minta, out var iratKod)
                          ? qry.Where(s => s.IRATKOD <= iratKod)
                          : qry.Where(s => s.IRATKOD >= 0);
                        break;
                    case Szempont.Keletkezett:
                        qry = DateTime.TryParse((string)f.Minta, out var keletkezett)
                          ? qry.Where(s => s.KELETKEZETT >= keletkezett)
                          : qry.Where(s => s.KELETKEZETT >= DateTime.MinValue);
                        break;
                    case Szempont.Ugyfel:
                        qry = qry.Where(s => s.UGYFEL.NEV.Contains((string)f.Minta));
                        break;
                    case Szempont.Targy:
                        qry = qry.Where(s => s.TARGY.Contains((string)f.Minta));
                        break;
                    case Szempont.Irattipus:
                        qry = qry.Where(s => s.IRATTIPUS.IRATTIPUS1.Contains((string)f.Minta));
                        break;
                    case Szempont.Kuldo:
                        qry = qry.Where(s => s.KULDO.Contains((string)f.Minta));
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
                          ? qry.OrderByDescending(s => s.IRATKOD)
                          : ((IOrderedQueryable<IRAT>)qry).ThenByDescending(s => s.IRATKOD);
                        break;
                    case Szempont.Keletkezett:
                        qry = elsoSorrend
                          ? qry.OrderByDescending(s => s.KELETKEZETT)
                          : ((IOrderedQueryable<IRAT>)qry).ThenByDescending(s => s.KELETKEZETT);
                        break;
                    case Szempont.Ugyfel:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.UGYFEL.NEV)
                          : ((IOrderedQueryable<IRAT>)qry).ThenBy(s => s.UGYFEL.NEV);
                        break;
                    case Szempont.Targy:
                        qry = elsoSorrend ? qry.OrderBy(s => s.TARGY) : ((IOrderedQueryable<IRAT>)qry).ThenBy(s => s.TARGY);
                        break;
                    case Szempont.Irattipus:
                        qry = elsoSorrend
                          ? qry.OrderBy(s => s.IRATTIPUS)
                          : ((IOrderedQueryable<IRAT>)qry).ThenBy(s => s.IRATTIPUS);
                        break;
                    case Szempont.Kuldo:
                        qry = elsoSorrend ? qry.OrderBy(s => s.KULDO) : ((IOrderedQueryable<IRAT>)qry).ThenBy(s => s.KULDO);
                        break;
                    default:
                        throw new Exception($"Lekezeletlen {f.Szempont} Szempont!");
                }
                elsoSorrend = false;
            }

            return (IOrderedQueryable<IRAT>)qry;
        }

        internal static int Add(OSSContext model, IRAT entity)
        {
            Register.Creation(model, entity);
            model.IRAT.Add(entity);
            model.SaveChanges();

            return entity.IRATKOD;
        }

        internal static void Lock(OSSContext model, int pKey, DateTime utoljaraModositva)
        {
            if (!model.LockIRAT(pKey, utoljaraModositva))
                throw new Exception(Messages.AdatMegvaltozottNemLehetModositani);
        }

        internal static IRAT Get(OSSContext model, int pKey)
        {
            var result = model.IRAT
              .Include("IRATTIPUS").Include("UGYFEL")
              .Where(s => s.PARTICIOKOD == model.Session.PARTICIOKOD)
              .Where(s => s.IRATKOD == pKey)
              .ToList();
            if (result.Count != 1)
                throw new Exception(string.Format(Messages.AdatNemTalalhato, $"{nameof(IRAT.IRATKOD)}={pKey}"));
            return result.First();
        }

        internal static void CheckReferences(OSSContext model, int pKey)
        {
            var result = new Dictionary<string, int>();

            var n = model.BIZONYLATKAPCSOLAT.Count(s => s.IRATKOD == pKey);
            if (n > 0)
                result.Add("BIZONYLATKAPCSOLAT.IRATKOD", n);

            n = model.PROJEKTKAPCSOLAT.Count(s => s.IRATKOD == pKey);
            if (n > 0)
                result.Add("PROJEKTKAPCSOLAT.IRATKOD", n);

            n = model.DOKUMENTUM.Count(s => s.IRATKOD == pKey);
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

        internal static void Delete(OSSContext model, IRAT entity)
        {
            model.IRAT.Remove(entity);
            model.SaveChanges();
        }

        internal static int Update(OSSContext model, IRAT entity)
        {
            Register.Modification(model, entity);
            model.SaveChanges();

            return entity.IRATKOD;
        }

        internal static void FotozasCheck(OSSContext model, int Particiokod, int Iratkod, string Kikuldesikod)
        {
            var list = model.IRAT.Where(s => s.PARTICIOKOD == Particiokod &&
                                          s.IRATKOD == Iratkod &&
                                          s.KIKULDESIKOD == Kikuldesikod)
                                          .ToList();
            if (list.Count != 1)
                throw new Exception("Nem fotózhat - hibás paraméterek!");
        }
    }
}
