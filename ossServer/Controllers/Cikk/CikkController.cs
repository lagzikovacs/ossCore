using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;

namespace ossServer.Controllers.Cikk
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CikkController : ControllerBase
    {
        private readonly ossContext _context;

        public CikkController(ossContext context)
        {
            _context = context;
        }
    }
}