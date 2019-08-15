namespace ossServer.Controllers.Particio
{
    public class ParticioDto
    {
        public int Particiokod { get; set; }
        public string Megnevezes { get; set; }

        public string SzallitoNev { get; set; }
        public string SzallitoIranyitoszam { get; set; }
        public string SzallitoHelysegnev { get; set; }
        public string SzallitoUtcahazszam { get; set; }
        public string SzallitoBankszamla1 { get; set; }
        public string SzallitoBankszamla2 { get; set; }
        public string SzallitoAdotorzsszam { get; set; }
        public string SzallitoAdoafakod { get; set; }
        public string SzallitoAdomegyekod { get; set; }

        public string NavUrl { get; set; }
        public string NavFelhasznaloazonosito { get; set; }
        public string NavFelhasznalojelszo { get; set; }
        public string NavAlairokulcs { get; set; }
        public string NavCserekulcs { get; set; }


        public string SmtpKlienstipus { get; set; }
        public string SmtpFelhasznaloazonosito { get; set; }
        public string SmtpFelhasznalojelszo { get; set; }
        public string SmtpKuldoneve { get; set; }
        public string SmtpKuldoemailcime { get; set; }
        public string SmtpCustomhost { get; set; }
        public int? SmtpCustomport { get; set; }
        public bool SmtpTls { get; set; }
        public string Hibaertesitesemailcimek { get; set; }

        //public int? BizonylatBizonylatkepIratkod { get; set; }
        //public int? BizonylatEredetipeldanyokSzama { get; set; }
        //public int? BizonylatMasolatokSzama { get; set; }

        public int? ProjektAjanlatIratkod { get; set; }
        public int? ProjektElegedettsegifelmeresIratkod { get; set; }
        public int? ProjektKeszrejelentesDemaszIratkod { get; set; }
        public int? ProjektKeszrejelentesElmuemaszIratkod { get; set; }
        public int? ProjektKeszrejelentesEonIratkod { get; set; }
        public int? ProjektMunkalapIratkod { get; set; }
        public int? ProjektSzallitasiszerzodesIratkod { get; set; }
        public int? ProjektSzerzodesIratkod { get; set; }
        public int? ProjektFeltetelesszerzodesIratkod { get; set; }
        public int? VolumeUjvolumeMaxmeret { get; set; }
        public string VolumeUjvolumeEleresiut { get; set; }

        public string Szallito { get; set; }
        public string Navonlineszamla { get; set; }
        public string Bizonylat { get; set; }
        public string Projekt { get; set; }
        public string Volume { get; set; }
        public string Emails { get; set; }

        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public System.DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
