using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ossServer.Models
{
    public partial class ossContext : DbContext
    {
        public ossContext()
        {
        }

        public ossContext(DbContextOptions<ossContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Afakulcs> Afakulcs { get; set; }
        public virtual DbSet<Ajanlatkeres> Ajanlatkeres { get; set; }
        public virtual DbSet<Bizonylat> Bizonylat { get; set; }
        public virtual DbSet<Bizonylatafa> Bizonylatafa { get; set; }
        public virtual DbSet<Bizonylatkapcsolat> Bizonylatkapcsolat { get; set; }
        public virtual DbSet<Bizonylattermekdij> Bizonylattermekdij { get; set; }
        public virtual DbSet<Bizonylattetel> Bizonylattetel { get; set; }
        public virtual DbSet<Cikk> Cikk { get; set; }
        public virtual DbSet<Csoport> Csoport { get; set; }
        public virtual DbSet<Csoportfelhasznalo> Csoportfelhasznalo { get; set; }
        public virtual DbSet<Csoportjog> Csoportjog { get; set; }
        public virtual DbSet<Dokumentum> Dokumentum { get; set; }
        public virtual DbSet<Esemenynaplo> Esemenynaplo { get; set; }
        public virtual DbSet<Felhasznalo> Felhasznalo { get; set; }
        public virtual DbSet<Fizetesimod> Fizetesimod { get; set; }
        public virtual DbSet<Helyseg> Helyseg { get; set; }
        public virtual DbSet<Irat> Irat { get; set; }
        public virtual DbSet<Irattipus> Irattipus { get; set; }
        public virtual DbSet<Kifizetes> Kifizetes { get; set; }
        public virtual DbSet<Kodgenerator> Kodgenerator { get; set; }
        public virtual DbSet<Lehetsegesjog> Lehetsegesjog { get; set; }
        public virtual DbSet<Mennyisegiegyseg> Mennyisegiegyseg { get; set; }
        public virtual DbSet<Navfeltoltes> Navfeltoltes { get; set; }
        public virtual DbSet<Particio> Particio { get; set; }
        public virtual DbSet<Penznem> Penznem { get; set; }
        public virtual DbSet<Penztar> Penztar { get; set; }
        public virtual DbSet<Penztartetel> Penztartetel { get; set; }
        public virtual DbSet<Projekt> Projekt { get; set; }
        public virtual DbSet<Projektkapcsolat> Projektkapcsolat { get; set; }
        public virtual DbSet<Projektteendo> Projektteendo { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<Szamlazasirend> Szamlazasirend { get; set; }
        public virtual DbSet<Teendo> Teendo { get; set; }
        public virtual DbSet<Termekdij> Termekdij { get; set; }
        public virtual DbSet<Ugyfel> Ugyfel { get; set; }
        public virtual DbSet<Ugyfelterlog> Ugyfelterlog { get; set; }
        public virtual DbSet<Verzio> Verzio { get; set; }
        public virtual DbSet<Volume> Volume { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=94.199.180.128, 50000;Database=oss;User ID=gsg;Password=san1man0;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Afakulcs>(entity =>
            {
                entity.HasKey(e => e.Afakulcskod);

                entity.ToTable("AFAKULCS");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => new { e.Particiokod, e.Afakulcs1 })
                    .HasName("IX_AFAKULCS_AFAKULCS")
                    .IsUnique();

                entity.Property(e => e.Afakulcskod).HasColumnName("AFAKULCSKOD");

                entity.Property(e => e.Afakulcs1)
                    .IsRequired()
                    .HasColumnName("AFAKULCS")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Afamerteke)
                    .HasColumnName("AFAMERTEKE")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Afakulcs)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AFAKULCS_PARTICIO");
            });

            modelBuilder.Entity<Ajanlatkeres>(entity =>
            {
                entity.HasKey(e => e.Ajanlatkereskod)
                    .HasName("PK_FELIRATKOZAS");

                entity.ToTable("AJANLATKERES");

                entity.HasIndex(e => e.Cim)
                    .HasName("IX_FELIRATKOZAS_TELEPULES");

                entity.HasIndex(e => e.Email)
                    .HasName("IX_FELIRATKOZAS_EMAIL");

                entity.HasIndex(e => e.Nev)
                    .HasName("IX_FELIRATKOZAS_NEV");

                entity.HasIndex(e => e.Particiokod)
                    .HasName("IX_FELIRATKOZAS_PARTICIOKOD");

                entity.HasIndex(e => e.Telefonszam)
                    .HasName("IX_FELIRATKOZAS_TELEFONSZAM");

                entity.Property(e => e.Ajanlatkereskod).HasColumnName("AJANLATKERESKOD");

                entity.Property(e => e.Cim)
                    .HasColumnName("CIM")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Havifogyasztaskwh)
                    .HasColumnName("HAVIFOGYASZTASKWH")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Haviszamlaft)
                    .HasColumnName("HAVISZAMLAFT")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Megjegyzes)
                    .HasColumnName("MEGJEGYZES")
                    .HasColumnType("text");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Napelemekteljesitmenyekw)
                    .HasColumnName("NAPELEMEKTELJESITMENYEKW")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Nev)
                    .HasColumnName("NEV")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Telefonszam)
                    .HasColumnName("TELEFONSZAM")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ugynoknev)
                    .IsRequired()
                    .HasColumnName("UGYNOKNEV")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Ajanlatkeres)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FELIRATKOZAS_PARTICIO");
            });

            modelBuilder.Entity<Bizonylat>(entity =>
            {
                entity.HasKey(e => e.Bizonylatkod);

                entity.ToTable("BIZONYLAT");

                entity.HasIndex(e => e.Bizonylatszam);

                entity.HasIndex(e => e.Fizetesimodkod);

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Penznemkod);

                entity.HasIndex(e => e.Ugyfelhelysegkod);

                entity.HasIndex(e => e.Ugyfelkod);

                entity.HasIndex(e => e.Ugyfelnev);

                entity.Property(e => e.Bizonylatkod).HasColumnName("BIZONYLATKOD");

                entity.Property(e => e.Afa)
                    .HasColumnName("AFA")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Arfolyam)
                    .HasColumnName("ARFOLYAM")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Azaz)
                    .HasColumnName("AZAZ")
                    .HasColumnType("text");

                entity.Property(e => e.Bizonylatkelte)
                    .HasColumnName("BIZONYLATKELTE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Bizonylatszam)
                    .HasColumnName("BIZONYLATSZAM")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Bizonylattipuskod).HasColumnName("BIZONYLATTIPUSKOD");

                entity.Property(e => e.Brutto)
                    .HasColumnName("BRUTTO")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Ezstornozo).HasColumnName("EZSTORNOZO");

                entity.Property(e => e.Ezstornozott).HasColumnName("EZSTORNOZOTT");

                entity.Property(e => e.Fizetesihatarido)
                    .HasColumnName("FIZETESIHATARIDO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fizetesimod)
                    .HasColumnName("FIZETESIMOD")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Fizetesimodkod).HasColumnName("FIZETESIMODKOD");

                entity.Property(e => e.Kifizetesrendben).HasColumnName("KIFIZETESRENDBEN");

                entity.Property(e => e.Kiszallitva).HasColumnName("KISZALLITVA");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Megjegyzesfej)
                    .HasColumnName("MEGJEGYZESFEJ")
                    .HasColumnType("text");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Netto)
                    .HasColumnName("NETTO")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Nyomtatottpeldanyokszama).HasColumnName("NYOMTATOTTPELDANYOKSZAMA");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Penznem)
                    .IsRequired()
                    .HasColumnName("PENZNEM")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Penznemkod).HasColumnName("PENZNEMKOD");

                entity.Property(e => e.Stornozobizonylatkod).HasColumnName("STORNOZOBIZONYLATKOD");

                entity.Property(e => e.Stornozottbizonylatkod).HasColumnName("STORNOZOTTBIZONYLATKOD");

                entity.Property(e => e.Szallitasihatarido)
                    .HasColumnName("SZALLITASIHATARIDO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Szallitoadoafakod)
                    .IsRequired()
                    .HasColumnName("SZALLITOADOAFAKOD")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Szallitoadomegyekod)
                    .IsRequired()
                    .HasColumnName("SZALLITOADOMEGYEKOD")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.Szallitoadotorzsszam)
                    .IsRequired()
                    .HasColumnName("SZALLITOADOTORZSSZAM")
                    .HasMaxLength(8)
                    .IsUnicode(false);

                entity.Property(e => e.Szallitobankszamla1)
                    .HasColumnName("SZALLITOBANKSZAMLA1")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Szallitobankszamla2)
                    .HasColumnName("SZALLITOBANKSZAMLA2")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Szallitohelysegnev)
                    .IsRequired()
                    .HasColumnName("SZALLITOHELYSEGNEV")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Szallitoiranyitoszam)
                    .IsRequired()
                    .HasColumnName("SZALLITOIRANYITOSZAM")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Szallitonev)
                    .IsRequired()
                    .HasColumnName("SZALLITONEV")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Szallitoutcahazszam)
                    .IsRequired()
                    .HasColumnName("SZALLITOUTCAHAZSZAM")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Teljesiteskelte)
                    .HasColumnName("TELJESITESKELTE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Termekdij)
                    .HasColumnName("TERMEKDIJ")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Ugyfeladoszam)
                    .HasColumnName("UGYFELADOSZAM")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ugyfelhazszam)
                    .HasColumnName("UGYFELHAZSZAM")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Ugyfelhelysegkod).HasColumnName("UGYFELHELYSEGKOD");

                entity.Property(e => e.Ugyfelhelysegnev)
                    .HasColumnName("UGYFELHELYSEGNEV")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ugyfeliranyitoszam)
                    .HasColumnName("UGYFELIRANYITOSZAM")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ugyfelkod).HasColumnName("UGYFELKOD");

                entity.Property(e => e.Ugyfelkozterulet)
                    .HasColumnName("UGYFELKOZTERULET")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ugyfelkozterulettipus)
                    .HasColumnName("UGYFELKOZTERULETTIPUS")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ugyfelnev)
                    .IsRequired()
                    .HasColumnName("UGYFELNEV")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.FizetesimodkodNavigation)
                    .WithMany(p => p.Bizonylat)
                    .HasForeignKey(d => d.Fizetesimodkod)
                    .HasConstraintName("FK_BIZONYLAT_FIZETESIMOD");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.BizonylatNavigation)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLAT_PARTICIO");

                entity.HasOne(d => d.PenznemkodNavigation)
                    .WithMany(p => p.Bizonylat)
                    .HasForeignKey(d => d.Penznemkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLAT_PENZNEM");

                entity.HasOne(d => d.StornozobizonylatkodNavigation)
                    .WithMany(p => p.InverseStornozobizonylatkodNavigation)
                    .HasForeignKey(d => d.Stornozobizonylatkod)
                    .HasConstraintName("FK_BIZONYLAT_STORNOZO");

                entity.HasOne(d => d.StornozottbizonylatkodNavigation)
                    .WithMany(p => p.InverseStornozottbizonylatkodNavigation)
                    .HasForeignKey(d => d.Stornozottbizonylatkod)
                    .HasConstraintName("FK_BIZONYLAT_STORNOZOTT");

                entity.HasOne(d => d.UgyfelhelysegkodNavigation)
                    .WithMany(p => p.Bizonylat)
                    .HasForeignKey(d => d.Ugyfelhelysegkod)
                    .HasConstraintName("FK_BIZONYLAT_HELYSEG");

                entity.HasOne(d => d.UgyfelkodNavigation)
                    .WithMany(p => p.Bizonylat)
                    .HasForeignKey(d => d.Ugyfelkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLAT_UGYFEL");
            });

            modelBuilder.Entity<Bizonylatafa>(entity =>
            {
                entity.HasKey(e => e.Bizonylatafakod);

                entity.ToTable("BIZONYLATAFA");

                entity.HasIndex(e => e.Afakulcskod);

                entity.HasIndex(e => e.Bizonylatkod);

                entity.HasIndex(e => e.Particiokod);

                entity.Property(e => e.Bizonylatafakod).HasColumnName("BIZONYLATAFAKOD");

                entity.Property(e => e.Afa)
                    .HasColumnName("AFA")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Afakulcs)
                    .IsRequired()
                    .HasColumnName("AFAKULCS")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Afakulcskod).HasColumnName("AFAKULCSKOD");

                entity.Property(e => e.Afamerteke)
                    .HasColumnName("AFAMERTEKE")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Bizonylatkod).HasColumnName("BIZONYLATKOD");

                entity.Property(e => e.Brutto)
                    .HasColumnName("BRUTTO")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Netto)
                    .HasColumnName("NETTO")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.HasOne(d => d.AfakulcskodNavigation)
                    .WithMany(p => p.Bizonylatafa)
                    .HasForeignKey(d => d.Afakulcskod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLATAFA_AFAKULCS");

                entity.HasOne(d => d.BizonylatkodNavigation)
                    .WithMany(p => p.Bizonylatafa)
                    .HasForeignKey(d => d.Bizonylatkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLATAFA_BIZONYLAT");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Bizonylatafa)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLATAFA_PARTICIO");
            });

            modelBuilder.Entity<Bizonylatkapcsolat>(entity =>
            {
                entity.HasKey(e => e.Bizonylatkapcsolatkod);

                entity.ToTable("BIZONYLATKAPCSOLAT");

                entity.HasIndex(e => e.Bizonylatkod);

                entity.HasIndex(e => e.Iratkod);

                entity.HasIndex(e => e.Particiokod);

                entity.Property(e => e.Bizonylatkapcsolatkod).HasColumnName("BIZONYLATKAPCSOLATKOD");

                entity.Property(e => e.Bizonylatkod).HasColumnName("BIZONYLATKOD");

                entity.Property(e => e.Iratkod).HasColumnName("IRATKOD");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.HasOne(d => d.BizonylatkodNavigation)
                    .WithMany(p => p.Bizonylatkapcsolat)
                    .HasForeignKey(d => d.Bizonylatkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLATKAPCSOLAT_BIZONYLAT");

                entity.HasOne(d => d.IratkodNavigation)
                    .WithMany(p => p.Bizonylatkapcsolat)
                    .HasForeignKey(d => d.Iratkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLATKAPCSOLAT_IRAT");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Bizonylatkapcsolat)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLATKAPCSOLAT_PARTICIO");
            });

            modelBuilder.Entity<Bizonylattermekdij>(entity =>
            {
                entity.HasKey(e => e.Bizonylattermekdijkod);

                entity.ToTable("BIZONYLATTERMEKDIJ");

                entity.HasIndex(e => e.Bizonylatkod);

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Termekdijkod);

                entity.Property(e => e.Bizonylattermekdijkod).HasColumnName("BIZONYLATTERMEKDIJKOD");

                entity.Property(e => e.Bizonylatkod).HasColumnName("BIZONYLATKOD");

                entity.Property(e => e.Ossztomegkg)
                    .HasColumnName("OSSZTOMEGKG")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Termekdij)
                    .HasColumnName("TERMEKDIJ")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Termekdijegysegar)
                    .HasColumnName("TERMEKDIJEGYSEGAR")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Termekdijkod).HasColumnName("TERMEKDIJKOD");

                entity.Property(e => e.Termekdijkt)
                    .IsRequired()
                    .HasColumnName("TERMEKDIJKT")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Termekdijmegnevezes)
                    .IsRequired()
                    .HasColumnName("TERMEKDIJMEGNEVEZES")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BizonylatkodNavigation)
                    .WithMany(p => p.Bizonylattermekdij)
                    .HasForeignKey(d => d.Bizonylatkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLATTERMEKDIJ_BIZONYLAT");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Bizonylattermekdij)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLATTERMEKDIJ_PARTICIO");

                entity.HasOne(d => d.TermekdijkodNavigation)
                    .WithMany(p => p.Bizonylattermekdij)
                    .HasForeignKey(d => d.Termekdijkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLATTERMEKDIJ_TERMEKDIJ");
            });

            modelBuilder.Entity<Bizonylattetel>(entity =>
            {
                entity.HasKey(e => e.Bizonylattetelkod);

                entity.ToTable("BIZONYLATTETEL");

                entity.HasIndex(e => e.Afakulcskod);

                entity.HasIndex(e => e.Bizonylatkod);

                entity.HasIndex(e => e.Cikkkod);

                entity.HasIndex(e => e.Mekod);

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Termekdijkod);

                entity.Property(e => e.Bizonylattetelkod).HasColumnName("BIZONYLATTETELKOD");

                entity.Property(e => e.Afa)
                    .HasColumnName("AFA")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Afakulcs)
                    .IsRequired()
                    .HasColumnName("AFAKULCS")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Afakulcskod).HasColumnName("AFAKULCSKOD");

                entity.Property(e => e.Afamerteke)
                    .HasColumnName("AFAMERTEKE")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Bizonylatkod).HasColumnName("BIZONYLATKOD");

                entity.Property(e => e.Brutto)
                    .HasColumnName("BRUTTO")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Cikkkod).HasColumnName("CIKKKOD");

                entity.Property(e => e.Egysegar)
                    .HasColumnName("EGYSEGAR")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Ezeloleg).HasColumnName("EZELOLEG");

                entity.Property(e => e.Kozvetitettszolgaltatas).HasColumnName("KOZVETITETTSZOLGALTATAS");

                entity.Property(e => e.Me)
                    .IsRequired()
                    .HasColumnName("ME")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Megjegyzes)
                    .HasColumnName("MEGJEGYZES")
                    .HasColumnType("text");

                entity.Property(e => e.Megnevezes)
                    .IsRequired()
                    .HasColumnName("MEGNEVEZES")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mekod).HasColumnName("MEKOD");

                entity.Property(e => e.Mennyiseg)
                    .HasColumnName("MENNYISEG")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Netto)
                    .HasColumnName("NETTO")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Ossztomegkg)
                    .HasColumnName("OSSZTOMEGKG")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Termekdij)
                    .HasColumnName("TERMEKDIJ")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Termekdijas).HasColumnName("TERMEKDIJAS");

                entity.Property(e => e.Termekdijegysegar)
                    .HasColumnName("TERMEKDIJEGYSEGAR")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Termekdijkod).HasColumnName("TERMEKDIJKOD");

                entity.Property(e => e.Termekdijkt)
                    .HasColumnName("TERMEKDIJKT")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Termekdijmegnevezes)
                    .HasColumnName("TERMEKDIJMEGNEVEZES")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tomegkg)
                    .HasColumnName("TOMEGKG")
                    .HasColumnType("decimal(16, 2)");

                entity.HasOne(d => d.AfakulcskodNavigation)
                    .WithMany(p => p.Bizonylattetel)
                    .HasForeignKey(d => d.Afakulcskod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLATTETEL_AFAKULCS");

                entity.HasOne(d => d.BizonylatkodNavigation)
                    .WithMany(p => p.Bizonylattetel)
                    .HasForeignKey(d => d.Bizonylatkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLATTETEL_BIZONYLAT");

                entity.HasOne(d => d.CikkkodNavigation)
                    .WithMany(p => p.Bizonylattetel)
                    .HasForeignKey(d => d.Cikkkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLATTETEL_CIKK");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Bizonylattetel)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BIZONYLATTETEL_PARTICIO");

                entity.HasOne(d => d.TermekdijkodNavigation)
                    .WithMany(p => p.Bizonylattetel)
                    .HasForeignKey(d => d.Termekdijkod)
                    .HasConstraintName("FK_BIZONYLATTETEL_TERMEKDIJ");
            });

            modelBuilder.Entity<Cikk>(entity =>
            {
                entity.HasKey(e => e.Cikkkod);

                entity.ToTable("CIKK");

                entity.HasIndex(e => e.Afakulcskod);

                entity.HasIndex(e => e.Mekod);

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Termekdijkod);

                entity.HasIndex(e => new { e.Particiokod, e.Megnevezes })
                    .HasName("IX_CIKK_MEGNEVEZES")
                    .IsUnique();

                entity.Property(e => e.Cikkkod).HasColumnName("CIKKKOD");

                entity.Property(e => e.Afakulcskod).HasColumnName("AFAKULCSKOD");

                entity.Property(e => e.Egysegar)
                    .HasColumnName("EGYSEGAR")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Keszletetkepez).HasColumnName("KESZLETETKEPEZ");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Megnevezes)
                    .IsRequired()
                    .HasColumnName("MEGNEVEZES")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mekod).HasColumnName("MEKOD");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Termekdijkod).HasColumnName("TERMEKDIJKOD");

                entity.Property(e => e.Tomegkg)
                    .HasColumnName("TOMEGKG")
                    .HasColumnType("decimal(16, 2)");

                entity.HasOne(d => d.AfakulcskodNavigation)
                    .WithMany(p => p.Cikk)
                    .HasForeignKey(d => d.Afakulcskod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CIKK_AFAKULCS");

                entity.HasOne(d => d.MekodNavigation)
                    .WithMany(p => p.Cikk)
                    .HasForeignKey(d => d.Mekod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CIKK_MENNYISEGIEGYSEG");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Cikk)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CIKK_PARTICIO");

                entity.HasOne(d => d.TermekdijkodNavigation)
                    .WithMany(p => p.Cikk)
                    .HasForeignKey(d => d.Termekdijkod)
                    .HasConstraintName("FK_CIKK_TERMEKDIJ");
            });

            modelBuilder.Entity<Csoport>(entity =>
            {
                entity.HasKey(e => e.Csoportkod);

                entity.ToTable("CSOPORT");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => new { e.Particiokod, e.Csoport1 })
                    .HasName("IX_CSOPORT_CSOPORT")
                    .IsUnique();

                entity.Property(e => e.Csoportkod).HasColumnName("CSOPORTKOD");

                entity.Property(e => e.Csoport1)
                    .IsRequired()
                    .HasColumnName("CSOPORT")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Csoport)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CSOPORT_PARTICIO");
            });

            modelBuilder.Entity<Csoportfelhasznalo>(entity =>
            {
                entity.HasKey(e => e.Csoportfelhasznalokod);

                entity.ToTable("CSOPORTFELHASZNALO");

                entity.HasIndex(e => e.Csoportkod);

                entity.HasIndex(e => e.Felhasznalokod);

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => new { e.Csoportkod, e.Felhasznalokod })
                    .HasName("IX_CSOPORTFELHASZNALO")
                    .IsUnique();

                entity.Property(e => e.Csoportfelhasznalokod).HasColumnName("CSOPORTFELHASZNALOKOD");

                entity.Property(e => e.Csoportkod).HasColumnName("CSOPORTKOD");

                entity.Property(e => e.Felhasznalokod).HasColumnName("FELHASZNALOKOD");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.HasOne(d => d.CsoportkodNavigation)
                    .WithMany(p => p.Csoportfelhasznalo)
                    .HasForeignKey(d => d.Csoportkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CSOPORTFELHASZNALO_CSOPORT");

                entity.HasOne(d => d.FelhasznalokodNavigation)
                    .WithMany(p => p.Csoportfelhasznalo)
                    .HasForeignKey(d => d.Felhasznalokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CSOPORTFELHASZNALO_FELHASZNALO");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Csoportfelhasznalo)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CSOPORTFELHASZNALO_PARTICIO");
            });

            modelBuilder.Entity<Csoportjog>(entity =>
            {
                entity.HasKey(e => e.Csoportjogkod);

                entity.ToTable("CSOPORTJOG");

                entity.HasIndex(e => e.Csoportkod);

                entity.HasIndex(e => e.Lehetsegesjogkod);

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => new { e.Csoportkod, e.Lehetsegesjogkod })
                    .HasName("IX_CSOPORTJOG")
                    .IsUnique();

                entity.Property(e => e.Csoportjogkod).HasColumnName("CSOPORTJOGKOD");

                entity.Property(e => e.Csoportkod).HasColumnName("CSOPORTKOD");

                entity.Property(e => e.Lehetsegesjogkod).HasColumnName("LEHETSEGESJOGKOD");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.HasOne(d => d.CsoportkodNavigation)
                    .WithMany(p => p.Csoportjog)
                    .HasForeignKey(d => d.Csoportkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CSOPORTJOG_CSOPORT");

                entity.HasOne(d => d.LehetsegesjogkodNavigation)
                    .WithMany(p => p.Csoportjog)
                    .HasForeignKey(d => d.Lehetsegesjogkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CSOPORTJOG_LEHETSEGESJOG");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Csoportjog)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CSOPORTJOG_PARTICIO");
            });

            modelBuilder.Entity<Dokumentum>(entity =>
            {
                entity.HasKey(e => e.Dokumentumkod);

                entity.ToTable("DOKUMENTUM");

                entity.HasIndex(e => e.Iratkod);

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Volumekod);

                entity.Property(e => e.Dokumentumkod).HasColumnName("DOKUMENTUMKOD");

                entity.Property(e => e.Ext)
                    .HasColumnName("EXT")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Hash)
                    .IsRequired()
                    .HasColumnName("HASH")
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Iratkod).HasColumnName("IRATKOD");

                entity.Property(e => e.Konyvtar).HasColumnName("KONYVTAR");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Megjegyzes)
                    .HasColumnName("MEGJEGYZES")
                    .HasColumnType("text");

                entity.Property(e => e.Meret).HasColumnName("MERET");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Volumekod).HasColumnName("VOLUMEKOD");

                entity.HasOne(d => d.IratkodNavigation)
                    .WithMany(p => p.Dokumentum)
                    .HasForeignKey(d => d.Iratkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DOKUMENTUM_IRAT");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Dokumentum)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DOKUMENTUM_PARTICIO");

                entity.HasOne(d => d.VolumekodNavigation)
                    .WithMany(p => p.Dokumentum)
                    .HasForeignKey(d => d.Volumekod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DOKUMENTUM_VOLUME");
            });

            modelBuilder.Entity<Esemenynaplo>(entity =>
            {
                entity.HasKey(e => e.Esemenynaplokod);

                entity.ToTable("ESEMENYNAPLO");

                entity.HasIndex(e => e.Csoportkod);

                entity.HasIndex(e => e.Felhasznalokod);

                entity.HasIndex(e => e.Particiokod);

                entity.Property(e => e.Esemenynaplokod).HasColumnName("ESEMENYNAPLOKOD");

                entity.Property(e => e.Azonosito)
                    .IsRequired()
                    .HasColumnName("AZONOSITO")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Csoport)
                    .HasColumnName("CSOPORT")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Csoportkod).HasColumnName("CSOPORTKOD");

                entity.Property(e => e.Esemenyazonosito).HasColumnName("ESEMENYAZONOSITO");

                entity.Property(e => e.Felhasznalo)
                    .IsRequired()
                    .HasColumnName("FELHASZNALO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Felhasznalokod).HasColumnName("FELHASZNALOKOD");

                entity.Property(e => e.Host)
                    .HasColumnName("HOST")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Idopont)
                    .HasColumnName("IDOPONT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Osuser)
                    .HasColumnName("OSUSER")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Particio)
                    .HasColumnName("PARTICIO")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.HasOne(d => d.CsoportkodNavigation)
                    .WithMany(p => p.Esemenynaplo)
                    .HasForeignKey(d => d.Csoportkod)
                    .HasConstraintName("FK_ESEMENYNAPLO_CSOPORT");

                entity.HasOne(d => d.FelhasznalokodNavigation)
                    .WithMany(p => p.Esemenynaplo)
                    .HasForeignKey(d => d.Felhasznalokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ESEMENYNAPLO_FELHASZNALO");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Esemenynaplo)
                    .HasForeignKey(d => d.Particiokod)
                    .HasConstraintName("FK_ESEMENYNAPLO_PARTICIO");
            });

            modelBuilder.Entity<Felhasznalo>(entity =>
            {
                entity.HasKey(e => e.Felhasznalokod);

                entity.ToTable("FELHASZNALO");

                entity.HasIndex(e => e.Azonosito)
                    .IsUnique();

                entity.HasIndex(e => e.Nev)
                    .IsUnique();

                entity.Property(e => e.Felhasznalokod).HasColumnName("FELHASZNALOKOD");

                entity.Property(e => e.Azonosito)
                    .IsRequired()
                    .HasColumnName("AZONOSITO")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Jelszo)
                    .IsRequired()
                    .HasColumnName("JELSZO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Logonlog).HasColumnName("LOGONLOG");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nev)
                    .IsRequired()
                    .HasColumnName("NEV")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Statusz)
                    .IsRequired()
                    .HasColumnName("STATUSZ")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Statuszkelte)
                    .HasColumnName("STATUSZKELTE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Telefon)
                    .HasColumnName("TELEFON")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Fizetesimod>(entity =>
            {
                entity.HasKey(e => e.Fizetesimodkod);

                entity.ToTable("FIZETESIMOD");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => new { e.Particiokod, e.Fizetesimodkod })
                    .HasName("IX_FIZETESIMOD_FIZETESIMOD")
                    .IsUnique();

                entity.Property(e => e.Fizetesimodkod).HasColumnName("FIZETESIMODKOD");

                entity.Property(e => e.Fizetesimod1)
                    .IsRequired()
                    .HasColumnName("FIZETESIMOD")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Fizetesimod)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FizetesiMod_PARTICIO");
            });

            modelBuilder.Entity<Helyseg>(entity =>
            {
                entity.HasKey(e => e.Helysegkod);

                entity.ToTable("HELYSEG");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => new { e.Particiokod, e.Helysegnev })
                    .HasName("IX_HELYSEG_HELYSEGNEV")
                    .IsUnique();

                entity.Property(e => e.Helysegkod).HasColumnName("HELYSEGKOD");

                entity.Property(e => e.Helysegnev)
                    .IsRequired()
                    .HasColumnName("HELYSEGNEV")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Helyseg)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HELYSEG_PARTICIO");
            });

            modelBuilder.Entity<Irat>(entity =>
            {
                entity.HasKey(e => e.Iratkod);

                entity.ToTable("IRAT");

                entity.HasIndex(e => e.Irattipuskod);

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Targy);

                entity.HasIndex(e => e.Ugyfelkod);

                entity.Property(e => e.Iratkod).HasColumnName("IRATKOD");

                entity.Property(e => e.Irany)
                    .IsRequired()
                    .HasColumnName("IRANY")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Irattipuskod).HasColumnName("IRATTIPUSKOD");

                entity.Property(e => e.Keletkezett)
                    .HasColumnName("KELETKEZETT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Kikuldesikod)
                    .HasColumnName("KIKULDESIKOD")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Kikuldesikodidopontja)
                    .HasColumnName("KIKULDESIKODIDOPONTJA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Kuldo)
                    .HasColumnName("KULDO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Targy)
                    .HasColumnName("TARGY")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Ugyfelkod).HasColumnName("UGYFELKOD");

                entity.HasOne(d => d.IrattipuskodNavigation)
                    .WithMany(p => p.Irat)
                    .HasForeignKey(d => d.Irattipuskod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IRAT_IRATTIPUS");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Irat)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IRAT_PARTICIO");

                entity.HasOne(d => d.UgyfelkodNavigation)
                    .WithMany(p => p.Irat)
                    .HasForeignKey(d => d.Ugyfelkod)
                    .HasConstraintName("FK_IRAT_UGYFEL");
            });

            modelBuilder.Entity<Irattipus>(entity =>
            {
                entity.HasKey(e => e.Irattipuskod);

                entity.ToTable("IRATTIPUS");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => new { e.Particiokod, e.Irattipus1 })
                    .HasName("IX_IRATTIPUS_IRATTIPUS")
                    .IsUnique();

                entity.Property(e => e.Irattipuskod).HasColumnName("IRATTIPUSKOD");

                entity.Property(e => e.Irattipus1)
                    .IsRequired()
                    .HasColumnName("IRATTIPUS")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Irattipus)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IRATTIPUS_PARTICIO");
            });

            modelBuilder.Entity<Kifizetes>(entity =>
            {
                entity.HasKey(e => e.Kifizeteskod);

                entity.ToTable("KIFIZETES");

                entity.HasIndex(e => e.Bizonylatkod);

                entity.HasIndex(e => e.Fizetesimodkod);

                entity.HasIndex(e => e.Particiokod)
                    .HasName("IX_KIFIZETES");

                entity.HasIndex(e => e.Penznemkod);

                entity.Property(e => e.Kifizeteskod).HasColumnName("KIFIZETESKOD");

                entity.Property(e => e.Bizonylatkod).HasColumnName("BIZONYLATKOD");

                entity.Property(e => e.Datum)
                    .HasColumnName("DATUM")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fizetesimodkod).HasColumnName("FIZETESIMODKOD");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Osszeg)
                    .HasColumnName("OSSZEG")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Penznemkod).HasColumnName("PENZNEMKOD");

                entity.HasOne(d => d.BizonylatkodNavigation)
                    .WithMany(p => p.Kifizetes)
                    .HasForeignKey(d => d.Bizonylatkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KIFIZETES_BIZONYLAT");

                entity.HasOne(d => d.FizetesimodkodNavigation)
                    .WithMany(p => p.Kifizetes)
                    .HasForeignKey(d => d.Fizetesimodkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KIFIZETES_FIZETESIMOD");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Kifizetes)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KIFIZETES_PARTICIO");

                entity.HasOne(d => d.PenznemkodNavigation)
                    .WithMany(p => p.Kifizetes)
                    .HasForeignKey(d => d.Penznemkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KIFIZETES_PENZNEM");
            });

            modelBuilder.Entity<Kodgenerator>(entity =>
            {
                entity.ToTable("KODGENERATOR");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => new { e.Particiokod, e.Kodnev })
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Kodnev)
                    .IsRequired()
                    .HasColumnName("KODNEV")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Kovetkezokod).HasColumnName("KOVETKEZOKOD");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Kodgenerator)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_KODGENERATOR_PARTICIO");
            });

            modelBuilder.Entity<Lehetsegesjog>(entity =>
            {
                entity.HasKey(e => e.Lehetsegesjogkod);

                entity.ToTable("LEHETSEGESJOG");

                entity.HasIndex(e => e.Jogkod)
                    .IsUnique();

                entity.Property(e => e.Lehetsegesjogkod).HasColumnName("LEHETSEGESJOGKOD");

                entity.Property(e => e.Jog)
                    .IsRequired()
                    .HasColumnName("JOG")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Jogkod)
                    .IsRequired()
                    .HasColumnName("JOGKOD")
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Mennyisegiegyseg>(entity =>
            {
                entity.HasKey(e => e.Mekod);

                entity.ToTable("MENNYISEGIEGYSEG");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => new { e.Particiokod, e.Me })
                    .HasName("IX_MENNYISEGIEGYSEG_ME")
                    .IsUnique();

                entity.Property(e => e.Mekod).HasColumnName("MEKOD");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Me)
                    .IsRequired()
                    .HasColumnName("ME")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Mennyisegiegyseg)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MENNYISEGIEGYSEG_PARTICIO");
            });

            modelBuilder.Entity<Navfeltoltes>(entity =>
            {
                entity.HasKey(e => e.Navfeltolteskod);

                entity.ToTable("NAVFELTOLTES");

                entity.HasIndex(e => e.Bizonylatkod);

                entity.Property(e => e.Navfeltolteskod).HasColumnName("NAVFELTOLTESKOD");

                entity.Property(e => e.Bizonylatkod).HasColumnName("BIZONYLATKOD");

                entity.Property(e => e.Elintezte)
                    .HasColumnName("ELINTEZTE")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Elintezve)
                    .HasColumnName("ELINTEZVE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Emailszamlalo).HasColumnName("EMAILSZAMLALO");

                entity.Property(e => e.Feltoltesellenorzesszamlalo).HasColumnName("FELTOLTESELLENORZESSZAMLALO");

                entity.Property(e => e.Feltoltesszamlalo).HasColumnName("FELTOLTESSZAMLALO");

                entity.Property(e => e.Hiba)
                    .HasColumnName("HIBA")
                    .HasColumnType("text");

                entity.Property(e => e.Idopont)
                    .HasColumnName("IDOPONT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Kovetkezoteendoidopont)
                    .HasColumnName("KOVETKEZOTEENDOIDOPONT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Statusz).HasColumnName("STATUSZ");

                entity.Property(e => e.Token)
                    .HasColumnName("TOKEN")
                    .HasColumnType("text");

                entity.Property(e => e.Tokenkeresszamlalo).HasColumnName("TOKENKERESSZAMLALO");

                entity.Property(e => e.Tranzakcioazonosito)
                    .HasColumnName("TRANZAKCIOAZONOSITO")
                    .HasColumnType("text");

                entity.HasOne(d => d.BizonylatkodNavigation)
                    .WithMany(p => p.Navfeltoltes)
                    .HasForeignKey(d => d.Bizonylatkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NAVFELTOLTES_BIZONYLAT");
            });

            modelBuilder.Entity<Particio>(entity =>
            {
                entity.HasKey(e => e.Particiokod);

                entity.ToTable("PARTICIO");

                entity.HasIndex(e => e.Megnevezes)
                    .IsUnique();

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Bizonylat)
                    .HasColumnName("BIZONYLAT")
                    .HasColumnType("text");

                entity.Property(e => e.Emails)
                    .HasColumnName("EMAILS")
                    .HasColumnType("text");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Megnevezes)
                    .IsRequired()
                    .HasColumnName("MEGNEVEZES")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Navonlineszamla)
                    .HasColumnName("NAVONLINESZAMLA")
                    .HasColumnType("text");

                entity.Property(e => e.Projekt)
                    .HasColumnName("PROJEKT")
                    .HasColumnType("text");

                entity.Property(e => e.Szallito)
                    .HasColumnName("SZALLITO")
                    .HasColumnType("text");

                entity.Property(e => e.Volume)
                    .HasColumnName("VOLUME")
                    .HasColumnType("text");
            });

            modelBuilder.Entity<Penznem>(entity =>
            {
                entity.HasKey(e => e.Penznemkod);

                entity.ToTable("PENZNEM");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => new { e.Particiokod, e.Penznem1 })
                    .HasName("IX_PENZNEM_PENZNEM")
                    .IsUnique();

                entity.Property(e => e.Penznemkod).HasColumnName("PENZNEMKOD");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Penznem1)
                    .IsRequired()
                    .HasColumnName("PENZNEM")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Penznem)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PENZNEM_PARTICIO");
            });

            modelBuilder.Entity<Penztar>(entity =>
            {
                entity.HasKey(e => e.Penztarkod);

                entity.ToTable("PENZTAR");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Penznemkod);

                entity.HasIndex(e => e.Penztar1);

                entity.Property(e => e.Penztarkod).HasColumnName("PENZTARKOD");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nyitva).HasColumnName("NYITVA");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Penznem)
                    .IsRequired()
                    .HasColumnName("PENZNEM")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Penznemkod).HasColumnName("PENZNEMKOD");

                entity.Property(e => e.Penztar1)
                    .IsRequired()
                    .HasColumnName("PENZTAR")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Penztar)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PENZTAR_PARTICIO");

                entity.HasOne(d => d.PenznemkodNavigation)
                    .WithMany(p => p.Penztar)
                    .HasForeignKey(d => d.Penznemkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PENZTAR_PENZNEM");
            });

            modelBuilder.Entity<Penztartetel>(entity =>
            {
                entity.HasKey(e => e.Penztartetelkod);

                entity.ToTable("PENZTARTETEL");

                entity.HasIndex(e => e.Bizonylatszam);

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Penztarbizonylatszam);

                entity.HasIndex(e => e.Penztarkod);

                entity.HasIndex(e => e.Ugyfelkod);

                entity.HasIndex(e => e.Ugyfelnev);

                entity.Property(e => e.Penztartetelkod).HasColumnName("PENZTARTETELKOD");

                entity.Property(e => e.Bevetel)
                    .HasColumnName("BEVETEL")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Bizonylatszam)
                    .HasColumnName("BIZONYLATSZAM")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Datum)
                    .HasColumnName("DATUM")
                    .HasColumnType("datetime");

                entity.Property(e => e.Jogcim)
                    .IsRequired()
                    .HasColumnName("JOGCIM")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Kiadas)
                    .HasColumnName("KIADAS")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Megjegyzes)
                    .HasColumnName("MEGJEGYZES")
                    .HasColumnType("text");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Penztarbizonylatszam)
                    .IsRequired()
                    .HasColumnName("PENZTARBIZONYLATSZAM")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Penztarkod).HasColumnName("PENZTARKOD");

                entity.Property(e => e.Ugyfelkod).HasColumnName("UGYFELKOD");

                entity.Property(e => e.Ugyfelnev)
                    .HasColumnName("UGYFELNEV")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Penztartetel)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PENZTARTETEL_PARTICIO");

                entity.HasOne(d => d.PenztarkodNavigation)
                    .WithMany(p => p.Penztartetel)
                    .HasForeignKey(d => d.Penztarkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PENZTARTETEL_PENZTAR");

                entity.HasOne(d => d.UgyfelkodNavigation)
                    .WithMany(p => p.Penztartetel)
                    .HasForeignKey(d => d.Ugyfelkod)
                    .HasConstraintName("FK_PENZTARTETEL_UGYFEL");
            });

            modelBuilder.Entity<Projekt>(entity =>
            {
                entity.HasKey(e => e.Projektkod);

                entity.ToTable("PROJEKT");

                entity.HasIndex(e => e.Keletkezett)
                    .HasName("IX_PROJEKT_KELETLEZETT");

                entity.HasIndex(e => e.Muszakiallapot);

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Penznemkod);

                entity.HasIndex(e => e.Telepitesicim);

                entity.HasIndex(e => e.Ugyfelkod);

                entity.Property(e => e.Projektkod).HasColumnName("PROJEKTKOD");

                entity.Property(e => e.Ackva)
                    .HasColumnName("ACKVA")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Dckw)
                    .HasColumnName("DCKW")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Inverter)
                    .HasColumnName("INVERTER")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Inverterallapot)
                    .HasColumnName("INVERTERALLAPOT")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Keletkezett)
                    .HasColumnName("KELETKEZETT")
                    .HasColumnType("datetime");

                entity.Property(e => e.Kivitelezesihatarido)
                    .HasColumnName("KIVITELEZESIHATARIDO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Megjegyzes)
                    .HasColumnName("MEGJEGYZES")
                    .HasColumnType("text");

                entity.Property(e => e.Megrendelve)
                    .HasColumnName("MEGRENDELVE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Munkalapszam)
                    .HasColumnName("MUNKALAPSZAM")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Muszakiallapot)
                    .HasColumnName("MUSZAKIALLAPOT")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Napelem)
                    .HasColumnName("NAPELEM")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Napelemallapot)
                    .HasColumnName("NAPELEMALLAPOT")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Penznemkod).HasColumnName("PENZNEMKOD");

                entity.Property(e => e.Projektjellege)
                    .HasColumnName("PROJEKTJELLEGE")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Statusz).HasColumnName("STATUSZ");

                entity.Property(e => e.Telepitesicim)
                    .HasColumnName("TELEPITESICIM")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Ugyfelkod).HasColumnName("UGYFELKOD");

                entity.Property(e => e.Vallalasiarnetto)
                    .HasColumnName("VALLALASIARNETTO")
                    .HasColumnType("decimal(16, 2)");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.ProjektNavigation)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJEKT_PARTICIO");

                entity.HasOne(d => d.PenznemkodNavigation)
                    .WithMany(p => p.Projekt)
                    .HasForeignKey(d => d.Penznemkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJEKT_PENZNEM");

                entity.HasOne(d => d.UgyfelkodNavigation)
                    .WithMany(p => p.Projekt)
                    .HasForeignKey(d => d.Ugyfelkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJEKT_UGYFEL");
            });

            modelBuilder.Entity<Projektkapcsolat>(entity =>
            {
                entity.HasKey(e => e.Projektkapcsolatkod);

                entity.ToTable("PROJEKTKAPCSOLAT");

                entity.HasIndex(e => e.Bizonylatkod);

                entity.HasIndex(e => e.Iratkod);

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Projektkod);

                entity.Property(e => e.Projektkapcsolatkod).HasColumnName("PROJEKTKAPCSOLATKOD");

                entity.Property(e => e.Bizonylatkod).HasColumnName("BIZONYLATKOD");

                entity.Property(e => e.Iratkod).HasColumnName("IRATKOD");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Projektkod).HasColumnName("PROJEKTKOD");

                entity.HasOne(d => d.BizonylatkodNavigation)
                    .WithMany(p => p.Projektkapcsolat)
                    .HasForeignKey(d => d.Bizonylatkod)
                    .HasConstraintName("FK_PROJEKTKAPCSOLAT_BIZONYLAT");

                entity.HasOne(d => d.IratkodNavigation)
                    .WithMany(p => p.Projektkapcsolat)
                    .HasForeignKey(d => d.Iratkod)
                    .HasConstraintName("FK_PROJEKTKAPCSOLAT_IRAT");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Projektkapcsolat)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJEKTKAPCSOLAT_PARTICIO");

                entity.HasOne(d => d.ProjektkodNavigation)
                    .WithMany(p => p.Projektkapcsolat)
                    .HasForeignKey(d => d.Projektkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJEKTKAPCSOLAT_PROJEKT");
            });

            modelBuilder.Entity<Projektteendo>(entity =>
            {
                entity.HasKey(e => e.Projektteendokod);

                entity.ToTable("PROJEKTTEENDO");

                entity.HasIndex(e => e.Dedikalva);

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Projektkod);

                entity.HasIndex(e => e.Teendokod);

                entity.Property(e => e.Projektteendokod).HasColumnName("PROJEKTTEENDOKOD");

                entity.Property(e => e.Dedikalva)
                    .HasColumnName("DEDIKALVA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Elvegezve)
                    .HasColumnName("ELVEGEZVE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Hatarido)
                    .HasColumnName("HATARIDO")
                    .HasColumnType("datetime");

                entity.Property(e => e.Leiras)
                    .HasColumnName("LEIRAS")
                    .HasColumnType("text");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Projektkod).HasColumnName("PROJEKTKOD");

                entity.Property(e => e.Teendokod).HasColumnName("TEENDOKOD");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Projektteendo)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJEKTTEENDO_PARTICIO");

                entity.HasOne(d => d.ProjektkodNavigation)
                    .WithMany(p => p.Projektteendo)
                    .HasForeignKey(d => d.Projektkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJEKTTEENDO_PROJEKT");

                entity.HasOne(d => d.TeendokodNavigation)
                    .WithMany(p => p.Projektteendo)
                    .HasForeignKey(d => d.Teendokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROJEKTTEENDO_TEENDO");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("SESSION");

                entity.HasIndex(e => e.Csoportkod);

                entity.HasIndex(e => e.Felhasznalokod);

                entity.HasIndex(e => e.Particiokod);

                entity.Property(e => e.Sessionid)
                    .HasColumnName("SESSIONID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Azonosito)
                    .IsRequired()
                    .HasColumnName("AZONOSITO")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Csoport)
                    .HasColumnName("CSOPORT")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Csoportkod).HasColumnName("CSOPORTKOD");

                entity.Property(e => e.Ervenyes)
                    .HasColumnName("ERVENYES")
                    .HasColumnType("datetime");

                entity.Property(e => e.Felhasznalo)
                    .IsRequired()
                    .HasColumnName("FELHASZNALO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Felhasznalokod).HasColumnName("FELHASZNALOKOD");

                entity.Property(e => e.Host)
                    .HasColumnName("HOST")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ip)
                    .HasColumnName("IP")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Logol).HasColumnName("LOGOL");

                entity.Property(e => e.Osuser)
                    .HasColumnName("OSUSER")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Particio)
                    .HasColumnName("PARTICIO")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.HasOne(d => d.CsoportkodNavigation)
                    .WithMany(p => p.Session)
                    .HasForeignKey(d => d.Csoportkod)
                    .HasConstraintName("FK_SESSION_CSOPORT");

                entity.HasOne(d => d.FelhasznalokodNavigation)
                    .WithMany(p => p.Session)
                    .HasForeignKey(d => d.Felhasznalokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SESSION_FELHASZNALO");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Session)
                    .HasForeignKey(d => d.Particiokod)
                    .HasConstraintName("FK_SESSION_PARTICIO");
            });

            modelBuilder.Entity<Szamlazasirend>(entity =>
            {
                entity.HasKey(e => e.Szamlazasirendkod);

                entity.ToTable("SZAMLAZASIREND");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Penznemkod);

                entity.HasIndex(e => e.Projektkod);

                entity.Property(e => e.Szamlazasirendkod).HasColumnName("SZAMLAZASIRENDKOD");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Megjegyzes)
                    .HasColumnName("MEGJEGYZES")
                    .HasColumnType("text");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Osszeg)
                    .HasColumnName("OSSZEG")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Penznemkod).HasColumnName("PENZNEMKOD");

                entity.Property(e => e.Projektkod).HasColumnName("PROJEKTKOD");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Szamlazasirend)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SZAMLAZASIREND_PARTICIO");

                entity.HasOne(d => d.PenznemkodNavigation)
                    .WithMany(p => p.Szamlazasirend)
                    .HasForeignKey(d => d.Penznemkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SZAMLAZASIREND_PENZNEM");

                entity.HasOne(d => d.ProjektkodNavigation)
                    .WithMany(p => p.Szamlazasirend)
                    .HasForeignKey(d => d.Projektkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SZAMLAZASIREND_PROJEKT");
            });

            modelBuilder.Entity<Teendo>(entity =>
            {
                entity.HasKey(e => e.Teendokod);

                entity.ToTable("TEENDO");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => new { e.Particiokod, e.Teendo1 })
                    .HasName("IX_TEENDO_TEENDO")
                    .IsUnique();

                entity.Property(e => e.Teendokod).HasColumnName("TEENDOKOD");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Teendo1)
                    .IsRequired()
                    .HasColumnName("TEENDO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Teendo)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TEENDO_PARTICIO");
            });

            modelBuilder.Entity<Termekdij>(entity =>
            {
                entity.HasKey(e => e.Termekdijkod);

                entity.ToTable("TERMEKDIJ");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Termekdijkt)
                    .IsUnique();

                entity.HasIndex(e => e.Termekdijmegnevezes);

                entity.Property(e => e.Termekdijkod).HasColumnName("TERMEKDIJKOD");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Termekdijegysegar)
                    .HasColumnName("TERMEKDIJEGYSEGAR")
                    .HasColumnType("decimal(16, 2)");

                entity.Property(e => e.Termekdijkt)
                    .IsRequired()
                    .HasColumnName("TERMEKDIJKT")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Termekdijmegnevezes)
                    .IsRequired()
                    .HasColumnName("TERMEKDIJMEGNEVEZES")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Termekdij)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TERMEKDIJ_PARTICIO");
            });

            modelBuilder.Entity<Ugyfel>(entity =>
            {
                entity.HasKey(e => e.Ugyfelkod);

                entity.ToTable("UGYFEL");

                entity.HasIndex(e => e.Helysegkod);

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => new { e.Csoport, e.Ceg })
                    .HasName("IX_UGYFEL_CEG");

                entity.HasIndex(e => new { e.Csoport, e.Email })
                    .HasName("IX_UGYFEL_EMAIL");

                entity.HasIndex(e => new { e.Csoport, e.Nev })
                    .HasName("IX_UGYFEL_NEV");

                entity.HasIndex(e => new { e.Csoport, e.Telefon })
                    .HasName("IX_UGYFEL_TELEFON");

                entity.Property(e => e.Ugyfelkod).HasColumnName("UGYFELKOD");

                entity.Property(e => e.Adoszam)
                    .HasColumnName("ADOSZAM")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Ajanlotta)
                    .HasColumnName("AJANLOTTA")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Beosztas)
                    .HasColumnName("BEOSZTAS")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Ceg)
                    .HasColumnName("CEG")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Csoport).HasColumnName("CSOPORT");

                entity.Property(e => e.Egyeblink)
                    .HasColumnName("EGYEBLINK")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Euadoszam)
                    .HasColumnName("EUADOSZAM")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Hazszam)
                    .HasColumnName("HAZSZAM")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Helysegkod).HasColumnName("HELYSEGKOD");

                entity.Property(e => e.Hirlevel).HasColumnName("HIRLEVEL");

                entity.Property(e => e.Iranyitoszam)
                    .HasColumnName("IRANYITOSZAM")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Kikuldesikod)
                    .HasColumnName("KIKULDESIKOD")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Kikuldesikodidopontja)
                    .HasColumnName("KIKULDESIKODIDOPONTJA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Kozterulet)
                    .HasColumnName("KOZTERULET")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Kozterulettipus)
                    .HasColumnName("KOZTERULETTIPUS")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Megjegyzes)
                    .HasColumnName("MEGJEGYZES")
                    .HasColumnType("text");

                entity.Property(e => e.Modositotta)
                    .IsRequired()
                    .HasColumnName("MODOSITOTTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Modositva)
                    .HasColumnName("MODOSITVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Nev)
                    .IsRequired()
                    .HasColumnName("NEV")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Telefon)
                    .HasColumnName("TELEFON")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Utcahazszam)
                    .HasColumnName("UTCAHAZSZAM")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Vasarolt).HasColumnName("VASAROLT");

                entity.HasOne(d => d.HelysegkodNavigation)
                    .WithMany(p => p.Ugyfel)
                    .HasForeignKey(d => d.Helysegkod)
                    .HasConstraintName("FK_UGYFEL_HELYSEG");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Ugyfel)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UGYFEL_PARTICIO");
            });

            modelBuilder.Entity<Ugyfelterlog>(entity =>
            {
                entity.HasKey(e => e.Ugyfelterlogkod);

                entity.ToTable("UGYFELTERLOG");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => e.Ugyfelkod);

                entity.Property(e => e.Ugyfelterlogkod).HasColumnName("UGYFELTERLOGKOD");

                entity.Property(e => e.Letrehozta)
                    .IsRequired()
                    .HasColumnName("LETREHOZTA")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Letrehozva)
                    .HasColumnName("LETREHOZVA")
                    .HasColumnType("datetime");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Ugyfelkod).HasColumnName("UGYFELKOD");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.Ugyfelterlog)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UGYFELTERLOG_PARTICIO");

                entity.HasOne(d => d.UgyfelkodNavigation)
                    .WithMany(p => p.Ugyfelterlog)
                    .HasForeignKey(d => d.Ugyfelkod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UGYFELTERLOG_UGYFEL");
            });

            modelBuilder.Entity<Verzio>(entity =>
            {
                entity.HasKey(e => e.Verzio1);

                entity.ToTable("VERZIO");

                entity.Property(e => e.Verzio1)
                    .HasColumnName("VERZIO")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Volume>(entity =>
            {
                entity.HasKey(e => e.Volumekod);

                entity.ToTable("VOLUME");

                entity.HasIndex(e => e.Particiokod);

                entity.HasIndex(e => new { e.Particiokod, e.Volumeno })
                    .HasName("IX_VOLUME_VOLUME")
                    .IsUnique();

                entity.Property(e => e.Volumekod).HasColumnName("VOLUMEKOD");

                entity.Property(e => e.Allapot)
                    .IsRequired()
                    .HasColumnName("ALLAPOT")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Allapotkelte)
                    .HasColumnName("ALLAPOTKELTE")
                    .HasColumnType("datetime");

                entity.Property(e => e.Eleresiut)
                    .IsRequired()
                    .HasColumnName("ELERESIUT")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Fajlokszamautolsokonyvtarban).HasColumnName("FAJLOKSZAMAUTOLSOKONYVTARBAN");

                entity.Property(e => e.Jelenlegimeret).HasColumnName("JELENLEGIMERET");

                entity.Property(e => e.Maxmeret).HasColumnName("MAXMERET");

                entity.Property(e => e.Particiokod).HasColumnName("PARTICIOKOD");

                entity.Property(e => e.Utolsokonyvtar).HasColumnName("UTOLSOKONYVTAR");

                entity.Property(e => e.Volumeno).HasColumnName("VOLUMENO");

                entity.HasOne(d => d.ParticiokodNavigation)
                    .WithMany(p => p.VolumeNavigation)
                    .HasForeignKey(d => d.Particiokod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VOLUME_PARTICIO");
            });
        }
    }
}
