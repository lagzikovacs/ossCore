using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatTermekdijDal
    {
        public static async Task<int> AddAsync(ossContext context, Bizonylattermekdij entity)
        {
            Register.Creation(context, entity);
            await context.Bizonylattermekdij.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Bizonylattermekdijkod;
        }

        public static List<Bizonylattermekdij> Select(ossContext context, int bizonylatKod)
        {
            return context.Bizonylattermekdij
                .Where(s => s.Bizonylatkod == bizonylatKod)
                .OrderBy(s => s.Bizonylattermekdijkod).ToList();
        }

        public static async Task DeleteAsync(ossContext context, Bizonylattermekdij entity)
        {
            context.Bizonylattermekdij.Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
