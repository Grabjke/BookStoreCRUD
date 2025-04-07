using BookStore.Domain.Entities;


namespace BookStore.Domain.Repository.Query
{
    public  class BookQuery
    {
        public string? TitleContains { get; set; }
        public List<int>? GenreId { get; set; }
        public List<int>? AuthorId { get; set; }
        private bool IncludeDeleted { get; set; } = false;
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; } 
        public string? SortBy { get; set; } 
        public bool SortAscending { get; set; } = true;
        public int Page { get; set; } = 1; 
        public int PageSize { get; set; } = 10;


        public IQueryable<Book> Apply(IQueryable<Book> query)
        {
            
            if (!IncludeDeleted)
            {
                query = query.Where(p => !p.IsDeleted);
            }

            
            if (!string.IsNullOrWhiteSpace(TitleContains))
            {
                query = query.Where(p => p.Title.Contains(TitleContains));
            }

            if (GenreId != null && GenreId.Any())
            {
                query = query.Where(p => p.Genres!.Any(g => GenreId.Contains(g.Id)));
            }
            if (AuthorId != null && AuthorId.Any())
            {
                query = query.Where(p => p.Authors!.Any(g => AuthorId.Contains(g.Id)));
            }

            if (MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= MinPrice.Value);
            }

            if (MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= MaxPrice.Value);
            }

            // Сортировка и пагинация
            

            return query;
        }
    }
}
