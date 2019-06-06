using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.PenztarTetel
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PenztarTetelController : ControllerBase
    {
        private readonly ossContext _context;

        public PenztarTetelController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<Int32Result> Add([FromQuery] string sid, [FromBody] PenztarTetelDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = PenztarTetelBll.Add(_context, sid, dto);

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
        public async Task<PenztarTetelResult> CreateNew([FromQuery] string sid)
        {
            var result = new PenztarTetelResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<PenztarTetelDto> { PenztarTetelBll.CreateNew(_context, sid) };

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
        public async Task<PenztarTetelResult> Get([FromQuery] string sid, [FromBody] int key)
        {
            var result = new PenztarTetelResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<PenztarTetelDto> { PenztarTetelBll.Get(_context, sid, key) };

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
        public async Task<PenztarTetelResult> Select([FromQuery] string sid, [FromBody] PenztarTetelParam par)
        {
            var result = new PenztarTetelResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = PenztarTetelBll.Select(_context, sid, par.RekordTol, par.LapMeret, 
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
        public async Task<ColumnSettingsResult> GetGridSettings([FromQuery] string sid)
        {
            var result = new ColumnSettingsResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = PenztarTetelBll.GridSettings(_context, sid);

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
                    result.Result = PenztarTetelBll.ReszletekSettings(_context, sid);

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