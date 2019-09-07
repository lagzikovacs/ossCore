using Microsoft.AspNetCore.Mvc;
using ossServer.BaseResults;
using ossServer.Utils;
using System;
using System.Collections.Generic;

namespace ossServer.Controllers.Kapcsolatihalo
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class KapcsolatihaloController : ControllerBase
    {
        [HttpPost]
        public StringResult StartReader([FromServices] KapcsolatihaloTask taskm, [FromQuery] string sid)
        {
            var result = new StringResult();

            try
            {
                taskm.Setup(sid, KapcsolatihaloTaskTypes.Reader);
                taskm.Start();
                result.Result = taskm.tasktoken;
            }
            catch (Exception ex)
            {
                result.Error = ex.InmostMessage();
            }

            return result;
        }

        [HttpPost]
        public StringResult StartWriter([FromServices] KapcsolatihaloTask taskm, [FromQuery] string sid,
            [FromBody] List<KapcsolatihaloPos> pos)
        {
            var result = new StringResult();

            try
            {
                taskm.Setup(sid, KapcsolatihaloTaskTypes.Writer, pos);
                taskm.Start();
                result.Result = taskm.tasktoken;
            }
            catch (Exception ex)
            {
                result.Error = ex.InmostMessage();
            }

            return result;
        }

        // TODO a lekérdezés elkészülte után több hívással kell megszerezni az adatokat
        // a hívások alatt már rajzolhatja a gráfot a böngésző

        [HttpPost]
        public KapcsolatihaloTaskResult TaskCheck([FromQuery] string sid, [FromBody] string taskToken)
        {
            var result = new KapcsolatihaloTaskResult();

            try
            {
                var taskm = KapcsolatihaloTaskManager.Get(taskToken, sid);
                return taskm.Check();
            }
            catch (Exception ex)
            {
                result.Error = ex.InmostMessage();
            }

            return result;
        }

        // TODO Cancel-t is lehessen hívni
    }
}