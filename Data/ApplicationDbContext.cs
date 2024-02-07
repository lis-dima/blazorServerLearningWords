using lewBlazorServer.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace lewBlazorServer.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Translation>()
                .HasOne(r => r.Word)
                .WithMany(s => s.Translations)
                .HasForeignKey(r => r.WordId);

            builder.Entity<Description>()
                .HasOne(r => r.Word)
                .WithMany(s => s.Descriptions)
                .HasForeignKey(r => r.WordId);

            builder.Entity<Example>()
                .HasOne(r => r.Word)
                .WithMany(s => s.Examples)
                .HasForeignKey(r => r.WordId);
        }

        public DbSet<Word> Words { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<Example> Examples { get; set; }
        public DbSet<OnStudy> OnStudies { get; set; }
    }
}
