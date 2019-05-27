using System.Collections.Generic;

namespace ossServer.Utils
{
    public class ColumnSettingsUtil
    {
        public static List<ColumnSettings> AddIdobelyeg(List<ColumnSettings> par)
        {
            if (par == null)
                throw new System.ArgumentNullException();

            par.Add(new ColumnSettings { Name = "Letrehozta", Title = "Létrehozta", Type = ColumnType.STRING });
            par.Add(new ColumnSettings { Name = "Letrehozva", Title = "Létrehozva", Type = ColumnType.DATETIME });
            par.Add(new ColumnSettings { Name = "Modositotta", Title = "Módosította", Type = ColumnType.STRING });
            par.Add(new ColumnSettings { Name = "Modositva", Title = "Módosítva", Type = ColumnType.DATETIME });

            return par;
        }
    }
}
