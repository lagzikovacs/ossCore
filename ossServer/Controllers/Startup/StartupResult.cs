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
        public List<ColumnSettings> BeszerzesKivet_Grid { get; set; }
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

        public List<ColumnSettings> Csoport_Grid { get; set; }
        public List<ColumnSettings> Csoport_Reszletek { get; set; }

        public List<ColumnSettings> Ajanlatkeres_Grid { get; set; }
        public List<ColumnSettings> Ajanlatkeres_Reszletek { get; set; }

        public List<ColumnSettings> Penztar_Grid { get; set; }
        public List<ColumnSettings> Penztar_Reszletek { get; set; }
        public List<ColumnSettings> Penztartetel_Grid { get; set; }
        public List<ColumnSettings> Penztartetel_Reszletek { get; set; }

        public List<ColumnSettings> Kifizetes_Grid { get; set; }
        public List<ColumnSettings> Kifizetes_Reszletek { get; set; }
        public List<ColumnSettings> Dokumentum_Grid { get; set; }
        public List<ColumnSettings> Dokumentum_Reszletek { get; set; }
        public List<ColumnSettings> Volume_Grid { get; set; }
        public List<ColumnSettings> Volume_Reszletek { get; set; }
        public List<ColumnSettings> Ugyfelterlog_Grid { get; set; }
        public List<ColumnSettings> Ugyfelterlog_Reszletek { get; set; }
    }
}
