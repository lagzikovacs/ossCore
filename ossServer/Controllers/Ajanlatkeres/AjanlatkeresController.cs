﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using ossServer.BaseResults;
using ossServer.Hubs;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ajanlatkeres
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AjanlatkeresController : ControllerBase
    {
        private readonly ossContext _context;
        private readonly IHubContext<OssHub> _hubcontext;
        private readonly IConfiguration _config;

        public AjanlatkeresController(ossContext context, IHubContext<OssHub> hubcontext, IConfiguration config)
        {
            _context = context;
            _hubcontext = hubcontext;
            _config = config;
        }

        [HttpPost]
        public async Task<BaseResults.EmptyResult> WebesAjanlatkeres([FromBody]WebesAjanlatkeresParam par)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    await AjanlatkeresBll.WebesAjanlatkeresAsync(_context, _hubcontext, _config, par);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<Int32Result> Add([FromQuery] string sid, [FromBody] AjanlatkeresDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await AjanlatkeresBll.AddAsync(_context, sid, dto);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<AjanlatkeresResult> CreateNew([FromQuery] string sid)
        {
            var result = new AjanlatkeresResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<AjanlatkeresDto> { await AjanlatkeresBll.CreateNewAsync(_context, sid) };

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<BaseResults.EmptyResult> Delete([FromQuery] string sid, 
            [FromBody] AjanlatkeresDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    await AjanlatkeresBll.DeleteAsync(_context, sid, dto);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<AjanlatkeresResult> Get([FromQuery] string sid, [FromBody] int key)
        {
            var result = new AjanlatkeresResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<AjanlatkeresDto> { await AjanlatkeresBll.GetAsync(_context, sid, key) };

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<AjanlatkeresResult> Select([FromQuery] string sid, [FromBody] AjanlatkeresParam par)
        {
            var result = new AjanlatkeresResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    var t = await AjanlatkeresBll.SelectAsync(_context, sid, par.RekordTol, par.LapMeret, par.Fi);
                    result.Result = t.Item1;
                    result.OsszesRekord = t.Item2;

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<Int32Result> Update([FromQuery] string sid, [FromBody] AjanlatkeresDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await AjanlatkeresBll.UpdateAsync(_context, sid, dto);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<ColumnSettingsResult> GetGridSettings([FromQuery] string sid)
        {
            var result = new ColumnSettingsResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = AjanlatkeresBll.GridSettings(_context, sid);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }

        [HttpPost]
        public async Task<ColumnSettingsResult> GetReszletekSettings([FromQuery] string sid)
        {
            var result = new ColumnSettingsResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = AjanlatkeresBll.ReszletekSettings(_context, sid);

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.InmostMessage();
                }

            return result;
        }
    }
}