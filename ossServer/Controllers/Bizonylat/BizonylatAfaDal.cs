using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatAfaDal
    {
        public static int Add(ossContext context, Bizonylatafa entity)
        {
            Register.Creation(context, entity);
            context.Bizonylatafa.Add(entity);
            context.SaveChanges();

            return entity.Bizonylatafakod;
        }

        public static List<Bizonylatafa> Select(ossContext context, int bizonylatKod)
        {
            return context.Bizonylatafa
                .Where(s => s.Bizonylatkod == bizonylatKod).ToList();
        }

        public static void Delete(ossContext context, Bizonylatafa entity)
        {
            context.Bizonylatafa.Remove(entity);
            context.SaveChanges();
        }
    }
}
