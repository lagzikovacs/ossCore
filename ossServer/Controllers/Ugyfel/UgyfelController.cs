using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ossServer.Models;

namespace ossServer.Controllers.Ugyfel
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UgyfelController : ControllerBase
    {
        private readonly ossContext _context;

        public UgyfelController(ossContext context)
        {
            _context = context;
        }
    }
}