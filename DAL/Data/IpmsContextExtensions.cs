using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DAL.Data
{
    public partial class _IpmsContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Policy>(entity =>
            {
                entity.Property(e => e.InsuranceType).HasMaxLength(20);
                entity.Property(e => e.FullName).HasMaxLength(100);
                entity.Property(e => e.MaritalStatus).HasMaxLength(20);
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.PANCard).HasMaxLength(20);
                entity.Property(e => e.HouseNo).HasMaxLength(50);
                entity.Property(e => e.Street).HasMaxLength(100);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.PinCode).HasMaxLength(10);
                entity.Property(e => e.MedicalHistory).HasMaxLength(500);
                entity.Property(e => e.NomineeName).HasMaxLength(100);
                entity.Property(e => e.NomineeRelationship).HasMaxLength(50);
                entity.Property(e => e.OrganizationName).HasMaxLength(100);
                entity.Property(e => e.PolicyNumber).HasMaxLength(30);
                entity.Property(e => e.Weight).HasColumnType("decimal(5,2)");
            });
            modelBuilder.Entity<InsuranceType>(entity =>
            {
                entity.HasKey(e => e.InsuranceTypeId);
                entity.Property(e => e.TypeName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(200);

                
                entity.HasData(
                    new InsuranceType { InsuranceTypeId = 1, TypeName = "Health", Description = "Health and medical coverage", IsActive = true },
                    new InsuranceType { InsuranceTypeId = 2, TypeName = "Life", Description = "Life and death benefit coverage", IsActive = true },
                    new InsuranceType { InsuranceTypeId = 3, TypeName = "Term", Description = "Fixed term life coverage", IsActive = true },
                    new InsuranceType { InsuranceTypeId = 4, TypeName = "Group", Description = "Group coverage for organizations", IsActive = true }
                );
            });

            modelBuilder.Entity<InsuranceScheme>(entity =>
            {
                entity.HasOne(s => s.InsuranceType)
                      .WithMany(t => t.Schemes)
                      .HasForeignKey(s => s.InsuranceTypeId)
                      .OnDelete(DeleteBehavior.SetNull);   
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentStatus)
                      .HasMaxLength(20)
                      .IsRequired();
            });
        }
    }
}