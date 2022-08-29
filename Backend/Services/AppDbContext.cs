using Microsoft.EntityFrameworkCore;
using MTGCC.Database;

namespace MTGCC.Services
{
    /// <summary>
    /// Represents the context for the application's database.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// A table containing users.
        /// </summary>
        public DbSet<AppUser> Users { get; set; }

        /// <summary>
        /// A table containing player owned cards.
        /// </summary>
        public DbSet<PlayerCard> PlayerCards { get; set; }

        /// <summary>
        /// A table containing player owned decks.
        /// </summary>
        public DbSet<Deck> Decks { get; set; }

        /// <summary>
        /// A table containing cards within decks.
        /// </summary>
        public DbSet<DeckCard> DeckCards { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeckCard>().HasKey(o => new { o.DeckID, o.ID });

            modelBuilder.Entity<DeckCard>().HasOne(e => e.PlayerCard).WithOne().OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
