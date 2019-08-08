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
        public static async Task<SzamlazasiRendDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = await SzamlazasiRendDal.GetAsync(context, key);
            var result = ObjectUtils.Convert<Szamlazasirend, SzamlazasiRendDto>(entity);
            result.Penznem = entity.PenznemkodNavigation.Penznem1;

            return result;
        }

        public static async Task<SzamlazasiRendDto> CreateNewAsync(ossContext context, string sid)
        {
            const string minta = "HUF";

            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var result = new SzamlazasiRendDto();
            var lst = await PenznemDal.ReadAsync(context, minta);
            if (lst.Count == 1)
            {
                result.Penznemkod = lst[0].Penznemkod;
                result.Penznem = lst[0].Penznem1;
            }

            return result;
        }

        public static async Task<int> AddAsync(ossContext context, string sid, SzamlazasiRendDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entity = ObjectUtils.Convert<SzamlazasiRendDto, Szamlazasirend>(dto);
            return await SzamlazasiRendDal.AddAsync(context, entity);
        }

        public static async Task<List<SzamlazasiRendDto>> SelectAsync(ossContext context, string sid, int projektKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            var entities = await SzamlazasiRendDal.SelectAsync(context, projektKod);
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
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            await SzamlazasiRendDal.Lock(context, dto.Szamlazasirendkod, dto.Modositva);
            var entity = await SzamlazasiRendDal.GetAsync(context, dto.Szamlazasirendkod);
            await SzamlazasiRendDal.DeleteAsync(context, entity);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, SzamlazasiRendDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.PROJEKT);

            await SzamlazasiRendDal.Lock(context, dto.Szamlazasirendkod, dto.Modositva);
            var entity = await SzamlazasiRendDal.GetAsync(context, dto.Szamlazasirendkod);
            ObjectUtils.Update(dto, entity);
            return await SzamlazasiRendDal.UpdateAsync(context, entity);
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
