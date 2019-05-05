using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Primitiv.Irattipus
{
    public class IrattipusResult : EmptyResult
    {
        public List<IrattipusDto> Result { get; set; }
    }
}
