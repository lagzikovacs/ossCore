using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;

namespace ossServer.Controllers.Bizonylat
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BizonylatController : ControllerBase
    {
        private readonly ossContext _context;

        public BizonylatController(ossContext context)
        {
            _context = context;
        }
    }
}