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
            // first delete all data
            // db.Categories.RemoveRange(db.Categories);
            // db.EmailOtps.RemoveRange(db.EmailOtps);
            // db.Products.RemoveRange(db.Products);
            // db.CartItems.RemoveRange(db.CartItems);
            // db.Orders.RemoveRange(db.Orders);
            // db.OrderItems.RemoveRange(db.OrderItems);
            // db.Customers.RemoveRange(db.Customers);
            await db.SaveChangesAsync();

            if (!db.Products.Any())
            {
                // fetch from https://dummyjson.com/products
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync("https://dummyjson.com/products?limit=0");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    var dummyData = JsonConvert.DeserializeObject<DummyProductResponse>(jsonString);

                    if (dummyData?.Products != null)
                    {
                        foreach (var item in dummyData.Products)
                        {
                            if (item.Category == null) continue;

                            // Normalize category names to lowercase
                            var category = item.Category.ToLower();

                            // Map categories to broader, clean names
                            string? dbCategory = category switch
                            {
                                "mens-shirts" => "Clothing",
                                "womens-dresses" => "Clothing",
                                "mens-shoes" => "Footwear",
                                "womens-shoes" => "Footwear",
                                "groceries" => "Groceries",
                                "smartphones" => "Smartphones",
                                "sports-accessories" => "Sports",
                                "kitchen-accessories" => "Kitchen",
                                _ => null
                            };

                            if (dbCategory != null)
                            {
                                var product = new Product
                                {
                                    ProductName = item.Title,
                                    ProductDescription = item.Description,
                                    Price = (decimal)item.Price,
                                    CategoryName = dbCategory.ToLower(),
                                    ImageUrl = item.Images?.FirstOrDefault(),
                                    AverageRating = (decimal)item.Rating,
                                    InStock = item.Stock > 0
                                };

                                db.Products.Add(product);
                            }
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