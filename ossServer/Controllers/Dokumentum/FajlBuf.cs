namespace ossServer.Controllers.Dokumentum
{
    public class FajlBuf
    {
        public string Fajlnev { get; set; } // feltöltéskor csak
        public int Meret { get; set; } // csak bejegyzéskor használja
        public byte[] b { get; set; } // feltöltéskor ebből veszi a méretet
        public string Ext { get; set; }
        public string Hash { get; set; }
        public int IratKod { get; set; }
        public string Megjegyzes { get; set; }
    }
}
