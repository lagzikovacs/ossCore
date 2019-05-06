namespace ossServer.Controllers.Volume
{
    public class VolumeDto
    {
        public int Volumekod { get; set; }
        public int Volumeno { get; set; }
        public string Eleresiut { get; set; }
        public int Maxmeret { get; set; }
        public int Jelenlegimeret { get; set; }
        public int Utolsokonyvtar { get; set; }
        public int Fajlokszamautolsokonyvtarban { get; set; }
        public string Allapot { get; set; }
        public System.DateTime Allapotkelte { get; set; }
    }
}
