using ossServer.Models;
using ossServer.Utils;

namespace ossServer.Controllers.UgyfelterLog
{
    public class UgyfelterLogDal
    {
        public static int Add(ossContext model, Models.Ugyfelterlog entity)
        {
            Register.Creation(model, entity);
            model.Ugyfelterlog.Add(entity);
            model.SaveChanges();

            return entity.Ugyfelterlogkod;
        }
    }
}
