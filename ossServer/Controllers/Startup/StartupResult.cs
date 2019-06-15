using ossServer.BaseResults;
using ossServer.Utils;
using System.Collections.Generic;

namespace ossServer.Controllers.Startup
{
    public class StartupResult : EmptyResult
    {
        public List<ColumnSettings> Afakulcs_Grid { get; set; }
        public List<ColumnSettings> Afakulcs_Reszletek { get; set; }
        public List<ColumnSettings> Felhasznalo_Grid { get; set; }
        public List<ColumnSettings> Felhasznalo_Reszletek { get; set; }
        public List<ColumnSettings> Fizetesimod_Grid { get; set; }
        public List<ColumnSettings> Fizetesimod_Reszletek { get; set; }
        public List<ColumnSettings> Helyseg_Grid { get; set; }
        public List<ColumnSettings> Helyseg_Reszletek { get; set; }
        public List<ColumnSettings> Irattipus_Grid { get; set; }
        public List<ColumnSettings> Irattipus_Reszletek { get; set; }
        public List<ColumnSettings> Me_Grid { get; set; }
        public List<ColumnSettings> Me_Reszletek { get; set; }
        public List<ColumnSettings> Penznem_Grid { get; set; }
        public List<ColumnSettings> Penznem_Reszletek { get; set; }
        public List<ColumnSettings> Teendo_Grid { get; set; }
        public List<ColumnSettings> Teendo_Reszletek { get; set; }
        public List<ColumnSettings> Termekdij_Grid { get; set; }
        public List<ColumnSettings> Termekdij_Reszletek { get; set; }

        public List<ColumnSettings> Cikk_Grid { get; set; }
        public List<ColumnSettings> Cikk_Reszletek { get; set; }
        public List<ColumnSettings> Ugyfel_Grid { get; set; }
        public List<ColumnSettings> Ugyfel_Reszletek { get; set; }

        public List<ColumnSettings> Projekt_Grid { get; set; }
        public List<ColumnSettings> Projekt_Reszletek { get; set; }
        public List<ColumnSettings> Projektteendo_Grid { get; set; }
        public List<ColumnSettings> Projektteendo_Reszletek { get; set; }
        public List<ColumnSettings> Szamlazasirend_Grid { get; set; }
        public List<ColumnSettings> Szamlazasirend_Reszletek { get; set; }

        public List<ColumnSettings> Irat_Grid { get; set; }
        public List<ColumnSettings> Irat_Reszletek { get; set; }
    }
}
