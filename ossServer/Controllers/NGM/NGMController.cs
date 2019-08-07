using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;

namespace ossServer.Controllers.NGM
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NGMController : ControllerBase
    {
        private readonly ossContext _context;

        public NGMController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<StringResult> Adatszolgaltatas([FromQuery] string sid, [FromBody] NGMParam par)
        {
            var result = new StringResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = await NgmBll.AdatszolgaltatasAsync(_context, sid,
                        par.Mode, par.SzamlaKelteTol, par.SzamlaKelteIg, par.SzamlaSzamTol, par.SzamlaSzamIg);

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