using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Logon;
using ossServer.Controllers.Projekt;
using ossServer.Controllers.Session;
using ossServer.Controllers.Ugyfel;
using ossServer.Controllers.UgyfelterLog;
using ossServer.Enums;
using ossServer.Hubs;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ossServer.Controllers.Ugyfelter
{
    public class UgyfelterBll
    {
        private static readonly string edKey = "nagy dirrel durral";

        private static string Link(UgyfelterParam up)
        {
            // a cím elejét a kliens tudja
            return "ugyfelter?up=" +
                HttpUtility.UrlEncode(StringCipher.Encrypt(JsonConvert.SerializeObject(up), edKey));
        }

        public static async Task<string> CreateNewLinkAsync(ossContext context, string sid, UgyfelDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEKMOD);

            await UgyfelDal.Lock(context, dto.Ugyfelkod, dto.Modositva);
            var entity = await UgyfelDal.GetAsync(context, dto.Ugyfelkod);

            var kikuldesikod = Guid.NewGuid().ToString();
            var up = new UgyfelterParam
            {
                Particiokod = (int)context.CurrentSession.Particiokod,
                Ugyfelkod = dto.Ugyfelkod,
                Kikuldesikod = kikuldesikod
            };

            entity.Kikuldesikod = kikuldesikod;
            entity.Kikuldesikodidopontja = DateTime.Now;
            await UgyfelDal.UpdateAsync(context, entity);

            return Link(up);
        }

        public static async Task<string> GetLinkAsync(ossContext context, string sid, UgyfelDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEKMOD);

            await UgyfelDal.Lock(context, dto.Ugyfelkod, dto.Modositva);
            var entity = await UgyfelDal.GetAsync(context, dto.Ugyfelkod);
            if (entity.Kikuldesikod == null)
                throw new Exception("Ez az ügyfél még nem kapott ügyféltér linket!");

            var up = new UgyfelterParam
            {
                Particiokod = (int)context.CurrentSession.Particiokod,
                Ugyfelkod = entity.Ugyfelkod,
                Kikuldesikod = entity.Kikuldesikod
            };

            return Link(up);
        }

        public static async Task ClearLinkAsync(ossContext context, string sid, UgyfelDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEKMOD);

            await UgyfelDal.Lock(context, dto.Ugyfelkod, dto.Modositva);
            var entity = await UgyfelDal.GetAsync(context, dto.Ugyfelkod);

            entity.Kikuldesikod = null;
            entity.Kikuldesikodidopontja = null;
            await UgyfelDal.UpdateAsync(context, entity);
        }

        public static async Task<UgyfelterDto> UgyfelterCheckAsync(ossContext context, IHubContext<OssHub> hubcontext,
            IConfiguration config, string linkparam)
        {
            const string uh = "Ügyféltér hiba {0} - értesítse a GridSolar sales-t!";

            UgyfelterParam up;

            try
            {
                up = JsonConvert.DeserializeObject<UgyfelterParam>(StringCipher.Decrypt(linkparam, edKey));
            }
            catch
            {
                throw new Exception(string.Format(uh, 1));
            }
            // adott particióban létezi-e az ügyfél a kiküldési kóddal
            await UgyfelDal.UgyfelterCheckAsync(context, up.Particiokod, up.Ugyfelkod, up.Kikuldesikod);

            var result = new UgyfelterDto();

            try
            {
                result.sid = await LogonBll.BejelentkezesAsync(context, hubcontext,
                    config.GetValue<string>("Ugyfelter:user"),
                    Crypt.MD5Hash(config.GetValue<string>("Ugyfelter:password")), "", "", "");
            }
            catch
            {
                throw new Exception(string.Format(uh, 2));
            }

            // az Ügyféltér usernek az ügyfél particióját kell tudni választani
            var csoport = LogonBll.Szerepkorok(context, result.sid).Where(s => s.Particiokod == up.Particiokod).ToList();
            if (csoport.Count != 1)
                throw new Exception(string.Format(uh, 3));

            LogonBll.SzerepkorValasztas(context, result.sid, csoport[0].Particiokod, csoport[0].Csoportkod);

            // ügyféltér log
            await UgyfelterLogDal.AddAsync(context, new Models.Ugyfelterlog { Ugyfelkod = up.Ugyfelkod });

            result.ugyfelDto = await UgyfelBll.GetAsync(context, result.sid, up.Ugyfelkod);

            result.lstProjektDto = ProjektBll.Select(context, result.sid, 0, int.MaxValue, 0,
                new List<SzMT> { new SzMT { Szempont = Szempont.UgyfelKod, Minta = up.Ugyfelkod.ToString() } }, out _);

            return result;
        }
    }
}

