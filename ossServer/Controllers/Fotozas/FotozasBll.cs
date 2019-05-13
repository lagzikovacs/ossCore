using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Dokumentum;
using ossServer.Controllers.Irat;
using ossServer.Controllers.Logon;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Hubs;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ossServer.Controllers.Fotozas
{
    public class FotozasBll
    {
        private static string edKey = "mézesmázos töklekvár";

        private static string Link(FotozasParam Fp)
        {
            // TODO valahogyan paraméterként
            //return "https://docport.hu/oss/fotozas?fp=" +
            return "http://localhost:4200/fotozas?fp=" +
            HttpUtility.UrlEncode(StringCipher.Encrypt(JsonConvert.SerializeObject(Fp), edKey));
        }

        public static string CreateNewLink(ossContext context, string sid, IratDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRATMOD);

            IratDal.Lock(context, dto.Iratkod, dto.Modositva);
            var entity = IratDal.Get(context, dto.Iratkod);

            var kikuldesikod = Guid.NewGuid().ToString();
            var up = new FotozasParam
            {
                Particiokod = (int)context.CurrentSession.Particiokod,
                Iratkod = dto.Iratkod,
                Kikuldesikod = kikuldesikod
            };

            entity.Kikuldesikod = kikuldesikod;
            entity.Kikuldesikodidopontja = DateTime.Now;
            IratDal.Update(context, entity);

            return Link(up);
        }
        public static string GetLink(ossContext context, string sid, IratDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.IRATMOD);

            IratDal.Lock(context, dto.Iratkod, dto.Modositva);
            var entity = IratDal.Get(context, dto.Iratkod);
            if (entity.Kikuldesikod == null)
                throw new Exception("Ez az irat még nem kapott fotózás linket!");

            var Up = new FotozasParam
            {
                Particiokod = (int)context.CurrentSession.Particiokod,
                Iratkod = entity.Iratkod,
                Kikuldesikod = entity.Kikuldesikod
            };

            return Link(Up);
        }

        public static FotozasDto Check(ossContext context, IHubContext<OssHub> hubcontext, string linkparam)
        {
            string uh = "Ügyféltér hiba {0} - értesítse a GridSolar sales-t!";

            FotozasParam Fp;

            try
            {
                Fp = JsonConvert.DeserializeObject<FotozasParam>(StringCipher.Decrypt(linkparam, edKey));
            }
            catch
            {
                throw new Exception(string.Format(uh, 1));
            }
            // adott particióban létezi-e az irat a kiküldési kóddal
            IratDal.FotozasCheck(context, Fp.Particiokod, Fp.Iratkod, Fp.Kikuldesikod);

            var result = new FotozasDto();

            try
            {
                // TODO jöhetnének konfig fájlból
                result.sid = LogonBll.Bejelentkezes(context, hubcontext, 
                    "fotozas", Crypt.MD5Hash("internacionale"), "", "", "");
            }
            catch
            {
                throw new Exception(string.Format(uh, 2));
            }

            // az Fotózás usernek az irat particióját kell tudni választani
            var csoport = LogonBll.Szerepkorok(context, result.sid)
                .Where(s => s.Particiokod == Fp.Particiokod).ToList();
            if (csoport.Count != 1)
                throw new Exception(string.Format(uh, 3));

            LogonBll.SzerepkorValasztas(context, result.sid, 
                csoport[0].Particiokod, csoport[0].Csoportkod);

            result.iratDto = IratBll.Select(context, result.sid, 0, 1,
                new List<SzMT> { new SzMT { Szempont = Szempont.Kod, Minta = Fp.Iratkod.ToString() } }, out _);
            result.dokumentumDto = DokumentumBll.Select(context, result.sid, Fp.Iratkod);

            return result;
        }
    }
}
