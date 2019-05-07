namespace ossServer.Controllers.Ugyfel
{
    public class UgyfelUtils
    {
        public static string Cim(Models.Ugyfel entity)
        {
            var result = "";
            if (entity.HelysegkodNavigation != null)
                result = $"{entity.Iranyitoszam} {entity.HelysegkodNavigation.Helysegnev}, {entity.Kozterulet} {entity.Kozterulettipus} {entity.Hazszam}";

            return result;
        }
    }
}
