﻿using ossServer.Controllers.Primitiv.Felhasznalo;
using ossServer.Controllers.Session;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ossServer.Controllers.Csoport
{
    public class CsoportBll
    {
        public static async Task<int> AddAsync(ossContext context, string sid, CsoportDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CSOPORT);

            var entity = ObjectUtils.Convert<CsoportDto, Models.Csoport>(dto);
            await CsoportDal.ExistsAsync(context, entity);
            return await CsoportDal.AddAsync(context, entity);
        }

        public static async Task<CsoportDto> CreateNewAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CSOPORT);

            return new CsoportDto();
        }

        public static async Task DeleteAsync(ossContext context, string sid, CsoportDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CSOPORT);

            await CsoportDal.Lock(context, dto.Csoportkod, dto.Modositva);
            await CsoportDal.CheckReferencesAsync(context, dto.Csoportkod);
            var entity = await CsoportDal.GetAsync(context, dto.Csoportkod);
            await CsoportDal.DeleteAsync(context, entity);
        }

        public static async Task<CsoportDto> GetAsync(ossContext context, string sid, int key)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CSOPORT);

            var entity = await CsoportDal.GetAsync(context, key);
            return ObjectUtils.Convert<Models.Csoport, CsoportDto>(entity);
        }

        public static async Task<List<CsoportDto>> ReadAsync(ossContext context, string sid, string maszk)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CSOPORT);

            var entities = await CsoportDal.ReadAsync(context, maszk);
            var result = new List<CsoportDto>();

            foreach (var e in entities)
            {
                var r = ObjectUtils.Convert<Models.Csoport, CsoportDto>(e);
                r.Particiomegnevezes = e.ParticiokodNavigation.Megnevezes;

                result.Add(r);
            }

            return result;
        }

        public static async Task<int> UpdateAsync(ossContext context, string sid, CsoportDto dto)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CSOPORT);

            await CsoportDal.Lock(context, dto.Csoportkod, dto.Modositva);
            var entity = await CsoportDal.GetAsync(context, dto.Csoportkod);
            ObjectUtils.Update(dto, entity);
            await CsoportDal.ExistsAnotherAsync(context, entity);

            return await CsoportDal.UpdateAsync(context, entity);
        }

        public static async Task<List<FelhasznaloDto>> SelectCsoportFelhasznaloAsync(ossContext context, string sid, int csoportKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CSOPORT);

            var csF = await CsoportDal.SelectCsoportFelhasznaloAsync(context, csoportKod);
            var entities = await FelhasznaloDal.ReadAsync(context, "");

            var result = new List<FelhasznaloDto>();

            foreach (var e in entities)
            {
                var dto = ObjectUtils.Convert<Felhasznalo, FelhasznaloDto>(e);
                dto.Csoporttag = csF.Contains(dto.Felhasznalokod);

                result.Add(dto);
            }

            return result;
        }

        public static async Task<List<LehetsegesJogDto>> SelectCsoportJogAsync(ossContext context, string sid, int csoportKod)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CSOPORT);

            var csJ = await CsoportDal.SelectCsoportJogAsync(context, csoportKod);
            var entities = JogDal.Read(context, "");

            var result = new List<LehetsegesJogDto>();

            foreach (var e in entities)
            {
                var dto = ObjectUtils.Convert<Lehetsegesjog, LehetsegesJogDto>(e);
                dto.Csoporttag = csJ.Contains(dto.Lehetsegesjogkod);

                result.Add(dto);
            }

            return result.OrderBy(s => s.Jog).ToList();
        }

        public static async Task CsoportFelhasznaloBeKiAsync(ossContext context, string sid, int csoportKod, int felhasznaloKod, bool Be)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CSOPORT);

            if (Be)
                await CsoportDal.CsoportFelhasznaloBeAsync(context, csoportKod, felhasznaloKod);
            else
                await CsoportDal.CsoportFelhasznaloKiAsync(context, csoportKod, felhasznaloKod);
        }

        public static async Task CsoportJogBeKiAsync(ossContext context, string sid, int csoportKod, int lehetsegesJogKod, bool Be)
        {
            SessionBll.Check(context, sid);
            await CsoportDal.JogeAsync(context, JogKod.CSOPORT);

            if (Be)
                await CsoportDal.CsoportJogBeAsync(context, csoportKod, lehetsegesJogKod);
            else
                await CsoportDal.CsoportJogKiAsync(context, csoportKod, lehetsegesJogKod);
        }

        public static async Task<List<JogKod>> JogaimAsync(ossContext context, string sid)
        {
            SessionBll.Check(context, sid);

            var lst = await CsoportDal.JogaimAsync(context);

            var result = new List<JogKod>();
            foreach (var l in lst)
                result.Add((JogKod)Enum.Parse(typeof(JogKod), l));

            return result;
        }

        public static List<ColumnSettings> GridColumns()
        {
            return new List<ColumnSettings>
            {
                new ColumnSettings {Name="Csoportkod", Title = "Id", Type = ColumnType.INT },
                new ColumnSettings {Name="Csoport1", Title = "Csoport", Type = ColumnType.STRING },
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
