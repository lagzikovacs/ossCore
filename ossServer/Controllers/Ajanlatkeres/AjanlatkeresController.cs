using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
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

        public AjanlatkeresController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<BaseResults.EmptyResult> WebesAjanlatkeres([FromBody]WebesAjanlatkeresParam par)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    AjanlatkeresBll.WebesAjanlatkeres(_context, par);

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
                    result.Result = AjanlatkeresBll.Add(_context, sid, dto);

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
                    result.Result = new List<AjanlatkeresDto> { AjanlatkeresBll.CreateNew(_context, sid) };

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
                    AjanlatkeresBll.Delete(_context, sid, dto);

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
                    result.Result = new List<AjanlatkeresDto> { AjanlatkeresBll.Get(_context, sid, key) };

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
                    result.Result = AjanlatkeresBll.Select(_context, sid, par.RekordTol, par.LapMeret, 
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
        public async Task<Int32Result> Update([FromQuery] string sid, [FromBody] AjanlatkeresDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = AjanlatkeresBll.Update(_context, sid, dto);

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