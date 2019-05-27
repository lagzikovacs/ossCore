using ossServer.BaseResults;
using System.Collections.Generic;

namespace ossServer.Utils
{
    public class ColumnSettingsResult : EmptyResult
    {
        public List<ColumnSettings> Result { get; set; }
    }
}
