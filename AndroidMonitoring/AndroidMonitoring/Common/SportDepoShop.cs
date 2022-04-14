using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AndroidMonitoring.Common
{
    public class SportDepoShop : IProductPriceReader
    {       
        public async Task<double> GetPriceAsync(string url)
        {           
            string page = await GetPageAsync(url);

            if (string.IsNullOrEmpty(page))
                return -1;

            double price = ParsePrice(page);

            return price;
        }

        private  async Task<string> GetPageAsync(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return string.Empty;

                return await response.Content.ReadAsStringAsync();
            }     
        }

        private double ParsePrice(string page)
        {
            double price = -1;
            Regex priceDivRegex = new Regex(@"<div itemprop=\SlowPrice\S>\d+</div>");


            var math = priceDivRegex.Match(page);
            if (!math.Success)
                return price;

            var mathPrice = Regex.Match(math.Value, @"\d+");
            if (!mathPrice.Success)
                return price;

            double.TryParse(mathPrice.Value, out price);

            return price;
        }
    }
}