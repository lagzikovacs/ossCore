using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Penztar
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PenztarController : ControllerBase
    {
        private readonly ossContext _context;

        public PenztarController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<Int32Result> Add([FromQuery] string sid, [FromBody] PenztarDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = PenztarBll.Add(_context, sid, dto);

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
        public async Task<PenztarResult> CreateNew([FromQuery] string sid)
        {
            var result = new PenztarResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<PenztarDto> { PenztarBll.CreateNew(_context, sid) };

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
        public async Task<BaseResults.EmptyResult> Delete([FromQuery] string sid, [FromBody] PenztarDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    PenztarBll.Delete(_context, sid, dto);

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
        public async Task<PenztarResult> Get([FromQuery] string sid, [FromBody] int key)
        {
            var result = new PenztarResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<PenztarDto> { PenztarBll.Get(_context, sid, key) };

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
        public async Task<PenztarResult> Read([FromQuery] string sid, [FromBody] string maszk)
        {
            var result = new PenztarResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = PenztarBll.Read(_context, sid, maszk);

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
        public async Task<PenztarResult> ReadById([FromQuery] string sid, [FromBody] int penztarkod)
        {
            var result = new PenztarResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = PenztarBll.Read(_context, sid, penztarkod);

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
        public async Task<PenztarResult> ReadByCurrencyOpened([FromQuery] string sid, [FromBody] int penznemkod)
        {
            var result = new PenztarResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = PenztarBll.ReadByCurrencyOpened(_context, sid, penznemkod);

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
        public async Task<Int32Result> Update([FromQuery] string sid, [FromBody] PenztarDto dto)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = PenztarBll.Update(_context, sid, dto);

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