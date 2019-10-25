using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ossServer.BaseResults;
using ossServer.Hubs;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Ugyfelkapcsolat
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UgyfelkapcsolatController : ControllerBase
    {
        private readonly ossContext _context;
        private readonly IHubContext<OssHub> _hubcontext;

        public UgyfelkapcsolatController(ossContext context, IHubContext<OssHub> hubcontext)
        {
            _context = context;
            _hubcontext = hubcontext;
        }

        [HttpPost]
        public async Task<UgyfelkapcsolatResult> CreateNew([FromQuery] string sid)
        {
            var result = new UgyfelkapcsolatResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<UgyfelkapcsolatDto> { await UgyfelkapcsolatBll.CreateNewAsync(_context, sid) };

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
        public async Task<Int32Result> Add([FromQuery] string sid, [FromBody] UgyfelkapcsolatDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await UgyfelkapcsolatBll.AddAsync(_context, sid, _hubcontext, dto);

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
        public async Task<BaseResults.EmptyResult> Delete([FromQuery] string sid, [FromBody] UgyfelkapcsolatDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    await UgyfelkapcsolatBll.DeleteAsync(_context, sid, dto);

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
        public async Task<UgyfelkapcsolatResult> Get([FromQuery] string sid, [FromBody] UgyfelkapcsolatGetParam param)
        {
            var result = new UgyfelkapcsolatResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<UgyfelkapcsolatDto> { await UgyfelkapcsolatBll.GetAsync(_context, sid, param) };

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
        public async Task<Int32Result> Update([FromQuery] string sid, [FromBody] UgyfelkapcsolatDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await UgyfelkapcsolatBll.UpdateAsync(_context, sid, dto);

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
        public async Task<UgyfelkapcsolatResult> Select([FromQuery] string sid, [FromBody] UgyfelkapcsolatParam par)
        {
            var result = new UgyfelkapcsolatResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    var t = await UgyfelkapcsolatBll.SelectAsync(_context, sid, 
                        par.RekordTol, par.LapMeret, par.Ugyfelkod, par.Fi, par.FromTo);

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
        public async Task<ColumnSettingsResult> GetGridSettings([FromQuery] string sid)
        {
            var result = new ColumnSettingsResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = UgyfelkapcsolatBll.GridSettings(_context, sid);

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
                    result.Result = UgyfelkapcsolatBll.ReszletekSettings(_context, sid);

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