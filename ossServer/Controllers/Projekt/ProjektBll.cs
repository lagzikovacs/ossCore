using ossServer.Controllers.Csoport;
using ossServer.Controllers.Primitiv.Penznem;
using ossServer.Controllers.Session;
using ossServer.Controllers.Ugyfel;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Projekt
{
    public class ProjektBll
    {
        public static int Add(ossContext context, string sid, ProjektDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKTMOD);

            var entity = ObjectUtils.Convert<ProjektDto, Models.Projekt>(dto);
            return ProjektDal.Add(context, entity);
        }

        public static async Task<ProjektDto> CreateNewAsync(ossContext context, string sid)
        {
            const string minta = "HUF";

            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKTMOD);

            var result = new ProjektDto
            {
                Keletkezett = DateTime.Now.Date,
                Vallalasiarnetto = 0,
                Muszakiallapot = "",
                Inverterallapot = "",
                Napelemallapot = ""
            };
            var lst = await PenznemDal.ReadAsync(context, minta);
            if (lst.Count == 1)
            {
                result.Penznemkod = lst[0].Penznemkod;
                result.Penznem = lst[0].Penznem1;
            }

            return result;
        }

        public static async Task DeleteAsync(ossContext context, string sid, ProjektDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKTMOD);

            await ProjektDal.Lock(context, dto.Projektkod, dto.Modositva);
            ProjektDal.CheckReferences(context, dto.Projektkod);
            var entity = ProjektDal.Get(context, dto.Projektkod);
            ProjektDal.Delete(context, entity);
        }

        private static ProjektDto Calc(Models.Projekt entity)
        {
            var result = ObjectUtils.Convert<Models.Projekt, ProjektDto>(entity);

            result.Penznem = entity.PenznemkodNavigation.Penznem1;
            result.Ugyfelnev = entity.UgyfelkodNavigation.Nev;
            result.Ugyfelcim = UgyfelBll.Cim(entity.UgyfelkodNavigation);
            result.Ugyfeltelefonszam = entity.UgyfelkodNavigation.Telefon;
            result.Ugyfelemail = entity.UgyfelkodNavigation.Email;

            return result;
        }

        public static ProjektDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = ProjektDal.Get(context, key);
            return Calc(entity);
        }

        public static List<ProjektDto> Select(ossContext context, string sid, int rekordTol, int lapMeret, 
            int statusz, List<SzMT> szmt, out int osszesRekord)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var qry = ProjektDal.GetQuery(context, statusz, szmt);
            osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();

            var result = new List<ProjektDto>();
            foreach (var entity in entities)
                result.Add(Calc(entity));

            return result;
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, ProjektDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKTMOD);

            await ProjektDal.Lock(context, dto.Projektkod, dto.Modositva);
            var entity = ProjektDal.Get(context, dto.Projektkod);

            ObjectUtils.Update(dto, entity);
            return ProjektDal.Update(context, entity);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Projektkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Statusz", Title = "Stsz", Type = ColumnType.INT },
                new ColumnSettings {Name="Muszakiallapot", Title = "Műszaki állapot", Type = ColumnType.STRING },
                new ColumnSettings {Name="Ugyfelnev", Title = "Ügyfél", Type = ColumnType.STRING },
                new ColumnSettings {Name="Ugyfelcim", Title = "Cím", Type = ColumnType.STRING },
                new ColumnSettings {Name="Ugyfeltelefonszam", Title = "Telefon", Type = ColumnType.STRING },
                new ColumnSettings {Name="Ugyfelemail", Title = "Email", Type = ColumnType.STRING },
                new ColumnSettings {Name="Telepitesicim", Title = "Telepítési cím", Type = ColumnType.STRING },
                new ColumnSettings {Name="Projektjellege", Title = "A projekt jellege", Type = ColumnType.STRING },
                new ColumnSettings {Name="Inverter", Title = "Inverter", Type = ColumnType.STRING },
                new ColumnSettings {Name="Inverterallapot", Title = "", Type = ColumnType.STRING },
                new ColumnSettings {Name="Napelem", Title = "Napelem", Type = ColumnType.STRING },
                new ColumnSettings {Name="Napelemallapot", Title = "", Type = ColumnType.STRING },
                new ColumnSettings {Name="Dckw", Title = "DC, kW", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Ackva", Title = "AC, kVA", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Vallalasiarnetto", Title = "Vállalási ár, netto", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Penznem", Title = "Pénznem", Type = ColumnType.STRING },
                new ColumnSettings {Name="Munkalapszam", Title = "Munkalapszám", Type = ColumnType.STRING },
                new ColumnSettings {Name="Keletkezett", Title = "Keletkezett", Type = ColumnType.DATE },
                new ColumnSettings {Name="Megrendelve", Title = "Megrendelve", Type = ColumnType.DATE },
                new ColumnSettings {Name="Kivitelezesihatarido", Title = "Kivitelezési határidő", Type = ColumnType.DATE },
                new ColumnSettings {Name="Megjegyzes", Title = "Megjegyzés", Type = ColumnType.STRING },
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
