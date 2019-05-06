using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;

namespace ossServer.Controllers.Particio
{
    public class ParticioBll
    {
        private static string edkey = "enter sandman";

        private static void EncryptDto(ParticioDto dto)
        {
            dto.NavFelhasznaloazonosito = StringCipher.Encrypt(dto.NavFelhasznaloazonosito, edkey);
            dto.NavFelhasznalojelszo = StringCipher.Encrypt(dto.NavFelhasznalojelszo, edkey);
            dto.NavAlairokulcs = StringCipher.Encrypt(dto.NavAlairokulcs, edkey);
            dto.NavCserekulcs = StringCipher.Encrypt(dto.NavCserekulcs, edkey);

            dto.SmtpFelhasznaloazonosito = StringCipher.Encrypt(dto.SmtpFelhasznaloazonosito, edkey);
            dto.SmtpFelhasznalojelszo = StringCipher.Encrypt(dto.SmtpFelhasznalojelszo, edkey);
        }
        private static void DecryptDto(ParticioDto dto)
        {
            dto.NavFelhasznaloazonosito = StringCipher.Decrypt(dto.NavFelhasznaloazonosito, edkey);
            dto.NavFelhasznalojelszo = StringCipher.Decrypt(dto.NavFelhasznalojelszo, edkey);
            dto.NavAlairokulcs = StringCipher.Decrypt(dto.NavAlairokulcs, edkey);
            dto.NavCserekulcs = StringCipher.Decrypt(dto.NavCserekulcs, edkey);

            dto.SmtpFelhasznaloazonosito = StringCipher.Decrypt(dto.SmtpFelhasznaloazonosito, edkey);
            dto.SmtpFelhasznalojelszo = StringCipher.Decrypt(dto.SmtpFelhasznalojelszo, edkey);
        }

        //TODO ez csak minimális ellenőrzés
        private static void CheckDto(ParticioDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            const string kot = " kitöltése kötelező!";

            if (string.IsNullOrEmpty(dto.Megnevezes))
                throw new Exception($"A Megnevezés{kot}");

            if (string.IsNullOrEmpty(dto.NavFelhasznaloazonosito))
                throw new Exception($"A NAV Azonosító{kot}");
            if (string.IsNullOrEmpty(dto.NavFelhasznalojelszo))
                throw new Exception($"A NAV Jelszó{kot}");
            if (string.IsNullOrEmpty(dto.NavAlairokulcs))
                throw new Exception($"A NAV Aláírókulcs{kot}");
            if (string.IsNullOrEmpty(dto.NavCserekulcs))
                throw new Exception($"A NAV Cserekulcs{kot}");

            if (string.IsNullOrEmpty(dto.SmtpFelhasznaloazonosito))
                throw new Exception($"Az Smtp Azonosító{kot}");
            if (string.IsNullOrEmpty(dto.SmtpFelhasznalojelszo))
                throw new Exception($"Az Smtp Jelszó{kot}");
        }

        public static ParticioDto Get(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PARTICIO);

            var entity = ParticioDal.Get(context);
            var dto = ObjectUtils.Convert<Models.Particio, ParticioDto>(entity);

            DecryptDto(dto);

            return dto;
        }

        public static int Update(ossContext context, string sid, ParticioDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PARTICIO);

            CheckDto(dto);
            EncryptDto(dto);

            ParticioDal.Lock(context, dto.Particiokod, dto.Modositva);
            var entity = ParticioDal.Get(context);
            ObjectUtils.Update(dto, entity);
            return ParticioDal.Update(context, entity);
        }
    }
}
