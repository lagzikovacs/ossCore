namespace ossServer.Utils
{
  public class Messages
  {
    public static string AdatNemTalalhato = "A(z) '{0}' adat nem található!";
    public static string AdatMegvaltozottNemLehetModositani = "A művelet nem végezhető el, időközben egy másik felhasználó megváltoztatta a tárolt adatokat! Frissítés után próbálja újra!";
    public static string NemTorolhetoReferenciakMiatt = "A törlés nem végezhető el, a törölni kívánt tételre az alábbi tételek hivatkoznak:";
    public static string MarLetezoTetel = "A rögzíteni kívánt tétel - {0} - már létezik az adatbázisban!";
    public static string NemMenthetoMarLetezik = "A módosítás nem végezhető el, ilyen tétel - {0} - már létezik az adatbázisban!";

    public static string TaszkNemindithato = "A taszk nem indítható!";
    public static string TaszkHozzadasaNemsikerult = "A taszk hozzáadása nem sikerült!";
    public static string TaszkNemTalalhatoVagyLejart = "A taszk nem található vagy lejárt!";
    public static string TaszkNemferhetHozza = "Nem férhet hozzá a taszkhoz!";

    public static string HibasZoom = "Hibás zoom, nincs ilyen {0}!";
    public static string ParticioHiba = "Partició hiba, nincs kitöltve vagy hibás: {0}!";
  }
}