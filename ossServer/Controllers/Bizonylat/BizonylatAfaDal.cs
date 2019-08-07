using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatAfaDal
    {
        public static async Task<int> AddAsync(ossContext context, Bizonylatafa entity)
        {
            Register.Creation(context, entity);
            await context.Bizonylatafa.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Bizonylatafakod;
        }

        public static List<Bizonylatafa> Select(ossContext context, int bizonylatKod)
        {
            return context.Bizonylatafa
                .Where(s => s.Bizonylatkod == bizonylatKod).ToList();
        }

        public static async Task DeleteAsync(ossContext context, Bizonylatafa entity)
        {
            context.Bizonylatafa.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
