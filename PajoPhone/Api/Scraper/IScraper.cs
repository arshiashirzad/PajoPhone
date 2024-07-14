namespace PajoPhone.Api.Scraper;
public interface IScraper
{
    Task<string>GetPriceAsync(string productName);
}