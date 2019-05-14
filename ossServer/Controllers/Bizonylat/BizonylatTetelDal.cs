using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatTetelDal
    {
        public static int Add(ossContext context, Bizonylattetel entity)
        {
            Register.Creation(context, entity);
            context.Bizonylattetel.Add(entity);
            context.SaveChanges();

            return entity.Bizonylattetelkod;
        }

        public static List<Bizonylattetel> Select(ossContext context, int bizonylatKod)
        {
            return context.Bizonylattetel
                .Where(s => s.Bizonylatkod == bizonylatKod)
                .OrderBy(s => s.Bizonylattetelkod).ToList();
        }

        public static void Delete(ossContext context, Bizonylattetel entity)
        {
            context.Bizonylattetel.Remove(entity);
            context.SaveChanges();
        }
    }
}
