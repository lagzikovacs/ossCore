using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.BizonylatKapcsolat
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BizonylatKapcsolatController : ControllerBase
    {
        private readonly ossContext _context;

        public BizonylatKapcsolatController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<Int32Result> AddIratToBizonylat([FromQuery] string sid, [FromBody] BizonylatKapcsolatParam par)
        {
            var result = new Int32Result();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = BizonylatKapcsolatBll.AddIratToBizonylat(_context, sid, 
                        par.BizonylatKod, par.IratKod);

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
        public async Task<BaseResults.EmptyResult> Delete([FromQuery] string sid, [FromBody] BizonylatKapcsolatDto dto)
        {
            var result = new BaseResults.EmptyResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    BizonylatKapcsolatBll.Delete(_context, sid, dto);

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
        public async Task<BizonylatKapcsolatResult> Get([FromQuery] string sid, [FromBody] int bizonylatkapcsolatKod)
        {
            var result = new BizonylatKapcsolatResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<BizonylatKapcsolatDto>
                    {
                        BizonylatKapcsolatBll.Get(_context, sid, bizonylatkapcsolatKod)
                    };

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
        public async Task<BizonylatKapcsolatResult> Select([FromQuery] string sid, [FromBody] int bizonylatKod)
        {
            var result = new BizonylatKapcsolatResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = BizonylatKapcsolatBll.Select(_context, sid, bizonylatKod);

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
