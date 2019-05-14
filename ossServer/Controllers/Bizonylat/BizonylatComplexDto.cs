using System.Collections.Generic;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatComplexDto
    {
        public BizonylatDto Dto { get; set; }
        public List<BizonylatTetelDto> LstTetelDto { get; set; }
        public List<BizonylatAfaDto> LstAfaDto { get; set; }
        public List<BizonylatTermekdijDto> LstTermekdijDto { get; set; }
    }
}
