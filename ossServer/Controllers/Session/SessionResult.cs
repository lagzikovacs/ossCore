using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Session
{
    public class SessionResult : EmptyResult
    {
        public List<SessionDto> Result { get; set; }
    }
}
