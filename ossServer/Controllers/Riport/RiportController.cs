using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ossServer.Controllers.Riport
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RiportController : ControllerBase
    {
        private IServiceProvider _services;

        public RiportController(IServiceProvider services)
        {
            _services = services;
        }
    }
}