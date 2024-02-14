using lewBlazorServer.Data.Entities;
using lewBlazorServer.Data.Interfaces;
using lewBlazorServer.Services.Helpers;
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

        public override int SaveChanges()
        {
            PerformAfterDeleteActions();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            PerformAfterDeleteActions();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void PerformAfterDeleteActions()
        {
            var deletedEntities = ChangeTracker.Entries()
                .Where(entry => entry.State == EntityState.Deleted)
                .Select(entry => entry.Entity)
                .ToList();

            foreach (var entity in deletedEntities)
            {
                switch (entity)
                {
                    case Word word:
                        Storage.DeleteAudio(Storage.GetAudioPath(EntityType.Word, word.Id, false));
                        break;
                    case IWordChildEntity iWordChildEntity:
                        // Example, Description, Translation
                        Storage.DeleteAudio(Storage.GetAudioPath(iWordChildEntity.WordChildType, iWordChildEntity.Id, false));
                        break;
                }
            }
        }

        public DbSet<Word> Words { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<Description> Descriptions { get; set; }
        public DbSet<Example> Examples { get; set; }
        public DbSet<OnStudy> OnStudies { get; set; }
    }
}
