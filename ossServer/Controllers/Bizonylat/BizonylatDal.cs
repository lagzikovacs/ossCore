using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatDal
    {
        public static int Add(ossContext context, Models.Bizonylat entity)
        {
            Register.Creation(context, entity);
            context.Bizonylat.Add(entity);
            context.SaveChanges();

            return entity.Bizonylatkod;
        }
    }
}
