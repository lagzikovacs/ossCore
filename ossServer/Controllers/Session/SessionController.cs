using Microsoft.AspNetCore.Mvc;
using ossServer.Models;
using ossServer.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ossServer.Controllers.Session
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ossContext _context;

        public SessionController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<SessionResult> Get([FromQuery] string sid)
        {
            var result = new SessionResult();

            using (var tr = await _context.Database.BeginTransactionAsync())
                try
                {
                    result.Result = new List<SessionDto> { SessionBll.Get(_context, sid) };

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