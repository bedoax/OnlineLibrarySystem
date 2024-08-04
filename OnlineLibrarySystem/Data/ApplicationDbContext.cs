using Microsoft.EntityFrameworkCore;
using OnlineLibrarySystem.Models;

namespace OnlineLibrarySystem.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<User>Users { get; set; }
        public DbSet<Book>Books { get; set; }   
        public DbSet<BorrowedBook>borrowedBooks { get; set; }
        public DbSet<OwnedBook> OwnedBooks { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options) 
        { 

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OwnedBook>()
                .ToTable("OwnedBooks")
                .HasKey(x => x.Id);
            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany() // One Role can have many Users
                .HasForeignKey(u => u.RoleId);

            // Configure Books Table
            modelBuilder.Entity<Book>()
                .ToTable("Books")
                .HasKey(x => x.Id);


            // Configure BorrowedBooks Table
            modelBuilder.Entity<BorrowedBook>()
                .ToTable("BorrowedBooks")
                .HasKey(x => x.Id);

            modelBuilder.Entity<BorrowedBook>()
                .HasOne(b => b.User)
                .WithMany(u => u.BorrowedBooks)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<BorrowedBook>()
                .HasOne(b => b.Book)
                .WithMany(bk => bk.BorrowedBooks)
                .HasForeignKey(b => b.BookId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
