using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Dokumentum;
using ossServer.Controllers.Irat;
using ossServer.Controllers.Logon;
using ossServer.Controllers.Projekt;
using ossServer.Controllers.ProjektKapcsolat;
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
        private static readonly string edKey = "mézesmázos töklekvár";

        private static string Link(FotozasParam Fp)
        {
            // a cím elejét a kliens tudja
            return "fotozas?fp=" + 
                HttpUtility.UrlEncode(StringCipher.Encrypt(JsonConvert.SerializeObject(Fp), edKey));
        }

        public static async Task<string> CreateNewLinkAsync(ossContext context, string sid, IratDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRATMOD);

            await IratDal.Lock(context, dto.Iratkod, dto.Modositva);
            var entity = await IratDal.GetAsync(context, dto.Iratkod);

            var kikuldesikod = Guid.NewGuid().ToString();
            var up = new FotozasParam
            {
                Particiokod = (int)context.CurrentSession.Particiokod,
                Iratkod = dto.Iratkod,
                Kikuldesikod = kikuldesikod
            };

            entity.Kikuldesikod = kikuldesikod;
            entity.Kikuldesikodidopontja = DateTime.Now;
            await IratDal.UpdateAsync(context, entity);

            return Link(up);
        }

        public static async Task ClearLinkAsync(ossContext context, string sid, IratDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEKMOD);

            await IratDal.Lock(context, dto.Iratkod, dto.Modositva);
            var entity = await IratDal.GetAsync(context, dto.Iratkod);

            entity.Kikuldesikod = null;
            entity.Kikuldesikodidopontja = null;
            await IratDal.UpdateAsync(context, entity);
        }

        public static async Task<string> GetLinkAsync(ossContext context, string sid, IratDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.IRATMOD);

            await IratDal.Lock(context, dto.Iratkod, dto.Modositva);
            var entity = await IratDal.GetAsync(context, dto.Iratkod);
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

        public static async Task<FotozasDto> CheckAsync(ossContext context, IHubContext<OssHub> hubcontext,
            IConfiguration config, string linkparam)
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
                result.sid = await LogonBll.BejelentkezesAsync(context, hubcontext, 
                    config.GetValue<string>("Fotozas:user"),
                    Crypt.MD5Hash(config.GetValue<string>("Fotozas:password")), "", "", "");
            }
            catch
            {
                throw new Exception(string.Format(uh, 2));
            }

            // az Fotózás usernek az irat particióját kell tudni választani
            var csoport = (await LogonBll.SzerepkorokAsync(context, result.sid))
                .Where(s => s.Particiokod == Fp.Particiokod).ToList();
            if (csoport.Count != 1)
                throw new Exception(string.Format(uh, 3));

            await LogonBll.SzerepkorValasztasAsync(context, result.sid, 
                csoport[0].Particiokod, csoport[0].Csoportkod);

            result.iratDto = (await IratBll.SelectAsync(context, result.sid, 0, 1,
                new List<SzMT> { new SzMT { Szempont = Szempont.Kod, Minta = Fp.Iratkod.ToString() } })).Item1;
            result.dokumentumDto = await DokumentumBll.SelectAsync(context, result.sid, Fp.Iratkod);

            var projektKapcsolatDto = ProjektKapcsolatBll.SelectByIrat(context, result.sid, Fp.Iratkod);
            if (projektKapcsolatDto.Count != 0)
                result.projektDto = (await ProjektBll.SelectAsync(context, result.sid, 0, 1, 0,
                    new List<SzMT> { new SzMT {Szempont = Szempont.Kod, Minta = projektKapcsolatDto[0].Projektkod.ToString() } })).Item1;

            return result;
        }
    }
}
