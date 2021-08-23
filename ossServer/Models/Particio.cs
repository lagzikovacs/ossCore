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
            Projektjegyzet = new HashSet<Projektjegyzet>();
            Projektkapcsolat = new HashSet<Projektkapcsolat>();
            Session = new HashSet<Session>();
            Szamlazasirend = new HashSet<Szamlazasirend>();
            Termekdij = new HashSet<Termekdij>();
            Tevekenyseg = new HashSet<Tevekenyseg>();
            Ugyfel = new HashSet<Ugyfel>();
            Ugyfelterlog = new HashSet<Ugyfelterlog>();
            VolumeNavigation = new HashSet<Volume>();
        }

        public int Particiokod { get; set; }
        public string Megnevezes { get; set; }
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
        public virtual ICollection<Projektjegyzet> Projektjegyzet { get; set; }
        public virtual ICollection<Projektkapcsolat> Projektkapcsolat { get; set; }
        public virtual ICollection<Session> Session { get; set; }
        public virtual ICollection<Szamlazasirend> Szamlazasirend { get; set; }
        public virtual ICollection<Termekdij> Termekdij { get; set; }
        public virtual ICollection<Tevekenyseg> Tevekenyseg { get; set; }
        public virtual ICollection<Ugyfel> Ugyfel { get; set; }
        public virtual ICollection<Ugyfelterlog> Ugyfelterlog { get; set; }
        public virtual ICollection<Volume> VolumeNavigation { get; set; }
    }
}
