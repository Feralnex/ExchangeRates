using DataAccess;
using DataAccess.Models;
using ExchangeRates.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Networking.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IDownloader _downloader;
        private readonly EndPoints _endPoints;
        private readonly ExchangeRatesContext _db;

        public List<MidExchangeRates> MidExchangeRates { get; set; }
        public List<TradeExchangeRates> TradeExchangeRates { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IDownloader downloader, IOptions<EndPoints> endPoints, ExchangeRatesContext db)
        {
            _logger = logger;
            _downloader = downloader;
            _endPoints = endPoints.Value;
            _db = db;

            MidExchangeRates = new List<MidExchangeRates>();
            TradeExchangeRates = new List<TradeExchangeRates>();
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                foreach (string source in _endPoints.MidExchangeRates)
                {
                    MidExchangeRates.AddRange(await _downloader.Download<MidExchangeRates[]>(source, Encoding.UTF8));
                }

                foreach (string source in _endPoints.TradeExchangeRates)
                {
                    TradeExchangeRates.AddRange(await _downloader.Download<TradeExchangeRates[]>(source, Encoding.UTF8));
                }

                OnExchangeRatesDownloaded();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
            }

            return Page();
        }

        private async Task OnExchangeRatesDownloaded()
        {
            IEnumerable<MidExchangeRates> midExchangeRatesToSave = MidExchangeRates.Where(exchangeRates => !_db.MidExchangeRates.Any(table => table.No == exchangeRates.No));
            IEnumerable<TradeExchangeRates> tradeExchangeRatesToSave = TradeExchangeRates.Where(exchangeRates => !_db.TradeExchangeRates.Any(table => table.No == exchangeRates.No));

            try
            {
                foreach (MidExchangeRates exchangeRates in midExchangeRatesToSave)
                {
                    await Save(exchangeRates);
                }

                foreach (TradeExchangeRates exchangeRates in tradeExchangeRatesToSave)
                {
                    await Save(exchangeRates);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
            }
        }

        private async Task Save(MidExchangeRates exchangeRates)
        {
            await AssignExistingCurrencies(exchangeRates.Rates);

            _db.MidExchangeRates.Add(exchangeRates);
            _db.SaveChanges();
        }

        private async Task Save(TradeExchangeRates exchangeRates)
        {
            await AssignExistingCurrencies(exchangeRates.Rates);

            _db.TradeExchangeRates.Add(exchangeRates);
            _db.SaveChanges();
        }

        private async Task AssignExistingCurrencies(IEnumerable<Rate> rates)
        {
            foreach (Rate rate in rates)
            {
                rate.Currency = await _db.Currencies.FindAsync(rate.Currency.Code) ?? rate.Currency;
            }
        }
    }
}
