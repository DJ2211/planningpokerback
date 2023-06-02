using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PlanningPoker.Entities.Models;

namespace PlanningPoker.Entities.Data
{
    public partial class Planning_pokerContext : DbContext
    {
        public Planning_pokerContext()
        {
        }

        public Planning_pokerContext(DbContextOptions<Planning_pokerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=PCA156\\SQL2017;DataBase=Planning_poker;User ID=sa;Password=Tatva@123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("games");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK__games__Userid__398D8EEE");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.GameToken)
                    .HasMaxLength(255)
                    .HasColumnName("game_token");

                entity.Property(e => e.IsCardSelected).HasColumnName("isCardSelected");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SelectedCard).HasColumnName("Selected_card");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
