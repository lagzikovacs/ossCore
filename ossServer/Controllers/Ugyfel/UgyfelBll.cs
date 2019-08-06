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
        public static int Add(ossContext context, string sid, UgyfelDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEKMOD);

            var entity = ObjectUtils.Convert<UgyfelDto, Models.Ugyfel>(dto);
            UgyfelDal.Exists(context, entity);
            return UgyfelDal.Add(context, entity);
        }

        public static UgyfelDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEKMOD);

            return new UgyfelDto { Csoport = 0 };
        }

        public static async Task DeleteAsync(ossContext context, string sid, UgyfelDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEKMOD);

            await UgyfelDal.Lock(context, dto.Ugyfelkod, dto.Modositva);
            UgyfelDal.CheckReferences(context, dto.Ugyfelkod);
            var entity = UgyfelDal.Get(context, dto.Ugyfelkod);
            UgyfelDal.Delete(context, entity);
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

            return result;
        }

        public static UgyfelDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEK);

            var entity = UgyfelDal.Get(context, key);
            return Calc(entity);
        }

        public static List<UgyfelDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEK);

            var entities = UgyfelDal.Read(context, maszk);
            var result = new List<UgyfelDto>();

            foreach (var entity in entities)
                result.Add(Calc(entity));
            return result;
        }

        public static List<UgyfelDto> Select(ossContext context, string sid, int rekordTol, int lapMeret,
            int csoport, List<SzMT> szmt, out int osszesRekord)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEK);

            var qry = UgyfelDal.GetQuery(context, csoport, szmt);
            osszesRekord = qry.Count();
            var entities = qry.Skip(rekordTol).Take(lapMeret).ToList();
            var result = new List<UgyfelDto>();

            foreach (var entity in entities)
                result.Add(Calc(entity));

            return result;
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, UgyfelDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEKMOD);

            await UgyfelDal.Lock(context, dto.Ugyfelkod, dto.Modositva);
            var entity = UgyfelDal.Get(context, dto.Ugyfelkod);

            ObjectUtils.Update(dto, entity);
            UgyfelDal.ExistsAnother(context, entity);
            return UgyfelDal.Update(context, entity);
        }

        public static void ZoomCheck(ossContext context, string sid, int ugyfelkod, string ugyfel)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEK);

            UgyfelDal.ZoomCheck(context, ugyfelkod, ugyfel);
        }

        // TODO
        public static string vCard(ossContext context, string sid, int ugyfelkod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.UGYFELEK);

            var entity = UgyfelDal.Get(context, ugyfelkod);

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

