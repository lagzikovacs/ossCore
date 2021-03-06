﻿using Microsoft.AspNetCore.Mvc;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.UgyfelterLog
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UgyfelterLogController : ControllerBase
    {
        private readonly ossContext _context;

        public UgyfelterLogController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<UgyfelterLogResult> Select([FromQuery] string sid, [FromBody] UgyfelterLogParam par)
        {
            var result = new UgyfelterLogResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    var t = await UgyfelterLogBll.SelectAsync(_context, sid, par.RekordTol, par.LapMeret, 
                        par.Fi);
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
    }
}