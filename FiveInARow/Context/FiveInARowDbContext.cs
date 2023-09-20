using Microsoft.EntityFrameworkCore;
using FiveInARow.Models;

namespace FiveInARow.Context
{
    public class FiveInARowDbContext : DbContext
    {
        public FiveInARowDbContext(DbContextOptions<FiveInARowDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<GameRecord> GameRecords { get; set; }
        public DbSet<UserGameRecord> UserGameRecords { get; set; }

        // Customize models
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGameRecord>()
                .HasKey(ugr => new { ugr.UserId, ugr.GameRecordId });
            modelBuilder.Entity<UserGameRecord>()
                .HasOne(ugr => ugr.User)
                .WithMany(u => u.UserGameRecords)
                .HasForeignKey(ugr => ugr.UserId);
            modelBuilder.Entity<UserGameRecord>()
                .HasOne(ugr => ugr.GameRecord)
                .WithMany(gr => gr.UserGameRecords)
                .HasForeignKey(ugr => ugr.GameRecordId);
            modelBuilder.Entity<GameRecord>()
                .HasKey(gr => gr.Id);
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
        }
    }
}
