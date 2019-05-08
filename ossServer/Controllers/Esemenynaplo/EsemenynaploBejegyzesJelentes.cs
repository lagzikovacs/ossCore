namespace ossServer.Controllers.Esemenynaplo
{
    public class EseBeJel
    {
        public EsemenynaploBejegyzesek Bejegyzes { get; set; }
        public string Jelentes { get; set; }

        public EseBeJel(EsemenynaploBejegyzesek bejegyzes, string jelentes)
        {
            Bejegyzes = bejegyzes;
            Jelentes = jelentes;
        }
    }
}
