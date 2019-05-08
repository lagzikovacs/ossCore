using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;

namespace ossServer.Controllers.Ugyfel
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UgyfelController : ControllerBase
    {
        private readonly ossContext _context;

        public UgyfelController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<Int32Result> Add([FromQuery] string sid, [FromBody] UgyfelDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = UgyfelBll.Add(_context, sid, dto);

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
        public async Task<UgyfelResult> CreateNew([FromQuery] string sid)
        {
            var result = new UgyfelResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<UgyfelDto> { UgyfelBll.CreateNew(_context, sid) };

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
        public async Task<BaseResults.EmptyResult> Delete([FromQuery] string sid, [FromBody] UgyfelDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    UgyfelBll.Delete(_context, sid, dto);

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
        public async Task<UgyfelResult> Get([FromQuery] string sid, [FromBody] int key)
        {
            var result = new UgyfelResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<UgyfelDto> { UgyfelBll.Get(_context, sid, key) };

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
        public async Task<UgyfelResult> Read([FromQuery] string sid, [FromBody] string maszk)
        {
            var result = new UgyfelResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = UgyfelBll.Read(_context, sid, maszk);

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
        public async Task<Int32Result> Update([FromQuery] string sid, [FromBody] UgyfelDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = UgyfelBll.Update(_context, sid, dto);

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
        public async Task<UgyfelResult> Select([FromQuery] string sid, [FromBody] UgyfelParam par)
        {
            var result = new UgyfelResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = UgyfelBll.Select(_context, sid, par.RekordTol, par.LapMeret, 
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
        public async Task<BaseResults.EmptyResult> ZoomCheck([FromQuery] string sid, [FromBody] UgyfelZoomParameter par)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    UgyfelBll.ZoomCheck(_context, sid, par.Ugyfelkod, par.Ugyfelnev);

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