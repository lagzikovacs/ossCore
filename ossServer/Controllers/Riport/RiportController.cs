using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Models;
using ossServer.Tasks;
using ossServer.Utils;
using System;
using System.Collections.Generic;

namespace ossServer.Controllers.Riport
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RiportController : ControllerBase
    {
        [HttpPost]
        public RiportResult TaskCheck([FromQuery] string sid, [FromBody] string taskToken)
        {
            var result = new RiportResult();

            try
            {
                var taskm = ServerTaskManager.Get(taskToken, sid);
                var taskresult = taskm.Check();

                result.Status = taskresult.Status;
                result.Error = taskresult.Error;

                if (taskresult.Status == ServerTaskStates.Completed)
                    result.Riport = taskresult.Result;
            }
            catch (Exception ex)
            {
                result.Error = ex.InmostMessage();
            }

            return result;
        }

        [HttpPost]
        public BaseResults.EmptyResult TaskCancel([FromQuery] string sid, 
            [FromBody] string taskToken)
        {
            var result = new BaseResults.EmptyResult();

            try
            {
                var taskm = ServerTaskManager.Get(taskToken, sid);
                taskm.Cancel();
            }
            catch (Exception ex)
            {
                result.Error = ex.InmostMessage();
            }

            return result;
        }

        [HttpPost]
        public StringResult KimenoSzamlaTaskStart([FromServices] KimenoSzamlaTask taskm, 
            [FromQuery] string sid, [FromBody] List<SzMT> fi)
        {
            var result = new StringResult();

            try
            {
                taskm.Setup(sid, fi);
                taskm.Start();
                result.Result = taskm._tasktoken;
            }
            catch (Exception ex)
            {
                result.Error = ex.InmostMessage();
            }

            return result;
        }

        [HttpPost]
        public StringResult BejovoSzamlaTaskStart([FromServices] BejovoSzamlaTask taskm,
            [FromQuery] string sid, [FromBody] List<SzMT> fi)
        {
            var result = new StringResult();

            try
            {
                taskm.Setup(sid, fi);
                taskm.Start();
                result.Result = taskm._tasktoken;
            }
            catch (Exception ex)
            {
                result.Error = ex.InmostMessage();
            }

            return result;
        }

        [HttpPost]
        public StringResult KovetelesekTaskStart([FromServices] KovetelesekTask taskm,
            [FromQuery] string sid, [FromBody] List<SzMT> fi)
        {
            var result = new StringResult();

            try
            {
                taskm.Setup(sid, fi);
                taskm.Start();
                result.Result = taskm._tasktoken;
            }
            catch (Exception ex)
            {
                result.Error = ex.InmostMessage();
            }

            return result;
        }

        [HttpPost]
        public StringResult TartozasokTaskStart([FromServices] TartozasokTask taskm,
            [FromQuery] string sid, [FromBody] List<SzMT> fi)
        {
            var result = new StringResult();

            try
            {
                taskm.Setup(sid, fi);
                taskm.Start();
                result.Result = taskm._tasktoken;
            }
            catch (Exception ex)
            {
                result.Error = ex.InmostMessage();
            }

            return result;
        }

        [HttpPost]
        public StringResult BeszerzesTaskStart([FromServices] BeszerzesTask taskm,
            [FromQuery] string sid, [FromBody] List<SzMT> fi)
        {
            var result = new StringResult();

            try
            {
                taskm.Setup(sid, fi);
                taskm.Start();
                result.Result = taskm._tasktoken;
            }
            catch (Exception ex)
            {
                result.Error = ex.InmostMessage();
            }

            return result;
        }

        [HttpPost]
        public StringResult KeszletTaskStart([FromServices] KeszletTask taskm,
            [FromQuery] string sid, [FromBody] List<SzMT> fi)
        {
            var result = new StringResult();

            try
            {
                taskm.Setup(sid, fi);
                taskm.Start();
                result.Result = taskm._tasktoken;
            }
            catch (Exception ex)
            {
                result.Error = ex.InmostMessage();
            }

            return result;
        }

        [HttpPost]
        public StringResult PenztarTetelTaskStart([FromServices] PenztarTetelTask taskm,
            [FromQuery] string sid, [FromBody] List<SzMT> fi)
        {
            var result = new StringResult();

            try
            {
                taskm.Setup(sid, fi);
                taskm.Start();
                result.Result = taskm._tasktoken;
            }
            catch (Exception ex)
            {
                result.Error = ex.InmostMessage();
            }

            return result;
        }

        [HttpPost]
        public StringResult ProjektTaskStart([FromServices] ProjektTask taskm,
            [FromQuery] string sid, [FromBody] List<SzMT> fi)
        {
            var result = new StringResult();

            try
            {
                taskm.Setup(sid, fi);
                taskm.Start();
                result.Result = taskm._tasktoken;
            }
            catch (Exception ex)
            {
                result.Error = ex.InmostMessage();
            }

            return result;
        }
    }
}