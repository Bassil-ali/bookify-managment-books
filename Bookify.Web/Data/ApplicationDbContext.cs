﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Area> Areas { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasSequence<int>("SerialNumber", schema: "shared")
                .StartsAt(1000001);

            builder.Entity<BookCopy>()
                .Property(e => e.SerialNumber)
                .HasDefaultValueSql("NEXT VALUE FOR shared.SerialNumber");

            builder.Entity<BookCategory>().HasKey(e => new { e.BookId, e.CategoryId });

            var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade && !fk.IsOwnership);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            // Seed data for Governorate
            builder.Entity<Governorate>().HasData(
                new Governorate
                {
                    Id = 1,
                    Name = "Example",
                    CreatedById = "138db5fe-7a06-4c85-9a62-b1c6bd936b57",

                }
            ) ;
            

			// Seed data for Area
			builder.Entity<Area>().HasData(
				new Area
				{
                    Id = 1,
					Name = "Example Area",
					GovernorateId = 1,
					CreatedById = "138db5fe-7a06-4c85-9a62-b1c6bd936b57",
				}
			);

			base.OnModelCreating(builder);

			
		}
    }
}