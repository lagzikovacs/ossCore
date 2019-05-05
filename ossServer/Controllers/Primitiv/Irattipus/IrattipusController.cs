using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;

namespace ossServer.Controllers.Primitiv.Irattipus
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IrattipusController : ControllerBase
    {
        private readonly ossContext _context;

        public IrattipusController(ossContext context)
        {
            _context = context;
        }
    }
}