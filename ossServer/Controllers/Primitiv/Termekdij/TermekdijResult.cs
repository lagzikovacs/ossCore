using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Termekdij
{
    public class TermekdijResult : EmptyResult
    {
        public List<TermekdijDto> Result { get; set; }
    }
}
