using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;

namespace ossServer.Controllers.Kifizetes
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class KifizetesController : ControllerBase
    {
        private readonly ossContext _context;

        public KifizetesController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<Int32Result> Add([FromQuery] string sid, [FromBody] KifizetesDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = KifizetesBll.Add(_context, sid, dto);

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
        public async Task<KifizetesResult> CreateNew([FromQuery] string sid)
        {
            var result = new KifizetesResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<KifizetesDto> { KifizetesBll.CreateNew(_context, sid) };

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
            [FromBody] KifizetesDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    KifizetesBll.Delete(_context, sid, dto);

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
        public async Task<KifizetesResult> Get([FromQuery] string sid, [FromBody] int key)
        {
            var result = new KifizetesResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<KifizetesDto> { KifizetesBll.Get(_context, sid, key) };

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
        public async Task<Int32Result> Update([FromQuery] string sid, [FromBody] KifizetesDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = KifizetesBll.Update(_context, sid, dto);

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
        public async Task<KifizetesResult> Select([FromQuery] string sid, [FromBody] int bizonylatKod)
        {
            var result = new KifizetesResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = KifizetesBll.Select(_context, sid, bizonylatKod);

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
                    result.Result = KifizetesBll.GridSettings(_context, sid);

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
                    result.Result = KifizetesBll.ReszletekSettings(_context, sid);

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