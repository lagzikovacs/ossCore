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
        public async Task<AfaKulcsResult> Read([FromQuery] string sid, [FromBody] string maszk)
        {
            return await new AfakulcsBll(_context, sid).Read(maszk);
        }
    }
}