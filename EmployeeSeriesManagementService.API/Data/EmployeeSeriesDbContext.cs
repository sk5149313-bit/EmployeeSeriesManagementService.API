using EmployeeSeriesManagementService.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSeriesManagementService.Api.Data;

public class EmployeeSeriesDbContext(DbContextOptions<EmployeeSeriesDbContext> options)
    : DbContext(options)
{
    //all dbset added here in dbcontext with there keys and relations
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<AddressType> AddressTypes => Set<AddressType>();
    public DbSet<EmployeeAddress> EmployeeAddresses => Set<EmployeeAddress>();
    public DbSet<Series> Series => Set<Series>();
    public DbSet<EmployeeSeries> EmployeeSeries => Set<EmployeeSeries>();
    public DbSet<EmployeeIdCard> EmployeeIdCards => Set<EmployeeIdCard>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>(e =>
        {
            e.ToTable("Employees");
            e.HasKey(x => x.EmployeeId);
            e.Property(x => x.UserId).HasMaxLength(50);
            e.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(50).IsRequired();
            e.Property(x => x.SecondName).HasMaxLength(50);
            e.Property(x => x.Language).HasMaxLength(2).IsRequired();
            e.ToTable(t => t.HasCheckConstraint("CK_Employees_Language", "[Language] IN ('FR','NL')"));
            e.Property(x => x.BirthPlace).HasMaxLength(50);
            e.Property(x => x.Nationality).HasMaxLength(3).IsUnicode(false);
            e.Property(x => x.EmailAddress).HasMaxLength(150);
            e.Property(x => x.PhoneNumber).HasMaxLength(30);
            e.Property(x => x.ProfileImage).HasColumnType("varbinary(max)");
        });

        modelBuilder.Entity<AddressType>(e =>
        {
            e.ToTable("AddressTypes");
            e.HasKey(x => x.Id);
            e.Property(x => x.Description).HasMaxLength(10).IsRequired();
            e.HasData(
                new AddressType { Id = 1, Description = "Personal" },
                new AddressType { Id = 2, Description = "Work" });
        });

        modelBuilder.Entity<Address>(e =>
        {
            e.ToTable("Addresses");
            e.HasKey(x => x.Id);
            e.Property(x => x.Country).HasMaxLength(50).IsRequired();
            e.Property(x => x.City).HasMaxLength(50).IsRequired();
            e.Property(x => x.ZipCode).HasMaxLength(15);
            e.Property(x => x.Street).HasMaxLength(50);
            e.Property(x => x.Number).HasMaxLength(10);
            e.Property(x => x.MailboxNumber).HasMaxLength(10);
            e.Property(x => x.Building).HasMaxLength(10);
            e.Property(x => x.Floor).HasMaxLength(10);
            e.HasOne(x => x.AddressType).WithMany(x => x.Addresses)
                .HasForeignKey(x => x.AddressTypeId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<EmployeeAddress>(e =>
        {
            e.ToTable("EmployeesAddresses");
            e.HasKey(x => new { x.EmployeeId, x.AddressId });
            e.HasOne(x => x.Employee).WithMany(x => x.EmployeeAddresses)
                .HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Address).WithMany(x => x.EmployeeAddresses)
                .HasForeignKey(x => x.AddressId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => x.AddressId);
        });

        modelBuilder.Entity<Series>(e =>
        {
            e.ToTable("Series");
            e.Property(x=>x.Code).ValueGeneratedNever();
            e.HasKey(x => x.Code);
            e.Property(x => x.Name).HasMaxLength(255).IsRequired();
        });

        modelBuilder.Entity<EmployeeSeries>(e =>
        {
            e.ToTable("EmployeesSeries");
            e.HasKey(x => new { x.EmployeeId, x.SeriesCode, x.StartDate });
            e.HasOne(x => x.Employee).WithMany(x => x.EmployeeSeries)
                .HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Series).WithMany(x => x.EmployeeSeries)
                .HasForeignKey(x => x.SeriesCode).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => new { x.EmployeeId, x.StartDate, x.EndDate });
            e.ToTable(t => t.HasCheckConstraint(
                "CK_EmployeesSeries_DateRange", "[EndDate] IS NULL OR [EndDate] >= [StartDate]"));
        });

        modelBuilder.Entity<EmployeeIdCard>(e =>
        {
            e.ToTable("EmployeesIdCards");
            e.HasKey(x => x.Id);
            e.Property(x => x.Number).HasMaxLength(250).IsRequired();
            e.HasOne(x => x.Employee).WithMany(x => x.IdCards)
                .HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
