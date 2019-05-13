namespace ossServer.Controllers.UgyfelterLog
{
    public class UgyfelterLogDto
    {
        public int Ugyfelterlogkod { get; set; }
        public int Ugyfelkod { get; set; }
        public string Ugyfelnev { get; set; }
        public string Ugyfelcim { get; set; }
        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
    }
}
