using System;

namespace ossServer.Controllers.Bizonylat
{
    public class BizonylatDto
    {
        public int Bizonylatkod { get; set; }
        public int Particiokod { get; set; }
        public int Bizonylattipuskod { get; set; }
        public string Bizonylatszam { get; set; }
        public string Szallitonev { get; set; }
        public string Szallitoiranyitoszam { get; set; }
        public string Szallitohelysegnev { get; set; }
        public string Szallitoutcahazszam { get; set; }
        public string Szallitobankszamla1 { get; set; }
        public string Szallitobankszamla2 { get; set; }
        public string Szallitoadotorzsszam { get; set; }
        public string Szallitoadoafakod { get; set; }
        public string Szallitoadomegyekod { get; set; }
        public int Ugyfelkod { get; set; }
        public string Ugyfelnev { get; set; }
        public string Ugyfeliranyitoszam { get; set; }
        public int Ugyfelhelysegkod { get; set; }
        public string Ugyfelhelysegnev { get; set; }
        public string Ugyfelkozterulet { get; set; }
        public string Ugyfelkozterulettipus { get; set; }
        public string Ugyfelhazszam { get; set; }
        public string Ugyfelcim { get; set; }
        public string Ugyfeladoszam { get; set; }
        public System.DateTime Bizonylatkelte { get; set; }
        public System.DateTime Teljesiteskelte { get; set; }
        public int? Fizetesimodkod { get; set; }
        public string Fizetesimod { get; set; }
        public DateTime Fizetesihatarido { get; set; }
        public DateTime? Szallitasihatarido { get; set; }
        public bool? Kifizetesrendben { get; set; }
        public bool? Kiszallitva { get; set; }
        public string Megjegyzesfej { get; set; }
        public decimal Netto { get; set; }
        public decimal Afa { get; set; }
        public decimal Brutto { get; set; }
        public decimal Termekdij { get; set; }
        public int Penznemkod { get; set; }
        public string Penznem { get; set; }
        public decimal Arfolyam { get; set; }
        public string Azaz { get; set; }
        public int Nyomtatottpeldanyokszama { get; set; }
        public bool Ezstornozo { get; set; }
        public bool Ezstornozott { get; set; }
        public int? Stornozobizonylatkod { get; set; }
        public int? Stornozottbizonylatkod { get; set; }

        public int? Fuvarszamlakod { get; set; }
        public string Fuvarszamla { get; set; }
        public decimal? Fuvardij { get; set; }
        public int? Fuvardijpenznemkod { get; set; }
        public string Fuvardijpenznem { get; set; }
        public decimal? Fuvardijarfolyam { get; set; }

        public System.DateTime Letrehozva { get; set; }
        public string Letrehozta { get; set; }
        public DateTime Modositva { get; set; }
        public string Modositotta { get; set; }
    }
}
