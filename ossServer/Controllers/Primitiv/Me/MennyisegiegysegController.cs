using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;

namespace ossServer.Controllers.Primitiv.Me
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MennyisegiegysegController : ControllerBase
    {
        private readonly ossContext _context;

        public MennyisegiegysegController(ossContext context)
        {
            _context = context;
        }
    }
}