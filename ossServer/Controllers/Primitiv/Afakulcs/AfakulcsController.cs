using Microsoft.AspNetCore.Mvc;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.Primitiv.Afakulcs
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AfakulcsController : ControllerBase
    {
        private readonly ossContext _context;

        public AfakulcsController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<AfaKulcsResult> Read([FromQuery] string sid, [FromBody] string maszk)
        {
            var result = new AfaKulcsResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = AfakulcsBll.Read(_context, sid, maszk);

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