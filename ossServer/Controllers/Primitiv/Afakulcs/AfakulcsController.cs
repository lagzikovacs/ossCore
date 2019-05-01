using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;

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
        public async Task<StringResult> Read([FromQuery] string sid, [FromBody] string maszk)
        {
            var result = new StringResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                //    if (sid == null)
                //        throw new ArgumentNullException(nameof(sid));

                    result.Result = sid + "-" + maszk;

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr.Rollback();
                    result.Error = ex.Message;
                }

            return result;
        }
    }
}