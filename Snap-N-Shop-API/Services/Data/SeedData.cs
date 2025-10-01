using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Snap_N_Shop_API.Data;
using Snap_N_Shop_API.Models;
using System.Net.Http;


namespace Snap_N_Shop_API.Services.Data
{
    public static class SeedData
    {
        public static async Task Seed(MyDbContext db)
        {
            if (!db.Products.Any())
            {
                // fetch from https://dummyjson.com/products
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://dummyjson.com/products");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    var dummyData = JsonConvert.DeserializeObject<DummyProductResponse>(jsonString);
                    if (dummyData?.Products != null)
                    {
                        foreach (var item in dummyData.Products)
                        {
                            var product = new Product
                            {
                                ProductName = item.Title,
                                ProductDescription = item.Description,
                                Price = (decimal)item.Price,
                                CategoryName = item.Category,
                                ImageUrl = item.Images?.FirstOrDefault(),
                                AverageRating = (decimal)item.Rating,
                                InStock = item.Stock > 0
                            };
                            db.Products.Add(product);
                        }
                        await db.SaveChangesAsync();
                        Console.WriteLine($"Seeded {dummyData.Products.Count} products");
                    }
                }
            }
        }
    }

    public class DummyProductResponse
    {
        [JsonProperty("products")]
        public List<DummyProduct>? Products { get; set; }
    }

    public class DummyProduct
    {
        [JsonProperty("title")]
        public string Title { get; set; } = null!;

        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; } = null!;

        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("stock")]
        public int Stock { get; set; }

        [JsonProperty("images")]
        public List<string>? Images { get; set; }
    }
}