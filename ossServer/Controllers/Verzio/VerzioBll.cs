using ossServer.Controllers.Session;
using ossServer.Models;
using System.Threading.Tasks;

namespace ossServer.Controllers.Verzio
{
    public class VerzioBll
    {
        public static async Task<string> VerzioEsBuildAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            return await VerzioDal.GetAsync(context);
        }
    }
}
