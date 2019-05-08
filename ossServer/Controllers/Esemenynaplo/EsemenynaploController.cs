using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;
using ossServer.Utils;

namespace ossServer.Controllers.Esemenynaplo
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EsemenynaploController : ControllerBase
    {
        private readonly ossContext _context;

        public EsemenynaploController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<EsemenynaploResult> Select([FromQuery] string sid, [FromBody] EsemenynaploParam par)
        {
            var result = new EsemenynaploResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = EsemenynaploBll.Select(_context, sid, par.RekordTol, par.LapMeret, 
                        par.Felhasznalokod, out var osszesRekord);
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
    }
}