using ossServer.BaseResults;
using ossServer.Controllers.Ugyfel;
using ossServer.Controllers.Ugyfelkapcsolat;
using System.Collections.Generic;

namespace ossServer.Controllers.Kapcsolatihalo
{
    public class KapcsolatihaloTaskResult : EmptyResult
    {
        public KapcsolatihaloTaskStates status = KapcsolatihaloTaskStates.Queued;

        public List<UgyfelkapcsolatDto> lstUgyfelkapcsolatDto;
        public List<UgyfelDto> lstUgyfelDto;
    }
}
