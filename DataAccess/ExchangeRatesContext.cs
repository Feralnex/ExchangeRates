using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ExchangeRatesContext : DbContext
    {
        public ExchangeRatesContext(DbContextOptions options) : base(options) { }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<MidRate> MidRates { get; set; }
        public DbSet<TradeRate> TradeRates { get; set; }
        public DbSet<MidExchangeRates> MidExchangeRates { get; set; }
        public DbSet<TradeExchangeRates> TradeExchangeRates { get; set; }
    }
}
