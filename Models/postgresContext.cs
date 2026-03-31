using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DotNetEnv;


namespace FilmsArchive.Models
{
    public partial class postgresContext : DbContext
    {
        public postgresContext()
        {
        }

        public postgresContext(DbContextOptions<postgresContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Actor> Actors { get; set; } = null!;
        public virtual DbSet<Movie> Movies { get; set; } = null!;
        public virtual DbSet<Movieactor> Movieactors { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        {


            Env.Load();
            var connectionString = Environment.GetEnvironmentVariable("SUPABASE_STRING");

            if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(connectionString))//чуть чуть отредактировал 
            {
        
                optionsBuilder.UseNpgsql(connectionString); //строка не будет пустая так как подтягивается из env
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Actor>(entity =>
            {
                entity.ToTable("actors");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Birthyear).HasColumnName("birthyear");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("movies");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Genre)
                    .HasMaxLength(100)
                    .HasColumnName("genre");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .HasColumnName("title");

                entity.Property(e => e.Year).HasColumnName("year");
            });

            modelBuilder.Entity<Movieactor>(entity =>
            {
                entity.HasKey(e => new { e.Movieid, e.Actorid })
                    .HasName("movieactors_pkey");

                entity.ToTable("movieactors");

                entity.HasIndex(e => e.Actorid, "idx_movieactors_actorid");

                entity.HasIndex(e => e.Movieid, "idx_movieactors_movieid");

                entity.Property(e => e.Movieid).HasColumnName("movieid");

                entity.Property(e => e.Actorid).HasColumnName("actorid");

                entity.Property(e => e.Role)
                    .HasMaxLength(255)
                    .HasColumnName("role");

                entity.HasOne(d => d.Actor)
                    .WithMany(p => p.Movieactors)
                    .HasForeignKey(d => d.Actorid)
                    .HasConstraintName("movieactors_actorid_fkey");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Movieactors)
                    .HasForeignKey(d => d.Movieid)
                    .HasConstraintName("movieactors_movieid_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
