using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TelegramJSON2SQLApp.Models
{
    public partial class TelegramDBContext : DbContext
    {
        public TelegramDBContext()
        {
        }

        public TelegramDBContext(DbContextOptions<TelegramDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=TelegramDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Persian_100_CI_AS");

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.Property(e => e.Fname)
                    .HasMaxLength(500)
                    .HasColumnName("FName");

                entity.Property(e => e.Lname)
                    .HasMaxLength(500)
                    .HasColumnName("LName");

                entity.Property(e => e.Phone).HasMaxLength(500);

                entity.Property(e => e.Username).HasMaxLength(500);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
