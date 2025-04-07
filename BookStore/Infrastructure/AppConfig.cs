namespace BookStore.Infrastructure
{
    public class AppConfig
    {
        public Database Database { get; set; } = new Database();
    }

    public class Database
    {
        public string? ConnectionString { get; set; }
    }
}
