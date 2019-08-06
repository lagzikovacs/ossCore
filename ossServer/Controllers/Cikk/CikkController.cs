using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Enums;
using ossServer.Models;
using ossServer.Utils;

namespace ossServer.Controllers.Cikk
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CikkController : ControllerBase
    {
        private readonly ossContext _context;

        public CikkController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<Int32Result> Add([FromQuery] string sid, [FromBody] CikkDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = CikkBll.Add(_context, sid, dto);

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
        public async Task<CikkResult> CreateNew([FromQuery] string sid)
        {
            var result = new CikkResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<CikkDto> { CikkBll.CreateNew(_context, sid) };

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
        public async Task<BaseResults.EmptyResult> Delete([FromQuery] string sid, [FromBody] CikkDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    await CikkBll.DeleteAsync(_context, sid, dto);

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
        public async Task<CikkResult> Get([FromQuery] string sid, [FromBody] int key)
        {
            var result = new CikkResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<CikkDto> { CikkBll.Get(_context, sid, key) };

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
        public async Task<CikkResult> Read([FromQuery] string sid, [FromBody] string maszk)
        {
            var result = new CikkResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = CikkBll.Read(_context, sid, maszk);
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
        public async Task<Int32Result> Update([FromQuery] string sid, [FromBody] CikkDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await CikkBll.UpdateAsync(_context, sid, dto);

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
        public async Task<CikkResult> Select([FromQuery] string sid, [FromBody] CikkParam par)
        {
            var result = new CikkResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = CikkBll.Select(_context, sid, par.RekordTol, par.LapMeret, 
                        par.Fi, out var osszesRekord);
                    result.OsszesRekord = osszesRekord;

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
        public async Task<CikkMozgasResult> Mozgas([FromQuery] string sid, [FromBody] CikkMozgasParam par)
        {
            var result = new CikkMozgasResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = CikkBll.Mozgas(_context, sid, par.CikkKod, 
                        (BizonylatTipus)par.BizonylatTipusKod);

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
        public async Task<BaseResults.EmptyResult> ZoomCheck([FromQuery] string sid, [FromBody] CikkZoomParameter par)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    CikkBll.ZoomCheck(_context, sid, par.CikkKod, par.Cikk);

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
                    result.Result = CikkBll.GridSettings(_context, sid);

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
                    result.Result = CikkBll.ReszletekSettings(_context, sid);

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