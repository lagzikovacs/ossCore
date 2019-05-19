using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.Onlineszamla
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OnlineszamlaController : ControllerBase
    {
        private readonly ossContext _context;

        public OnlineszamlaController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<OnlineszamlaResult> Select([FromQuery] string sid, 
            [FromBody] OnlineszamlaParam par)
        {
            var result = new OnlineszamlaResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = OnlineszamlaBll.Select(_context, sid, 
                        par.RekordTol, par.LapMeret, par.Fi, out var osszesRekord);
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
        public StringResult Adoszamellenorzes([FromQuery] string sid, 
            [FromBody] string adoszam)
        {
            return new StringResult { Error = "Nincs megvalósítva" };
        }

        [HttpPost]
        public StringResult Szamlalekerdezes([FromQuery] string sid, 
            [FromBody] string szamlaszam)
        {
            return new StringResult { Error = "Nincs megvalósítva" };
        }
    }
}