using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicClubManager.Core.Models;
using MusicClubManager.Models;
    
namespace MusicClubManager.Core
{
    public class MusicClubManagerDbContext(DbContextOptions<MusicClubManagerDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Artist> Artists => Set<Artist>();

        public DbSet<Event> Events => Set<Event>();

        public DbSet<Lineup> Lineups => Set<Lineup>();

        public DbSet<Performance> Performances => Set<Performance>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Lineup>()
                .HasOne(l => l.Event)
                .WithMany(e => e.Lineups)
                .HasForeignKey(l => l.EventId)
                .IsRequired(false);

            builder.Entity<Performance>()
                .HasOne(p => p.Artist)
                .WithMany(a => a.Performances)
                .HasForeignKey(p => p.ArtistId)
                .IsRequired(true);

            builder.Entity<Performance>()
                .HasOne(p => p.Lineup)
                .WithMany(l => l.Performances)
                .HasForeignKey(p => p.LineupId)
                .IsRequired(true);

            base.OnModelCreating(builder);
        }
    }
}