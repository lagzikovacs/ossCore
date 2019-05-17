using System;

namespace ossServer.Controllers.NGM
{
    public class NGMParam
    {
        public NGMMode Mode { get; set; }
        public DateTime SzamlaKelteTol { get; set; }
        public DateTime SzamlaKelteIg { get; set; }
        public string SzamlaSzamTol { get; set; }
        public string SzamlaSzamIg { get; set; }
    }
}
