namespace ossServer.Controllers.Particio
{
    public class EmailConf
    {
        public string ConfName { get; set; }

        public string Tipus { get; set; }
        public string Azonosito { get; set; }
        public string Jelszo { get; set; }
        public string KuldoNeve { get; set; }
        public string KuldoEmailcime { get; set; }
        public bool Ssl { get; set; }
        public string CustomHost { get; set; }
        public int? CustomPort { get; set; }
    }
}
