using Microsoft.EntityFrameworkCore;
using RpgGame.Models;

namespace RpgGame.DbContexts
{
    public class GameContext : DbContext
    {
        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
        }
        
        public DbSet<Character> Characters { get; set; }
    }
}