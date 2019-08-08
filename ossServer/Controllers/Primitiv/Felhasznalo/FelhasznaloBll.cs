using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Felhasznalo
{
    public class FelhasznaloBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, FelhasznaloDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.FELHASZNALOMOD);

            var entity = ObjectUtils.Convert<FelhasznaloDto, Models.Felhasznalo>(dto);
            entity.Jelszo = Crypt.MD5Hash("");
            await FelhasznaloDal.ExistsAsync(context, entity);
            return await FelhasznaloDal.AddAsync(context, entity);
        }

        public static async Task<FelhasznaloDto> CreateNewAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PRIMITIVEKMOD);
            return new FelhasznaloDto { Statusz = "OK", Statuszkelte = DateTime.Now.Date, Logonlog = true };
        }

        public static async Task<FelhasznaloDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.FELHASZNALO);

            var entity = await FelhasznaloDal.GetAsync(context, key);
            return ObjectUtils.Convert<Models.Felhasznalo, FelhasznaloDto>(entity);
        }

        public static async Task<List<FelhasznaloDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.FELHASZNALO);

            var entities = await FelhasznaloDal.ReadAsync(context, maszk);
            return ObjectUtils.Convert<Models.Felhasznalo, FelhasznaloDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, FelhasznaloDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.FELHASZNALOMOD);

            await FelhasznaloDal.Lock(context, dto.Felhasznalokod, dto.Modositva);
            var entity = await FelhasznaloDal.GetAsync(context, dto.Felhasznalokod);

            if (entity.Statusz != dto.Statusz)
                dto.Statuszkelte = DateTime.Now.Date;

            ObjectUtils.Update(dto, entity);
            await FelhasznaloDal.ExistsAnotherAsync(context, entity);
            return await FelhasznaloDal.UpdateAsync(context, entity);
        }

        public static async Task DeleteAsync(ossContext context, string sid, FelhasznaloDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.FELHASZNALOMOD);

            await FelhasznaloDal.Lock(context, dto.Felhasznalokod, dto.Modositva);
            await FelhasznaloDal.CheckReferencesAsync(context, dto.Felhasznalokod);
            var entity = await FelhasznaloDal.GetAsync(context, dto.Felhasznalokod);
            await FelhasznaloDal.DeleteAsync(context, entity);
        }

        //rendszergazdának, felülírja a jelszót
        //a jelszó legyen hash-elt
        public static async Task JelszoBeallitasAsync(ossContext context, string sid, int felhasznaloKod, string jelszo, DateTime utolsoModositas)
        {
            SessionBll.Check(context, sid);
            // TODO: külön jog legyen ehhez
            await CsoportDal.JogeAsync(context, JogKod.FELHASZNALOMOD);

            await FelhasznaloDal.Lock(context, felhasznaloKod, utolsoModositas);
            var entity = await FelhasznaloDal.GetAsync(context, felhasznaloKod);
            entity.Jelszo = jelszo;
            await FelhasznaloDal.UpdateAsync(context, entity);
        }

        //usernek, csak a sajátot lehet, a régit ellenőrzi
        //a jelszó legyen hash-elt
        public static async Task JelszoCsereAsync(ossContext context, string sid, string regiJelszo, string ujJelszo)
        {
            SessionBll.Check(context, sid);
            //nincs külön jog, csak a bejelentkezett, szerepkört is választott usernek működik

            // TODO: nincs lock, felülíródik a jelszó...
            var entity = await FelhasznaloDal.GetAsync(context, context.CurrentSession.Felhasznalokod);
            if (entity.Jelszo.ToUpper() != regiJelszo.ToUpper())
                throw new Exception("A régi jelszó hibás!");
            entity.Jelszo = ujJelszo;
            await FelhasznaloDal.UpdateAsync(context, entity);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Felhasznalokod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Azonosito", Title = "Azonosító", Type = ColumnType.STRING },
                new ColumnSettings {Name="Nev", Title = "Név", Type = ColumnType.STRING },
                new ColumnSettings {Name="Telefon", Title = "Telefon", Type = ColumnType.STRING },
                new ColumnSettings {Name="Email", Title = "E-mail", Type = ColumnType.STRING },
                new ColumnSettings {Name="Statusz", Title = "Státusz", Type = ColumnType.STRING },
                new ColumnSettings {Name="Statuszkelte", Title = "A státusz kelte", Type = ColumnType.DATE },
                new ColumnSettings {Name="Logonlog", Title = "Log", Type = ColumnType.BOOL },
            };
        }

        public static List<ColumnSettings> ReszletekColumns()
        {
            return ColumnSettingsUtil.AddIdobelyeg(GridColumns());
        }

        public static List<ColumnSettings> GridSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            return GridColumns();
        }

        public static List<ColumnSettings> ReszletekSettings(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            return ReszletekColumns();
        }
    }
}
