using ossServer.Controllers.Session;
using ossServer.Models;

namespace ossServer.Controllers.Verzio
{
    public class VerzioBll
    {
        public static string VerzioEsBuild(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            return VerzioDal.Get(context);
        }
    }
}
