using Microsoft.EntityFrameworkCore;

namespace ImportExport.Models;

public partial class DBContext : DbContext
{
    public  DBContext() { }
    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblCustomer> TblCustomers { get; set; }

    public virtual DbSet<TblMovie> TblMovies { get; set; }

    public virtual DbSet<TblRent> TblRents { get; set; }

    public virtual DbSet<VwCusMvRent> VwCusMvRents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblCustomer>(entity =>
        {
            entity.HasKey(e => e.CusId);

            entity.ToTable("tbl_customer");

            entity.Property(e => e.CusId).ValueGeneratedNever();
            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Salutation)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<TblMovie>(entity =>
        {
            entity.HasKey(e => e.MvId);

            entity.ToTable("tbl_movie");

            entity.Property(e => e.MvId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(125)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblRent>(entity =>
        {
            entity.HasKey(e => e.RentId);

            entity.ToTable("tbl_rent");

            entity.Property(e => e.RentId).ValueGeneratedNever();
            entity.Property(e => e.RentAt).HasColumnType("datetime");
            entity.Property(e => e.ReturnAt).HasColumnType("datetime");

            entity.HasOne(d => d.Cus).WithMany(p => p.TblRents)
                .HasForeignKey(d => d.CusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_customer_tbl_rent");

            entity.HasOne(d => d.Mv).WithMany(p => p.TblRents)
                .HasForeignKey(d => d.MvId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_movie_tbl_rent");
        });

        modelBuilder.Entity<VwCusMvRent>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_cus_mv_rent");

            entity.Property(e => e.Address)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Salutation)
                .IsRequired()
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(125)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var configSection = configBuilder.GetSection("ConnectionStrings");
        var connectionStrings = configSection["DefaultConnection"] ?? null;
        optionsBuilder.UseSqlServer(connectionStrings);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}