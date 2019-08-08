using Microsoft.EntityFrameworkCore;
using ossServer.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Esemenynaplo
{
    public class EsemenynaploDal
    {
        public static async Task<int> AddAsync(ossContext context, Models.Esemenynaplo entity)
        {
            await context.Esemenynaplo.AddRangeAsync(entity);
            await context.SaveChangesAsync();

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
