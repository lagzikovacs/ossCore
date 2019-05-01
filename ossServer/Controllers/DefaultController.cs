using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ossServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [HttpPost]
        public async Task<StringResult> Test([FromQuery] string sid, [FromBody] string par)
        {
            var result = new StringResult();

            try
            {
                if (sid == null)
                    throw new ArgumentNullException(nameof(sid));

                result.Result = sid + "-" + par;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }

        // GET: api/Default/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Default
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Default/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class EmptyResult
    {
        public string Error { get; set; }
    }
    public class StringResult : EmptyResult
    {
        public string Result { get; set; }
    }
}
