using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Threading.Tasks;

namespace ossServer.Controllers.Verzio
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VerzioController : ControllerBase
    {
        private readonly ossContext _context;

        public VerzioController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<StringResult> VerzioEsBuild([FromQuery] string sid)
        {
            var result = new StringResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = VerzioBll.VerzioEsBuild(_context, sid);

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