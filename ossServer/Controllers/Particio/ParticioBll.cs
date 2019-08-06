using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.Particio
{
    public class ParticioBll
    {
        private static string edkey = "enter sandman";

        private static void EncryptDto(ParticioDto dto)
        {
            if (!string.IsNullOrEmpty(dto.NavFelhasznaloazonosito))
                dto.NavFelhasznaloazonosito = StringCipher.Encrypt(dto.NavFelhasznaloazonosito, edkey);
            if (!string.IsNullOrEmpty(dto.NavFelhasznalojelszo))
                dto.NavFelhasznalojelszo = StringCipher.Encrypt(dto.NavFelhasznalojelszo, edkey);
            if (!string.IsNullOrEmpty(dto.NavAlairokulcs))
                dto.NavAlairokulcs = StringCipher.Encrypt(dto.NavAlairokulcs, edkey);
            if (!string.IsNullOrEmpty(dto.NavCserekulcs))
                dto.NavCserekulcs = StringCipher.Encrypt(dto.NavCserekulcs, edkey);

            if (!string.IsNullOrEmpty(dto.SmtpFelhasznaloazonosito))
                dto.SmtpFelhasznaloazonosito = StringCipher.Encrypt(dto.SmtpFelhasznaloazonosito, edkey);
            if (!string.IsNullOrEmpty(dto.SmtpFelhasznalojelszo))
                dto.SmtpFelhasznalojelszo = StringCipher.Encrypt(dto.SmtpFelhasznalojelszo, edkey);
        }
        private static void DecryptDto(ParticioDto dto)
        {
            if (!string.IsNullOrEmpty(dto.NavFelhasznaloazonosito))
                dto.NavFelhasznaloazonosito = StringCipher.Decrypt(dto.NavFelhasznaloazonosito, edkey);
            if (!string.IsNullOrEmpty(dto.NavFelhasznalojelszo))
                dto.NavFelhasznalojelszo = StringCipher.Decrypt(dto.NavFelhasznalojelszo, edkey);
            if (!string.IsNullOrEmpty(dto.NavAlairokulcs))
                dto.NavAlairokulcs = StringCipher.Decrypt(dto.NavAlairokulcs, edkey);
            if (!string.IsNullOrEmpty(dto.NavCserekulcs))
                dto.NavCserekulcs = StringCipher.Decrypt(dto.NavCserekulcs, edkey);

            if (!string.IsNullOrEmpty(dto.SmtpFelhasznaloazonosito))
                dto.SmtpFelhasznaloazonosito = StringCipher.Decrypt(dto.SmtpFelhasznaloazonosito, edkey);
            if (!string.IsNullOrEmpty(dto.SmtpFelhasznalojelszo))
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

        public static async Task<int> UpdateAsync(ossContext context, string sid, ParticioDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PARTICIO);

            // csak akkor, ha NAV feltöltés van
            // CheckDto(dto);
            EncryptDto(dto);

            await ParticioDal.Lock(context, dto.Particiokod, dto.Modositva);
            var entity = ParticioDal.Get(context);
            ObjectUtils.Update(dto, entity);
            return ParticioDal.Update(context, entity);
        }
    }
}
