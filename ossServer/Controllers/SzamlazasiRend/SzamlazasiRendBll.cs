using ossServer.Controllers.Csoport;
using ossServer.Controllers.Primitiv.Penznem;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.SzamlazasiRend
{
    public class SzamlazasiRendBll
    {
        public static SzamlazasiRendDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = SzamlazasiRendDal.Get(context, key);
            var result = ObjectUtils.Convert<Szamlazasirend, SzamlazasiRendDto>(entity);
            result.Penznem = entity.PenznemkodNavigation.Penznem1;

            return result;
        }

        public static async Task<SzamlazasiRendDto> CreateNewAsync(ossContext context, string sid)
        {
            const string minta = "HUF";

            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var result = new SzamlazasiRendDto();
            var lst = await PenznemDal.ReadAsync(context, minta);
            if (lst.Count == 1)
            {
                result.Penznemkod = lst[0].Penznemkod;
                result.Penznem = lst[0].Penznem1;
            }

            return result;
        }

        public static int Add(ossContext context, string sid, SzamlazasiRendDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = ObjectUtils.Convert<SzamlazasiRendDto, Szamlazasirend>(dto);
            return SzamlazasiRendDal.Add(context, entity);
        }

        public static List<SzamlazasiRendDto> Select(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entities = SzamlazasiRendDal.Select(context, projektKod);
            var result = new List<SzamlazasiRendDto>();

            foreach (var entity in entities)
            {
                var dto = ObjectUtils.Convert<Szamlazasirend, SzamlazasiRendDto>(entity);
                dto.Penznem = entity.PenznemkodNavigation.Penznem1;

                result.Add(dto);
            }

            return result;
        }

        public static async Task DeleteAsync(ossContext context, string sid, SzamlazasiRendDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            await SzamlazasiRendDal.Lock(context, dto.Szamlazasirendkod, dto.Modositva);
            var entity = SzamlazasiRendDal.Get(context, dto.Szamlazasirendkod);
            SzamlazasiRendDal.Delete(context, entity);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, SzamlazasiRendDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            await SzamlazasiRendDal.Lock(context, dto.Szamlazasirendkod, dto.Modositva);
            var entity = SzamlazasiRendDal.Get(context, dto.Szamlazasirendkod);
            ObjectUtils.Update(dto, entity);
            return SzamlazasiRendDal.Update(context, entity);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Szamlazasirendkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Osszeg", Title = "Összeg", Type = ColumnType.NUMBER },
                new ColumnSettings {Name="Penznem", Title = "Pénznem", Type = ColumnType.STRING },
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
