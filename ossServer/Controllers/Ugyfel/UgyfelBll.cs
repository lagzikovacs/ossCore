using Microsoft.EntityFrameworkCore;
using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ugyfel
{
    public class UgyfelBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, UgyfelDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEKMOD);

            var entity = ObjectUtils.Convert<UgyfelDto, Models.Ugyfel>(dto);
            await UgyfelDal.ExistsAsync(context, entity);
            return await UgyfelDal.AddAsync(context, entity);
        }

        public static async Task<UgyfelDto> CreateNewAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEKMOD);

            return new UgyfelDto { Csoport = 0 };
        }

        public static async Task DeleteAsync(ossContext context, string sid, UgyfelDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEKMOD);

            await UgyfelDal.Lock(context, dto.Ugyfelkod, dto.Modositva);
            await UgyfelDal.CheckReferencesAsync(context, dto.Ugyfelkod);
            var entity = await UgyfelDal.GetAsync(context, dto.Ugyfelkod);
            await UgyfelDal.DeleteAsync(context, entity);
        }

        public static string Cim(Models.Ugyfel entity)
        {
            var result = "";
            if (entity.HelysegkodNavigation != null)
                result = $"{entity.Iranyitoszam} {entity.HelysegkodNavigation.Helysegnev}, {entity.Kozterulet} {entity.Kozterulettipus} {entity.Hazszam}";

            return result;
        }

        private static UgyfelDto Calc(Models.Ugyfel entity)
        {
            var result = ObjectUtils.Convert<Models.Ugyfel, UgyfelDto>(entity);

            if (entity.HelysegkodNavigation != null)
            {
                result.Helysegnev = entity.HelysegkodNavigation.Helysegnev;
                result.Cim = Cim(entity);
            }
            if (entity.TevekenysegkodNavigation != null)
                result.Tevekenyseg = entity.TevekenysegkodNavigation.Tevekenyseg1;

            return result;
        }

        public static async Task<UgyfelDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEK);

            var entity = await UgyfelDal.GetAsync(context, key);
            return Calc(entity);
        }
        public static async Task<UgyfelDto> GetAsync(ossContext context, int key)
        {
            var entity = await UgyfelDal.GetAsync(context, key);
            return Calc(entity);
        }

        public static async Task<List<UgyfelDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEK);

            var entities = await UgyfelDal.ReadAsync(context, maszk);
            var result = new List<UgyfelDto>();

            foreach (var entity in entities)
                result.Add(Calc(entity));
            return result;
        }

        public static async Task<Tuple<List<UgyfelDto>, int>> SelectAsync(ossContext context, string sid, int rekordTol, int lapMeret,
            int csoport, List<SzMT> szmt)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEK);

            var qry = UgyfelDal.GetQuery(context, csoport, szmt);
            var osszesRekord = await qry.CountAsync();
            var entities = await qry.Skip(rekordTol).Take(lapMeret).ToListAsync();
            var result = new List<UgyfelDto>();

            foreach (var entity in entities)
                result.Add(Calc(entity));

            return new Tuple<List<UgyfelDto>, int>(result, osszesRekord);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, UgyfelDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEKMOD);

            await UgyfelDal.Lock(context, dto.Ugyfelkod, dto.Modositva);
            var entity = await UgyfelDal.GetAsync(context, dto.Ugyfelkod);

            ObjectUtils.Update(dto, entity);
            await UgyfelDal.ExistsAnotherAsync(context, entity);
            return await UgyfelDal.UpdateAsync(context, entity);
        }

        public static async Task ZoomCheckAsync(ossContext context, string sid, int ugyfelkod, string ugyfel)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEK);

            await UgyfelDal.ZoomCheckAsync(context, ugyfelkod, ugyfel);
        }

        // TODO
        public static async Task<string> vCardAsync(ossContext context, string sid, int ugyfelkod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.UGYFELEK);

            var entity = await UgyfelDal.GetAsync(context, ugyfelkod);

            var b = new StringBuilder();
            b.Append("BEGIN:VCARD\r\nVERSION:2.1\r\n");
            b.Append("FN;ENCODING=QUOTED-PRINTABLE;CHARSET=utf-8:").Append(entity.Nev).Append("\r\n");
            b.Append("N;ENCODING=QUOTED-PRINTABLE;CHARSET=utf-8:").Append(entity.Nev).Append(";;;\r\n");
            b.Append("END:VCARD\r\n");

            return b.ToString();
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Ugyfelkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Csoport", Title = "", Type = ColumnType.STRING },
                new ColumnSettings {Name="Nev", Title = "Név", Type = ColumnType.STRING },
                new ColumnSettings {Name="Ceg", Title = "Cég", Type = ColumnType.STRING },
                new ColumnSettings {Name="Beosztas", Title = "Beosztás", Type = ColumnType.STRING },
                new ColumnSettings {Name="Iranyitoszam", Title = "Irányítószám", Type = ColumnType.STRING },
                new ColumnSettings {Name="Helysegnev", Title = "Helységnév", Type = ColumnType.STRING },
                new ColumnSettings {Name="Kozterulet", Title = "Közterület", Type = ColumnType.STRING },
                new ColumnSettings {Name="Kozterulettipus", Title = "Közterület tipus", Type = ColumnType.STRING },
                new ColumnSettings {Name="Hazszam", Title = "Házszám", Type = ColumnType.STRING },
                new ColumnSettings {Name="Adoszam", Title = "Adószám", Type = ColumnType.STRING },
                new ColumnSettings {Name="Euadoszam", Title = "EU adószám", Type = ColumnType.STRING },
                new ColumnSettings {Name="Telefon", Title = "Telefon", Type = ColumnType.STRING },
                new ColumnSettings {Name="Email", Title = "Email", Type = ColumnType.STRING },
                new ColumnSettings {Name="Tevekenyseg", Title = "Tevékenység", Type = ColumnType.STRING },
                new ColumnSettings {Name="Egyeblink", Title = "Egyéb link", Type = ColumnType.STRING },
                new ColumnSettings {Name="Ajanlotta", Title = "Ajánlotta", Type = ColumnType.STRING },
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

