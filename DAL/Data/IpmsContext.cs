using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public partial class _IpmsContext : DbContext
{
    public _IpmsContext()
    {
    }

    public _IpmsContext(DbContextOptions<_IpmsContext> options): base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Claim> Claims { get; set; }

    public virtual DbSet<ClaimDocument> ClaimDocuments { get; set; }

    public virtual DbSet<ClaimStatus> ClaimStatuses { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public virtual DbSet<InsuranceScheme> InsuranceSchemes { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<InsuranceType> InsuranceTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__AuditLog__A17F239815BF9AC0");

            entity.Property(e => e.ActionDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ActionType).HasMaxLength(50);
            entity.Property(e => e.EntityName).HasMaxLength(50);
        });

        modelBuilder.Entity<Claim>(entity =>
        {
            entity.HasKey(e => e.ClaimId).HasName("PK__Claims__EF2E139B028D888E");

            entity.Property(e => e.ClaimAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClaimDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ClaimStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Policy).WithMany(p => p.Claims)
                .HasForeignKey(d => d.PolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Claims__PolicyId__47A6A41B");
        });

        modelBuilder.Entity<ClaimDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__ClaimDoc__1ABEEF0FBE0B36DD");

            entity.Property(e => e.DocumentName).HasMaxLength(200);
            entity.Property(e => e.DocumentType).HasMaxLength(50);
            entity.Property(e => e.UploadedDate).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<ClaimStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__ClaimSta__C8EE20635A85EC85");

            entity.ToTable("ClaimStatus");

            entity.HasIndex(e => e.StatusName, "UQ__ClaimSta__05E7698A90666019").IsUnique();

            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D891FF4DAE");

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D105341E3A0626").IsUnique();

            entity.Property(e => e.AddressLine1).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PhoneNo).HasMaxLength(15);
            entity.Property(e => e.State).HasMaxLength(50);
            entity.Property(e => e.ZipCode).HasMaxLength(10);

            entity.HasOne(d => d.Role).WithMany(p => p.Customers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customers__RoleI__32AB8735");
        });

        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.ErrorId).HasName("PK__ErrorLog__35856A2A541BA66B");

            entity.Property(e => e.LoggedDate).HasDefaultValueSql("(getutcdate())");
        });

        modelBuilder.Entity<InsuranceScheme>(entity =>
        {
            entity.HasKey(e => e.SchemeId).HasName("PK__Insuranc__DB7E1A629F3C00F4");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProfitRatio).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.SchemeName).HasMaxLength(100);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A38FE38ED03");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMode).HasMaxLength(20);

            entity.HasOne(d => d.Policy).WithMany(p => p.Payments)
                .HasForeignKey(d => d.PolicyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Policy__43D61337");
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK__Policies__2E1339A464C59345");

            entity.Property(e => e.MaturityDate).HasColumnType("datetime");
            entity.Property(e => e.PolicyDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PolicyStatus)
                .HasMaxLength(20)
                .HasDefaultValue("Pending");
            entity.Property(e => e.PremiumAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SumAssured).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Policies)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Policies__Custom__3E1D39E1");

            entity.HasOne(d => d.Scheme).WithMany(p => p.Policies)
                .HasForeignKey(d => d.SchemeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Policies__Scheme__3F115E1A");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1ABBBB8892");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160A8A81ECE").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
