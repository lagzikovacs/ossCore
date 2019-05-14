using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatTermekdijDal
    {
        public static int Add(ossContext context, Bizonylattermekdij entity)
        {
            Register.Creation(context, entity);
            context.Bizonylattermekdij.Add(entity);
            context.SaveChanges();

            return entity.Bizonylattermekdijkod;
        }

        public static List<Bizonylattermekdij> Select(ossContext context, int bizonylatKod)
        {
            return context.Bizonylattermekdij
                .Where(s => s.Bizonylatkod == bizonylatKod)
                .OrderBy(s => s.Bizonylattermekdijkod).ToList();
        }

        public static void Delete(ossContext context, Bizonylattermekdij entity)
        {
            context.Bizonylattermekdij.Remove(entity);
            context.SaveChanges();
        }
    }
}
