using System;
using System.Collections.Generic;

namespace ossServer.Models
{
    public partial class Particio
    {
        public Particio()
        {
            Afakulcs = new HashSet<Afakulcs>();
            Ajanlatkeres = new HashSet<Ajanlatkeres>();
            BizonylatNavigation = new HashSet<Bizonylat>();
            Bizonylatafa = new HashSet<Bizonylatafa>();
            Bizonylatkapcsolat = new HashSet<Bizonylatkapcsolat>();
            Bizonylattermekdij = new HashSet<Bizonylattermekdij>();
            Bizonylattetel = new HashSet<Bizonylattetel>();
            Cikk = new HashSet<Cikk>();
            Csoport = new HashSet<Csoport>();
            Csoportfelhasznalo = new HashSet<Csoportfelhasznalo>();
            Csoportjog = new HashSet<Csoportjog>();
            Dokumentum = new HashSet<Dokumentum>();
            Esemenynaplo = new HashSet<Esemenynaplo>();
            Fizetesimod = new HashSet<Fizetesimod>();
            Helyseg = new HashSet<Helyseg>();
            Irat = new HashSet<Irat>();
            Irattipus = new HashSet<Irattipus>();
            Kifizetes = new HashSet<Kifizetes>();
            Kodgenerator = new HashSet<Kodgenerator>();
            Mennyisegiegyseg = new HashSet<Mennyisegiegyseg>();
            Penznem = new HashSet<Penznem>();
            Penztar = new HashSet<Penztar>();
            Penztartetel = new HashSet<Penztartetel>();
            ProjektNavigation = new HashSet<Projekt>();
            Projektkapcsolat = new HashSet<Projektkapcsolat>();
            Projektteendo = new HashSet<Projektteendo>();
            Session = new HashSet<Session>();
            Szamlazasirend = new HashSet<Szamlazasirend>();
            Teendo = new HashSet<Teendo>();
            Termekdij = new HashSet<Termekdij>();
            Ugyfel = new HashSet<Ugyfel>();
            Ugyfelterlog = new HashSet<Ugyfelterlog>();
            VolumeNavigation = new HashSet<Volume>();
        }

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
        public bool? SmtpTls { get; set; }
        public string Hibaertesitesemailcimek { get; set; }
        public int? BizonylatBizonylatkepIratkod { get; set; }
        public int? BizonylatEredetipeldanyokSzama { get; set; }
        public int? BizonylatMasolatokSzama { get; set; }
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
        public DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }

        public virtual ICollection<Afakulcs> Afakulcs { get; set; }
        public virtual ICollection<Ajanlatkeres> Ajanlatkeres { get; set; }
        public virtual ICollection<Bizonylat> BizonylatNavigation { get; set; }
        public virtual ICollection<Bizonylatafa> Bizonylatafa { get; set; }
        public virtual ICollection<Bizonylatkapcsolat> Bizonylatkapcsolat { get; set; }
        public virtual ICollection<Bizonylattermekdij> Bizonylattermekdij { get; set; }
        public virtual ICollection<Bizonylattetel> Bizonylattetel { get; set; }
        public virtual ICollection<Cikk> Cikk { get; set; }
        public virtual ICollection<Csoport> Csoport { get; set; }
        public virtual ICollection<Csoportfelhasznalo> Csoportfelhasznalo { get; set; }
        public virtual ICollection<Csoportjog> Csoportjog { get; set; }
        public virtual ICollection<Dokumentum> Dokumentum { get; set; }
        public virtual ICollection<Esemenynaplo> Esemenynaplo { get; set; }
        public virtual ICollection<Fizetesimod> Fizetesimod { get; set; }
        public virtual ICollection<Helyseg> Helyseg { get; set; }
        public virtual ICollection<Irat> Irat { get; set; }
        public virtual ICollection<Irattipus> Irattipus { get; set; }
        public virtual ICollection<Kifizetes> Kifizetes { get; set; }
        public virtual ICollection<Kodgenerator> Kodgenerator { get; set; }
        public virtual ICollection<Mennyisegiegyseg> Mennyisegiegyseg { get; set; }
        public virtual ICollection<Penznem> Penznem { get; set; }
        public virtual ICollection<Penztar> Penztar { get; set; }
        public virtual ICollection<Penztartetel> Penztartetel { get; set; }
        public virtual ICollection<Projekt> ProjektNavigation { get; set; }
        public virtual ICollection<Projektkapcsolat> Projektkapcsolat { get; set; }
        public virtual ICollection<Projektteendo> Projektteendo { get; set; }
        public virtual ICollection<Session> Session { get; set; }
        public virtual ICollection<Szamlazasirend> Szamlazasirend { get; set; }
        public virtual ICollection<Teendo> Teendo { get; set; }
        public virtual ICollection<Termekdij> Termekdij { get; set; }
        public virtual ICollection<Ugyfel> Ugyfel { get; set; }
        public virtual ICollection<Ugyfelterlog> Ugyfelterlog { get; set; }
        public virtual ICollection<Volume> VolumeNavigation { get; set; }
    }
}
