using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Menu
{
    public class AngularMenuResult : EmptyResult
    {
        public List<AngularMenuDto> Result { get; set; }
    }
}
