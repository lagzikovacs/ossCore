using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Controllers.Volume
{
    public class VolumeResult : EmptyResult
    {
        public List<VolumeDto> Result { get; set; }
    }
}
