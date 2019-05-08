namespace ossServer.Controllers.Esemenynaplo
{
    public enum EsemenynaploBejegyzesek
    {
        Bejelentkezes = EsemenynaploBejegyzesKategoriak.Bejelentkezesek | 0,
        Kijelentkezes = EsemenynaploBejegyzesKategoriak.Bejelentkezesek | 1,
        SzerepkorValasztas = EsemenynaploBejegyzesKategoriak.Bejelentkezesek | 2
    }
}
