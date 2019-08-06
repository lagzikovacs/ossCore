﻿using ossServer.Controllers.Csoport;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Helyseg
{
    public class HelysegBll
    {
        public static int Add(ossContext context, string sid, HelysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            var entity = ObjectUtils.Convert<HelysegDto, Models.Helyseg>(dto);
            HelysegDal.Exists(context, entity);
            return HelysegDal.Add(context, entity);
        }

        public static HelysegDto CreateNew(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);
            return new HelysegDto();
        }

        public static async Task DeleteAsync(ossContext context, string sid, HelysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            await HelysegDal.Lock(context, dto.Helysegkod, dto.Modositva);
            HelysegDal.CheckReferences(context, dto.Helysegkod);
            var entity = HelysegDal.Get(context, dto.Helysegkod);
            HelysegDal.Delete(context, entity);
        }

        public static HelysegDto Get(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entity = HelysegDal.Get(context, key);
            return ObjectUtils.Convert<Models.Helyseg, HelysegDto>(entity);
        }

        public static List<HelysegDto> Read(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            var entities = HelysegDal.Read(context, maszk);
            return ObjectUtils.Convert<Models.Helyseg, HelysegDto>(entities);
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, HelysegDto dto)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEKMOD);

            await HelysegDal.Lock(context, dto.Helysegkod, dto.Modositva);
            var entity = HelysegDal.Get(context, dto.Helysegkod);
            ObjectUtils.Update(dto, entity);
            HelysegDal.ExistsAnother(context, entity);
            return HelysegDal.Update(context, entity);
        }

        public static void ZoomCheck(ossContext context, string sid, int helysegkod, string helysegnev)
        {
            SessionBll.Check(context, sid);
            CsoportDal.Joge(context, JogKod.PRIMITIVEK);

            HelysegDal.ZoomCheck(context, helysegkod, helysegnev);
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Helysegkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Helysegnev", Title = "Helységnév", Type = ColumnType.STRING },
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
