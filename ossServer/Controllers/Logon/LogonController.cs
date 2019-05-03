using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;

namespace ossServer.Controllers.Logon
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LogonController : ControllerBase
    {
        private readonly ossContext _context;

        public LogonController(ossContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<BaseResults.EmptyResult> Kijelentkezes([FromQuery] string sid)
        {
            return await LogonBll.Kijelentkezes(_context, sid);
        }
    }
}