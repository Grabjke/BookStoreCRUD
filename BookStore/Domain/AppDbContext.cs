using BookStore.Domain.Entities;
using BookStore.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Domain
{
    public class AppDbContext:DbContext
    {
        

      
     
      public  DbSet<Book> Books {  get; set; }
      public  DbSet<Author> Authors {  get; set; }
      public  DbSet<Genre> Genre {  get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        

    }
}
