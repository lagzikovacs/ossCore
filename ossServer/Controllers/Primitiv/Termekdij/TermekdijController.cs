using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;

namespace ossServer.Controllers.Primitiv.Termekdij
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TermekdijController : ControllerBase
    {
        private readonly ossContext _context;

        public TermekdijController(ossContext context)
        {
            _context = context;
        }
    }
}