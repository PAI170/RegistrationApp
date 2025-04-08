using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Models;

namespace RegistrationAPI.Data
{
    public class EmployeesDbContext : DbContext
    {
        public EmployeesDbContext(DbContextOptions<EmployeesDbContext> options)
            : base(options)
        {
        }

        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeImage> EmployeeImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuracion de relaciones y Constrains

            // DocumentType
            modelBuilder.Entity<DocumentType>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.DocumentType)
                .HasForeignKey(e => e.DocumentTypeId);

            // Country
            modelBuilder.Entity<Country>()
                .HasMany(c => c.Provinces)
                .WithOne(p => p.Country)
                .HasForeignKey(p => p.CountryId);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Employees)
                .WithOne(e => e.Country)
                .HasForeignKey(e => e.CountryId);

            modelBuilder.Entity<Country>()
                .HasIndex(c => c.CountryCode)
                .IsUnique();

            // Province
            modelBuilder.Entity<Province>()
                .HasMany(p => p.Cities)
                .WithOne(c => c.Province)
                .HasForeignKey(c => c.ProvinceId);

            modelBuilder.Entity<Province>()
                .HasMany(p => p.Employees)
                .WithOne(e => e.Province)
                .HasForeignKey(e => e.ProvinceId);

            // City
            modelBuilder.Entity<City>()
                .HasMany(c => c.Districts)
                .WithOne(d => d.City)
                .HasForeignKey(d => d.CityId);

            modelBuilder.Entity<City>()
                .HasMany(c => c.Employees)
                .WithOne(e => e.City)
                .HasForeignKey(e => e.CityId);

            // District
            modelBuilder.Entity<District>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.District)
                .HasForeignKey(e => e.DistrictId);

            // Employee
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.EmployeeImages)
                .WithOne(i => i.Employee)
                .HasForeignKey(i => i.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(e => new { e.DocumentTypeId, e.DocumentId })
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(e => new { e.Name, e.LastName });

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.JoinedDate);

            modelBuilder.Entity<Employee>()
                .Property(e => e.CostPerHour)
                .HasColumnType("decimal(10, 2)");
        }
    }
}