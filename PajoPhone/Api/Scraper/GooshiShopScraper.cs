using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace PajoPhone.Api.Scraper;

public class GooshiShopScraper: IScraper
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GooshiShopScraper> _logger;
    private readonly IConfiguration _configuration;
    public GooshiShopScraper(HttpClient httpClient, ILogger<GooshiShopScraper> logger,IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }
    public  async Task<string> GetPriceAsync(string productName)
    {
        string url =_configuration["ScrapingSettings:gooshiShopUrl"]+ productName;
        try
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc =await web.LoadFromWebAsync(url);
            var spanNode = doc.DocumentNode.SelectSingleNode("//span[contains(@class, 'price actual-price h4 mb-0')]"); 
            string innerHtml = spanNode.InnerHtml.Trim();
            string price = Regex.Replace(innerHtml, "[^0-9]", "");
            return price;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching prices for product '{productName}': {ex.Message}");
        }
        return null!;
    }
}