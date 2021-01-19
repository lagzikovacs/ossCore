using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatTetelDal
    {
        public static async Task<int> AddAsync(ossContext context, Bizonylattetel entity)
        {
            Register.Creation(context, entity);
            await context.Bizonylattetel.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Bizonylattetelkod;
        }

        public static List<Bizonylattetel> Select(ossContext context, int bizonylatKod)
        {
            return context.Bizonylattetel
                .Where(s => s.Bizonylatkod == bizonylatKod)
                .OrderBy(s => s.Bizonylattetelkod).ToList();
        }

        public static async Task<int> UpdateAsync(ossContext context, Bizonylattetel entity)
        {
            await context.SaveChangesAsync();

            return entity.Bizonylatkod;
        }

        public static async Task DeleteAsync(ossContext context, Bizonylattetel entity)
        {
            context.Bizonylattetel.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
