using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Felhasznalo
{
    public class FelhasznaloBll
    {
        public static int Add(ossContext context, string sid, FelhasznaloDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.FELHASZNALOMOD);

            var entity = ObjectUtils.Convert<FelhasznaloDto, Models.Felhasznalo>(dto);
            entity.Jelszo = Crypt.MD5Hash("");
            FelhasznaloDal.Exists(context, entity);
            return FelhasznaloDal.Add(context, entity);
        }

        public static FelhasznaloDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);
            return new FelhasznaloDto { Statusz = "OK", Statuszkelte = DateTime.Now.Date, Logonlog = true };
        }

        public static FelhasznaloDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.FELHASZNALO);

            var entity = FelhasznaloDal.Get(context, key);
            return ObjectUtils.Convert<Models.Felhasznalo, FelhasznaloDto>(entity);
        }

        public static List<FelhasznaloDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.FELHASZNALO);

            var entities = FelhasznaloDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Felhasznalo, FelhasznaloDto>(entities);
        }

        public static int Update(ossContext context, string sid, FelhasznaloDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.FELHASZNALOMOD);

            FelhasznaloDal.Lock(context, dto.Felhasznalokod, dto.Modositva);
            var entity = FelhasznaloDal.Get(context, dto.Felhasznalokod);

            if (entity.Statusz != dto.Statusz)
                dto.Statuszkelte = DateTime.Now.Date;

            ObjectUtils.Update(dto, entity);
            FelhasznaloDal.ExistsAnother(context, entity);
            return FelhasznaloDal.Update(context, entity);
        }

        public static void Delete(ossContext context, string sid, FelhasznaloDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.FELHASZNALOMOD);

            FelhasznaloDal.Lock(context, dto.Felhasznalokod, dto.Modositva);
            FelhasznaloDal.CheckReferences(context, dto.Felhasznalokod);
            var entity = FelhasznaloDal.Get(context, dto.Felhasznalokod);
            FelhasznaloDal.Delete(context, entity);
        }

        //rendszergazdának, felülírja a jelszót
        //a jelszó legyen hash-elt
        public static void JelszoBeallitas(ossContext context, string sid, int felhasznaloKod, string jelszo, DateTime utolsoModositas)
        {
            SessionBll.Check(context, sid);
            // TODO: külön jog legyen ehhez
            CsoportDal.Joge(context, JogKod.FELHASZNALOMOD);

            FelhasznaloDal.Lock(context, felhasznaloKod, utolsoModositas);
            var entity = FelhasznaloDal.Get(context, felhasznaloKod);
            entity.Jelszo = jelszo;
            FelhasznaloDal.Update(context, entity);
        }

        //usernek, csak a sajátot lehet, a régit ellenőrzi
        //a jelszó legyen hash-elt
        public static void JelszoCsere(ossContext context, string sid, string regiJelszo, string ujJelszo)
        {
            SessionBll.Check(context, sid);
            //nincs külön jog, csak a bejelentkezett, szerepkört is választott usernek működik

            // TODO: nincs lock, felülíródik a jelszó...
            var entity = FelhasznaloDal.Get(context, context.CurrentSession.Felhasznalokod);
            if (entity.Jelszo.ToUpper() != regiJelszo.ToUpper())
                throw new Exception("A régi jelszó hibás!");
            entity.Jelszo = ujJelszo;
            FelhasznaloDal.Update(context, entity);
        }
    }
}
