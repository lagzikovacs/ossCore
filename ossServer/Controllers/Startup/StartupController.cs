using Microsoft.AspNetCore.Mvc;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.Startup
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StartupController : ControllerBase
    {
        private readonly ossContext _context;

        public StartupController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<StartupResult> Get([FromQuery] string sid)
        {
            var result = new StartupResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result = StartupBll.Get(_context, sid);

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