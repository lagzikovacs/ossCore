using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using System.Linq;

namespace ossServer.Controllers.Esemenynaplo
{
    public class EsemenynaploDal
    {
        public static int Add(ossContext context, Models.Esemenynaplo entity)
        {
            context.Esemenynaplo.Add(entity);
            context.SaveChanges();

            return entity.Esemenynaplokod;
        }

        public static IOrderedQueryable<Models.Esemenynaplo> GetQuery(ossContext context, int Felhasznalokod)
        {
            var qry = context.Esemenynaplo.AsNoTracking()
              .Where(m => m.Felhasznalokod == Felhasznalokod)
              .OrderByDescending(m => m.Esemenynaplokod);

            return qry;
        }
    }
}
