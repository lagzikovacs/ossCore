using Microsoft.AspNetCore.SignalR;
using ossServer.Controllers.Esemenynaplo;

namespace ossServer.Hubs
{
    public class HubUtils
    {
        public static void Uzenet(IHubContext<OssHub> hubcontext, string felhasznalo, 
            string uzenet)
        {
            var msg = $"{felhasznalo} - {uzenet}";
            hubcontext.Clients.All.SendAsync("Uzenet", msg);
        }

        public static void Uzenet(IHubContext<OssHub> hubcontext, string felhasznalo,
            EsemenynaploBejegyzesek esemeny)
        {
            var msg = $"{felhasznalo} - {EsemenynaploBll.Jelentes(EsemenynaploBll.EseBeJel(), esemeny)}";
            hubcontext.Clients.All.SendAsync("Uzenet", msg);
        }
    }
}
