﻿using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.UgyfelterLog
{
    public class UgyfelterLogParam
    {
        public int RekordTol { get; set; }
        public int LapMeret { get; set; }
        public List<SzMT> Fi { get; set; }
    }
}
