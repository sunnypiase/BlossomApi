using BlossomApi.Models;
using System.Text.Json;

namespace BlossomApi.Seeders
{
    public static class DatabaseProductSeeder
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "Product 1",
                    NameEng = "Product 1 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand B",
                    Price = 847.64m,
                    Discount = 40.17m,
                    IsNew = true,
                    Rating = 3.0,
                    InStock = false,
                    AvailableAmount = 32,
                    NumberOfReviews = 83,
                    NumberOfPurchases = 527,
                    NumberOfViews = 613,
                    Article = "133067",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 9, 5 }),
                    Description = "Description of Product 1"
                },

                new Product
                {
                    ProductId = 2,
                    Name = "Product 2",
                    NameEng = "Product 2 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand C",
                    Price = 380.04m,
                    Discount = 20.22m,
                    IsNew = true,
                    Rating = 4.3,
                    InStock = false,
                    AvailableAmount = 72,
                    NumberOfReviews = 59,
                    NumberOfPurchases = 789,
                    NumberOfViews = 541,
                    Article = "722566",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 6 }),
                    Description = "Description of Product 2",
                },

                new Product
                {
                    ProductId = 3,
                    Name = "Product 3",
                    NameEng = "Product 3 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand D",
                    Price = 311.66m,
                    Discount = 22.85m,
                    IsNew = false,
                    Rating = 4.8,
                    InStock = true,
                    AvailableAmount = 62,
                    NumberOfReviews = 220,
                    NumberOfPurchases = 790,
                    NumberOfViews = 372,
                    Article = "156683",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 1 }),
                    Description = "Description of Product 3",
                },

                new Product
                {
                    ProductId = 4,
                    Name = "Product 4",
                    NameEng = "Product 4 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand E",
                    Price = 424.1m,
                    Discount = 2.22m,
                    IsNew = false,
                    Rating = 2.2,
                    InStock = false,
                    AvailableAmount = 55,
                    NumberOfReviews = 368,
                    NumberOfPurchases = 880,
                    NumberOfViews = 1243,
                    Article = "358132",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 1, 8, 9 }),
                    Description = "Description of Product 4",
                },

                new Product
                {
                    ProductId = 5,
                    Name = "Product 5",
                    NameEng = "Product 5 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand F",
                    Price = 549.77m,
                    Discount = 22.83m,
                    IsNew = true,
                    Rating = 2.2,
                    InStock = true,
                    AvailableAmount = 44,
                    NumberOfReviews = 179,
                    NumberOfPurchases = 958,
                    NumberOfViews = 369,
                    Article = "284004",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 8, 5, 6, 9 }),
                    Description = "Description of Product 5",
                },

                new Product
                {
                    ProductId = 6,
                    Name = "Product 6",
                    NameEng = "Product 6 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand G",
                    Price = 675.98m,
                    Discount = 25.93m,
                    IsNew = false,
                    Rating = 4.3,
                    InStock = true,
                    AvailableAmount = 13,
                    NumberOfReviews = 286,
                    NumberOfPurchases = 206,
                    NumberOfViews = 3,
                    Article = "932122",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 6, 9, 10, 1 }),
                    Description = "Description of Product 6",
                },

                new Product
                {
                    ProductId = 7,
                    Name = "Product 7",
                    NameEng = "Product 7 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand H",
                    Price = 207.68m,
                    Discount = 35.1m,
                    IsNew = false,
                    Rating = 1.9,
                    InStock = false,
                    AvailableAmount = 32,
                    NumberOfReviews = 310,
                    NumberOfPurchases = 292,
                    NumberOfViews = 1286,
                    Article = "440052",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5, 9, 9 }),
                    Description = "Description of Product 7",
                },

                new Product
                {
                    ProductId = 8,
                    Name = "Product 8",
                    NameEng = "Product 8 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand I",
                    Price = 501.77m,
                    Discount = 22.34m,
                    IsNew = true,
                    Rating = 4.8,
                    InStock = true,
                    AvailableAmount = 7,
                    NumberOfReviews = 159,
                    NumberOfPurchases = 652,
                    NumberOfViews = 119,
                    Article = "150689",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 8, 1, 6, 4 }),
                    Description = "Description of Product 8",
                },

                new Product
                {
                    ProductId = 9,
                    Name = "Product 9",
                    NameEng = "Product 9 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand J",
                    Price = 296.62m,
                    Discount = 32.42m,
                    IsNew = false,
                    Rating = 2.5,
                    InStock = true,
                    AvailableAmount = 20,
                    NumberOfReviews = 407,
                    NumberOfPurchases = 963,
                    NumberOfViews = 1332,
                    Article = "625433",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 4 }),
                    Description = "Description of Product 9",
                },

                new Product
                {
                    ProductId = 10,
                    Name = "Product 10",
                    NameEng = "Product 10 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand K",
                    Price = 262.41m,
                    Discount = 40.75m,
                    IsNew = true,
                    Rating = 2.9,
                    InStock = false,
                    AvailableAmount = 16,
                    NumberOfReviews = 377,
                    NumberOfPurchases = 552,
                    NumberOfViews = 934,
                    Article = "824675",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 1 }),
                    Description = "Description of Product 10",
                },

                new Product
                {
                    ProductId = 11,
                    Name = "Product 11",
                    NameEng = "Product 11 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand L",
                    Price = 900.23m,
                    Discount = 31.03m,
                    IsNew = false,
                    Rating = 4.0,
                    InStock = false,
                    AvailableAmount = 74,
                    NumberOfReviews = 348,
                    NumberOfPurchases = 914,
                    NumberOfViews = 1934,
                    Article = "121158",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5, 1, 8, 7, 2 }),
                    Description = "Description of Product 11",
                },

                new Product
                {
                    ProductId = 12,
                    Name = "Product 12",
                    NameEng = "Product 12 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand M",
                    Price = 509.98m,
                    Discount = 42.19m,
                    IsNew = true,
                    Rating = 3.5,
                    InStock = true,
                    AvailableAmount = 49,
                    NumberOfReviews = 26,
                    NumberOfPurchases = 246,
                    NumberOfViews = 577,
                    Article = "235436",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 10, 2, 9 }),
                    Description = "Description of Product 12",
                },

                new Product
                {
                    ProductId = 13,
                    Name = "Product 13",
                    NameEng = "Product 13 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand N",
                    Price = 745.86m,
                    Discount = 10.75m,
                    IsNew = false,
                    Rating = 2.4,
                    InStock = false,
                    AvailableAmount = 94,
                    NumberOfReviews = 40,
                    NumberOfPurchases = 297,
                    NumberOfViews = 1904,
                    Article = "119997",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 4 }),
                    Description = "Description of Product 13",
                },

                new Product
                {
                    ProductId = 14,
                    Name = "Product 14",
                    NameEng = "Product 14 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand O",
                    Price = 620.38m,
                    Discount = 42.53m,
                    IsNew = false,
                    Rating = 4.5,
                    InStock = false,
                    AvailableAmount = 96,
                    NumberOfReviews = 485,
                    NumberOfPurchases = 665,
                    NumberOfViews = 1246,
                    Article = "135476",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 9, 10, 6, 5 }),
                    Description = "Description of Product 14",
                },

                new Product
                {
                    ProductId = 15,
                    Name = "Product 15",
                    NameEng = "Product 15 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand P",
                    Price = 466.04m,
                    Discount = 34.06m,
                    IsNew = true,
                    Rating = 3.3,
                    InStock = true,
                    AvailableAmount = 33,
                    NumberOfReviews = 457,
                    NumberOfPurchases = 797,
                    NumberOfViews = 166,
                    Article = "357402",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 8 }),
                    Description = "Description of Product 15",
                },

                new Product
                {
                    ProductId = 16,
                    Name = "Product 16",
                    NameEng = "Product 16 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand Q",
                    Price = 153.03m,
                    Discount = 36.9m,
                    IsNew = true,
                    Rating = 4.8,
                    InStock = false,
                    AvailableAmount = 96,
                    NumberOfReviews = 454,
                    NumberOfPurchases = 475,
                    NumberOfViews = 463,
                    Article = "310170",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 4, 7 }),
                    Description = "Description of Product 16",
                },

                new Product
                {
                    ProductId = 17,
                    Name = "Product 17",
                    NameEng = "Product 17 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand R",
                    Price = 249.65m,
                    Discount = 34.59m,
                    IsNew = true,
                    Rating = 3.7,
                    InStock = true,
                    AvailableAmount = 34,
                    NumberOfReviews = 371,
                    NumberOfPurchases = 509,
                    NumberOfViews = 1044,
                    Article = "869692",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 2, 3, 7, 4 }),
                    Description = "Description of Product 17",
                },

                new Product
                {
                    ProductId = 18,
                    Name = "Product 18",
                    NameEng = "Product 18 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand S",
                    Price = 133.26m,
                    Discount = 27.48m,
                    IsNew = false,
                    Rating = 4.9,
                    InStock = false,
                    AvailableAmount = 49,
                    NumberOfReviews = 423,
                    NumberOfPurchases = 646,
                    NumberOfViews = 664,
                    Article = "179439",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 6, 8, 4, 2, 1 }),
                    Description = "Description of Product 18",
                },

                new Product
                {
                    ProductId = 19,
                    Name = "Product 19",
                    NameEng = "Product 19 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand T",
                    Price = 638.26m,
                    Discount = 20.95m,
                    IsNew = false,
                    Rating = 3.3,
                    InStock = true,
                    AvailableAmount = 87,
                    NumberOfReviews = 180,
                    NumberOfPurchases = 651,
                    NumberOfViews = 137,
                    Article = "794901",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 10 }),
                    Description = "Description of Product 19",
                },

                new Product
                {
                    ProductId = 20,
                    Name = "Product 20",
                    NameEng = "Product 20 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand U",
                    Price = 271.72m,
                    Discount = 5.41m,
                    IsNew = false,
                    Rating = 3.6,
                    InStock = false,
                    AvailableAmount = 29,
                    NumberOfReviews = 276,
                    NumberOfPurchases = 842,
                    NumberOfViews = 123,
                    Article = "595606",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 3, 5, 9, 5 }),
                    Description = "Description of Product 20",
                },

                new Product
                {
                    ProductId = 21,
                    Name = "Product 21",
                    NameEng = "Product 21 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand V",
                    Price = 630.71m,
                    Discount = 1.84m,
                    IsNew = false,
                    Rating = 3.9,
                    InStock = true,
                    AvailableAmount = 50,
                    NumberOfReviews = 90,
                    NumberOfPurchases = 415,
                    NumberOfViews = 694,
                    Article = "594472",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 2, 8 }),
                    Description = "Description of Product 21",
                },

                new Product
                {
                    ProductId = 22,
                    Name = "Product 22",
                    NameEng = "Product 22 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand W",
                    Price = 229.37m,
                    Discount = 43.63m,
                    IsNew = false,
                    Rating = 4.9,
                    InStock = false,
                    AvailableAmount = 56,
                    NumberOfReviews = 202,
                    NumberOfPurchases = 100,
                    NumberOfViews = 426,
                    Article = "809558",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5, 5 }),
                    Description = "Description of Product 22",
                },

                new Product
                {
                    ProductId = 23,
                    Name = "Product 23",
                    NameEng = "Product 23 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand X",
                    Price = 783.8m,
                    Discount = 26.46m,
                    IsNew = true,
                    Rating = 3.8,
                    InStock = true,
                    AvailableAmount = 98,
                    NumberOfReviews = 336,
                    NumberOfPurchases = 593,
                    NumberOfViews = 268,
                    Article = "250826",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5, 5, 3 }),
                    Description = "Description of Product 23",
                },

                new Product
                {
                    ProductId = 24,
                    Name = "Product 24",
                    NameEng = "Product 24 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand Y",
                    Price = 436.87m,
                    Discount = 39.42m,
                    IsNew = true,
                    Rating = 4.2,
                    InStock = false,
                    AvailableAmount = 46,
                    NumberOfReviews = 56,
                    NumberOfPurchases = 358,
                    NumberOfViews = 1992,
                    Article = "537870",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 7, 5, 4, 3, 2 }),
                    Description = "Description of Product 24",
                },

                new Product
                {
                    ProductId = 25,
                    Name = "Product 25",
                    NameEng = "Product 25 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand Z",
                    Price = 909.94m,
                    Discount = 48.11m,
                    IsNew = false,
                    Rating = 3.7,
                    InStock = true,
                    AvailableAmount = 1,
                    NumberOfReviews = 191,
                    NumberOfPurchases = 927,
                    NumberOfViews = 326,
                    Article = "380078",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 6, 5 }),
                    Description = "Description of Product 25",
                },

                new Product
                {
                    ProductId = 26,
                    Name = "Product 26",
                    NameEng = "Product 26 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand A",
                    Price = 242.13m,
                    Discount = 36.95m,
                    IsNew = true,
                    Rating = 2.2,
                    InStock = true,
                    AvailableAmount = 13,
                    NumberOfReviews = 455,
                    NumberOfPurchases = 939,
                    NumberOfViews = 1589,
                    Article = "903038",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 9, 4, 5 }),
                    Description = "Description of Product 26",
                },

                new Product
                {
                    ProductId = 27,
                    Name = "Product 27",
                    NameEng = "Product 27 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand B",
                    Price = 540.28m,
                    Discount = 17.17m,
                    IsNew = false,
                    Rating = 4.3,
                    InStock = false,
                    AvailableAmount = 100,
                    NumberOfReviews = 411,
                    NumberOfPurchases = 834,
                    NumberOfViews = 1794,
                    Article = "747515",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 8 }),
                    Description = "Description of Product 27",
                },

                new Product
                {
                    ProductId = 28,
                    Name = "Product 28",
                    NameEng = "Product 28 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand C",
                    Price = 191.65m,
                    Discount = 22.78m,
                    IsNew = false,
                    Rating = 1.6,
                    InStock = true,
                    AvailableAmount = 90,
                    NumberOfReviews = 438,
                    NumberOfPurchases = 669,
                    NumberOfViews = 826,
                    Article = "975176",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 3, 3, 5, 10 }),
                    Description = "Description of Product 28",
                },

                new Product
                {
                    ProductId = 29,
                    Name = "Product 29",
                    NameEng = "Product 29 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand D",
                    Price = 386.9m,
                    Discount = 3.3m,
                    IsNew = true,
                    Rating = 2.3,
                    InStock = false,
                    AvailableAmount = 87,
                    NumberOfReviews = 249,
                    NumberOfPurchases = 75,
                    NumberOfViews = 262,
                    Article = "907604",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 9, 10 }),
                    Description = "Description of Product 29",
                },

                new Product
                {
                    ProductId = 30,
                    Name = "Product 30",
                    NameEng = "Product 30 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand E",
                    Price = 231.5m,
                    Discount = 10.56m,
                    IsNew = true,
                    Rating = 1.0,
                    InStock = true,
                    AvailableAmount = 15,
                    NumberOfReviews = 314,
                    NumberOfPurchases = 727,
                    NumberOfViews = 424,
                    Article = "630632",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 1, 10, 4, 2 }),
                    Description = "Description of Product 30",
                },

                new Product
                {
                    ProductId = 31,
                    Name = "Product 31",
                    NameEng = "Product 31 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand F",
                    Price = 570.08m,
                    Discount = 47.32m,
                    IsNew = true,
                    Rating = 4.0,
                    InStock = true,
                    AvailableAmount = 33,
                    NumberOfReviews = 216,
                    NumberOfPurchases = 392,
                    NumberOfViews = 1107,
                    Article = "505239",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 6, 3, 3 }),
                    Description = "Description of Product 31",
                },

                new Product
                {
                    ProductId = 32,
                    Name = "Product 32",
                    NameEng = "Product 32 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand G",
                    Price = 373.75m,
                    Discount = 23.47m,
                    IsNew = true,
                    Rating = 1.9,
                    InStock = false,
                    AvailableAmount = 24,
                    NumberOfReviews = 318,
                    NumberOfPurchases = 367,
                    NumberOfViews = 939,
                    Article = "206022",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 7, 10, 3 }),
                    Description = "Description of Product 32",
                },

                new Product
                {
                    ProductId = 33,
                    Name = "Product 33",
                    NameEng = "Product 33 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand H",
                    Price = 610.84m,
                    Discount = 5.64m,
                    IsNew = true,
                    Rating = 1.6,
                    InStock = true,
                    AvailableAmount = 4,
                    NumberOfReviews = 110,
                    NumberOfPurchases = 942,
                    NumberOfViews = 749,
                    Article = "408787",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 6, 10, 10 }),
                    Description = "Description of Product 33",
                },

                new Product
                {
                    ProductId = 34,
                    Name = "Product 34",
                    NameEng = "Product 34 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand I",
                    Price = 504.86m,
                    Discount = 30.8m,
                    IsNew = false,
                    Rating = 4.3,
                    InStock = false,
                    AvailableAmount = 45,
                    NumberOfReviews = 432,
                    NumberOfPurchases = 6,
                    NumberOfViews = 8,
                    Article = "297818",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 7, 7, 5, 3 }),
                    Description = "Description of Product 34",
                },

                new Product
                {
                    ProductId = 35,
                    Name = "Product 35",
                    NameEng = "Product 35 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand J",
                    Price = 326.07m,
                    Discount = 18.58m,
                    IsNew = false,
                    Rating = 1.8,
                    InStock = true,
                    AvailableAmount = 47,
                    NumberOfReviews = 376,
                    NumberOfPurchases = 32,
                    NumberOfViews = 1301,
                    Article = "263453",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 1, 9 }),
                    Description = "Description of Product 35",
                },

                new Product
                {
                    ProductId = 36,
                    Name = "Product 36",
                    NameEng = "Product 36 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand K",
                    Price = 329.32m,
                    Discount = 1.21m,
                    IsNew = false,
                    Rating = 3.8,
                    InStock = true,
                    AvailableAmount = 45,
                    NumberOfReviews = 36,
                    NumberOfPurchases = 98,
                    NumberOfViews = 1111,
                    Article = "975500",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 7, 4 }),
                    Description = "Description of Product 36",
                },

                new Product
                {
                    ProductId = 37,
                    Name = "Product 37",
                    NameEng = "Product 37 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand L",
                    Price = 870.68m,
                    Discount = 9.54m,
                    IsNew = false,
                    Rating = 3.7,
                    InStock = false,
                    AvailableAmount = 85,
                    NumberOfReviews = 18,
                    NumberOfPurchases = 130,
                    NumberOfViews = 1518,
                    Article = "753598",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 1 }),
                    Description = "Description of Product 37",
                },

                new Product
                {
                    ProductId = 38,
                    Name = "Product 38",
                    NameEng = "Product 38 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand M",
                    Price = 957.17m,
                    Discount = 7.17m,
                    IsNew = false,
                    Rating = 3.1,
                    InStock = false,
                    AvailableAmount = 86,
                    NumberOfReviews = 400,
                    NumberOfPurchases = 181,
                    NumberOfViews = 1602,
                    Article = "545207",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 2, 1, 8 }),
                    Description = "Description of Product 38",
                },

                new Product
                {
                    ProductId = 39,
                    Name = "Product 39",
                    NameEng = "Product 39 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand N",
                    Price = 994.86m,
                    Discount = 22.06m,
                    IsNew = true,
                    Rating = 1.3,
                    InStock = true,
                    AvailableAmount = 11,
                    NumberOfReviews = 282,
                    NumberOfPurchases = 341,
                    NumberOfViews = 1184,
                    Article = "431814",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 6, 5, 1, 7 }),
                    Description = "Description of Product 39",
                },

                new Product
                {
                    ProductId = 40,
                    Name = "Product 40",
                    NameEng = "Product 40 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand O",
                    Price = 526.28m,
                    Discount = 14.16m,
                    IsNew = false,
                    Rating = 2.6,
                    InStock = false,
                    AvailableAmount = 99,
                    NumberOfReviews = 5,
                    NumberOfPurchases = 296,
                    NumberOfViews = 1958,
                    Article = "917212",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 8, 1, 4, 8, 4 }),
                    Description = "Description of Product 40",
                },

                new Product
                {
                    ProductId = 41,
                    Name = "Product 41",
                    NameEng = "Product 41 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand P",
                    Price = 530.31m,
                    Discount = 36.54m,
                    IsNew = true,
                    Rating = 4.4,
                    InStock = false,
                    AvailableAmount = 17,
                    NumberOfReviews = 366,
                    NumberOfPurchases = 910,
                    NumberOfViews = 619,
                    Article = "592719",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5, 7, 5, 2, 3 }),
                    Description = "Description of Product 41",
                },

                new Product
                {
                    ProductId = 42,
                    Name = "Product 42",
                    NameEng = "Product 42 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand Q",
                    Price = 710.99m,
                    Discount = 26.76m,
                    IsNew = true,
                    Rating = 4.9,
                    InStock = false,
                    AvailableAmount = 49,
                    NumberOfReviews = 259,
                    NumberOfPurchases = 206,
                    NumberOfViews = 1114,
                    Article = "865534",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 8 }),
                    Description = "Description of Product 42",
                },

                new Product
                {
                    ProductId = 43,
                    Name = "Product 43",
                    NameEng = "Product 43 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand R",
                    Price = 975.55m,
                    Discount = 49.61m,
                    IsNew = false,
                    Rating = 1.2,
                    InStock = false,
                    AvailableAmount = 53,
                    NumberOfReviews = 475,
                    NumberOfPurchases = 81,
                    NumberOfViews = 524,
                    Article = "669644",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 10, 1, 1, 7, 6 }),
                    Description = "Description of Product 43",
                },

                new Product
                {
                    ProductId = 44,
                    Name = "Product 44",
                    NameEng = "Product 44 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand S",
                    Price = 678.07m,
                    Discount = 40.48m,
                    IsNew = false,
                    Rating = 3.3,
                    InStock = true,
                    AvailableAmount = 18,
                    NumberOfReviews = 125,
                    NumberOfPurchases = 146,
                    NumberOfViews = 393,
                    Article = "203037",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5, 5, 9, 5 }),
                    Description = "Description of Product 44",
                },

                new Product
                {
                    ProductId = 45,
                    Name = "Product 45",
                    NameEng = "Product 45 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand T",
                    Price = 116.09m,
                    Discount = 46.42m,
                    IsNew = true,
                    Rating = 2.6,
                    InStock = false,
                    AvailableAmount = 46,
                    NumberOfReviews = 409,
                    NumberOfPurchases = 37,
                    NumberOfViews = 602,
                    Article = "756833",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 2 }),
                    Description = "Description of Product 45",
                },

                new Product
                {
                    ProductId = 46,
                    Name = "Product 46",
                    NameEng = "Product 46 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand U",
                    Price = 622.51m,
                    Discount = 3.16m,
                    IsNew = false,
                    Rating = 2.6,
                    InStock = false,
                    AvailableAmount = 73,
                    NumberOfReviews = 114,
                    NumberOfPurchases = 725,
                    NumberOfViews = 624,
                    Article = "276912",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5, 5 }),
                    Description = "Description of Product 46",
                },

                new Product
                {
                    ProductId = 47,
                    Name = "Product 47",
                    NameEng = "Product 47 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand V",
                    Price = 555.7m,
                    Discount = 1.24m,
                    IsNew = true,
                    Rating = 2.9,
                    InStock = false,
                    AvailableAmount = 87,
                    NumberOfReviews = 356,
                    NumberOfPurchases = 871,
                    NumberOfViews = 997,
                    Article = "292706",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 6, 10, 5 }),
                    Description = "Description of Product 47",
                },

                new Product
                {
                    ProductId = 48,
                    Name = "Product 48",
                    NameEng = "Product 48 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand W",
                    Price = 717.66m,
                    Discount = 18.22m,
                    IsNew = false,
                    Rating = 1.5,
                    InStock = false,
                    AvailableAmount = 44,
                    NumberOfReviews = 457,
                    NumberOfPurchases = 385,
                    NumberOfViews = 1285,
                    Article = "803478",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 1 }),
                    Description = "Description of Product 48",
                },

                new Product
                {
                    ProductId = 49,
                    Name = "Product 49",
                    NameEng = "Product 49 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand X",
                    Price = 451.35m,
                    Discount = 49.49m,
                    IsNew = true,
                    Rating = 1.3,
                    InStock = false,
                    AvailableAmount = 66,
                    NumberOfReviews = 206,
                    NumberOfPurchases = 14,
                    NumberOfViews = 650,
                    Article = "135499",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 2 }),
                    Description = "Description of Product 49",
                },

                new Product
                {
                    ProductId = 50,
                    Name = "Product 50",
                    NameEng = "Product 50 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand Y",
                    Price = 501.77m,
                    Discount = 41.66m,
                    IsNew = false,
                    Rating = 3.8,
                    InStock = true,
                    AvailableAmount = 6,
                    NumberOfReviews = 5,
                    NumberOfPurchases = 129,
                    NumberOfViews = 1174,
                    Article = "158261",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 8, 2, 7 }),
                    Description = "Description of Product 50",
                },

                new Product
                {
                    ProductId = 51,
                    Name = "Product 51",
                    NameEng = "Product 51 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand Z",
                    Price = 894.02m,
                    Discount = 28.25m,
                    IsNew = true,
                    Rating = 3.1,
                    InStock = true,
                    AvailableAmount = 63,
                    NumberOfReviews = 78,
                    NumberOfPurchases = 464,
                    NumberOfViews = 1775,
                    Article = "857109",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 2, 10, 1 }),
                    Description = "Description of Product 51",
                },

                new Product
                {
                    ProductId = 52,
                    Name = "Product 52",
                    NameEng = "Product 52 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand A",
                    Price = 317.81m,
                    Discount = 16.87m,
                    IsNew = false,
                    Rating = 2.5,
                    InStock = true,
                    AvailableAmount = 44,
                    NumberOfReviews = 455,
                    NumberOfPurchases = 70,
                    NumberOfViews = 1553,
                    Article = "507988",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 7, 2 }),
                    Description = "Description of Product 52",
                },

                new Product
                {
                    ProductId = 53,
                    Name = "Product 53",
                    NameEng = "Product 53 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand B",
                    Price = 174.4m,
                    Discount = 34.21m,
                    IsNew = true,
                    Rating = 2.1,
                    InStock = true,
                    AvailableAmount = 10,
                    NumberOfReviews = 206,
                    NumberOfPurchases = 272,
                    NumberOfViews = 558,
                    Article = "597985",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 8, 7 }),
                    Description = "Description of Product 53",
                },

                new Product
                {
                    ProductId = 54,
                    Name = "Product 54",
                    NameEng = "Product 54 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand C",
                    Price = 985.6m,
                    Discount = 37.98m,
                    IsNew = false,
                    Rating = 4.4,
                    InStock = true,
                    AvailableAmount = 60,
                    NumberOfReviews = 198,
                    NumberOfPurchases = 962,
                    NumberOfViews = 34,
                    Article = "982932",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 8, 9, 10 }),
                    Description = "Description of Product 54",
                },

                new Product
                {
                    ProductId = 55,
                    Name = "Product 55",
                    NameEng = "Product 55 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand D",
                    Price = 903.81m,
                    Discount = 4.03m,
                    IsNew = false,
                    Rating = 1.4,
                    InStock = true,
                    AvailableAmount = 10,
                    NumberOfReviews = 291,
                    NumberOfPurchases = 73,
                    NumberOfViews = 1969,
                    Article = "626398",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 8 }),
                    Description = "Description of Product 55",
                },

                new Product
                {
                    ProductId = 56,
                    Name = "Product 56",
                    NameEng = "Product 56 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand E",
                    Price = 221.42m,
                    Discount = 38.39m,
                    IsNew = true,
                    Rating = 1.7,
                    InStock = false,
                    AvailableAmount = 72,
                    NumberOfReviews = 108,
                    NumberOfPurchases = 285,
                    NumberOfViews = 1524,
                    Article = "527511",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 4, 7, 9, 2, 3 }),
                    Description = "Description of Product 56",
                },

                new Product
                {
                    ProductId = 57,
                    Name = "Product 57",
                    NameEng = "Product 57 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand F",
                    Price = 307.29m,
                    Discount = 18.11m,
                    IsNew = false,
                    Rating = 2.1,
                    InStock = true,
                    AvailableAmount = 68,
                    NumberOfReviews = 244,
                    NumberOfPurchases = 759,
                    NumberOfViews = 176,
                    Article = "498833",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 7, 3 }),
                    Description = "Description of Product 57",
                },

                new Product
                {
                    ProductId = 58,
                    Name = "Product 58",
                    NameEng = "Product 58 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand G",
                    Price = 696.74m,
                    Discount = 31.47m,
                    IsNew = false,
                    Rating = 4.6,
                    InStock = true,
                    AvailableAmount = 48,
                    NumberOfReviews = 173,
                    NumberOfPurchases = 312,
                    NumberOfViews = 1658,
                    Article = "424356",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 7, 9, 8, 1 }),
                    Description = "Description of Product 58",
                },

                new Product
                {
                    ProductId = 59,
                    Name = "Product 59",
                    NameEng = "Product 59 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand H",
                    Price = 140.85m,
                    Discount = 12.81m,
                    IsNew = false,
                    Rating = 4.4,
                    InStock = false,
                    AvailableAmount = 43,
                    NumberOfReviews = 90,
                    NumberOfPurchases = 639,
                    NumberOfViews = 976,
                    Article = "762427",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5, 9 }),
                    Description = "Description of Product 59",
                },

                new Product
                {
                    ProductId = 60,
                    Name = "Product 60",
                    NameEng = "Product 60 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand I",
                    Price = 578.51m,
                    Discount = 10.82m,
                    IsNew = true,
                    Rating = 1.6,
                    InStock = true,
                    AvailableAmount = 50,
                    NumberOfReviews = 77,
                    NumberOfPurchases = 299,
                    NumberOfViews = 155,
                    Article = "461863",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 7, 5, 7, 8, 4 }),
                    Description = "Description of Product 60",
                },

                new Product
                {
                    ProductId = 61,
                    Name = "Product 61",
                    NameEng = "Product 61 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand J",
                    Price = 324.45m,
                    Discount = 10.81m,
                    IsNew = false,
                    Rating = 3.0,
                    InStock = false,
                    AvailableAmount = 67,
                    NumberOfReviews = 467,
                    NumberOfPurchases = 119,
                    NumberOfViews = 1754,
                    Article = "982611",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5, 8, 10, 3, 3 }),
                    Description = "Description of Product 61",
                },

                new Product
                {
                    ProductId = 62,
                    Name = "Product 62",
                    NameEng = "Product 62 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand K",
                    Price = 973.63m,
                    Discount = 43.87m,
                    IsNew = false,
                    Rating = 2.5,
                    InStock = false,
                    AvailableAmount = 61,
                    NumberOfReviews = 134,
                    NumberOfPurchases = 554,
                    NumberOfViews = 1528,
                    Article = "301738",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 9 }),
                    Description = "Description of Product 62",
                },

                new Product
                {
                    ProductId = 63,
                    Name = "Product 63",
                    NameEng = "Product 63 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand L",
                    Price = 940.63m,
                    Discount = 27.11m,
                    IsNew = true,
                    Rating = 2.2,
                    InStock = true,
                    AvailableAmount = 51,
                    NumberOfReviews = 37,
                    NumberOfPurchases = 377,
                    NumberOfViews = 462,
                    Article = "834152",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 10, 8, 6 }),
                    Description = "Description of Product 63",
                },

                new Product
                {
                    ProductId = 64,
                    Name = "Product 64",
                    NameEng = "Product 64 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand M",
                    Price = 141.1m,
                    Discount = 28.31m,
                    IsNew = true,
                    Rating = 3.4,
                    InStock = false,
                    AvailableAmount = 59,
                    NumberOfReviews = 491,
                    NumberOfPurchases = 345,
                    NumberOfViews = 1144,
                    Article = "576278",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 4, 7, 7 }),
                    Description = "Description of Product 64",
                },

                new Product
                {
                    ProductId = 65,
                    Name = "Product 65",
                    NameEng = "Product 65 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand N",
                    Price = 936.76m,
                    Discount = 40.34m,
                    IsNew = false,
                    Rating = 3.4,
                    InStock = true,
                    AvailableAmount = 33,
                    NumberOfReviews = 180,
                    NumberOfPurchases = 922,
                    NumberOfViews = 130,
                    Article = "717196",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 1, 3, 2, 5 }),
                    Description = "Description of Product 65",
                },

                new Product
                {
                    ProductId = 66,
                    Name = "Product 66",
                    NameEng = "Product 66 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand O",
                    Price = 838.65m,
                    Discount = 36.45m,
                    IsNew = true,
                    Rating = 1.5,
                    InStock = false,
                    AvailableAmount = 65,
                    NumberOfReviews = 247,
                    NumberOfPurchases = 722,
                    NumberOfViews = 1712,
                    Article = "337747",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 2, 3, 10 }),
                    Description = "Description of Product 66",
                },

                new Product
                {
                    ProductId = 67,
                    Name = "Product 67",
                    NameEng = "Product 67 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand P",
                    Price = 633.39m,
                    Discount = 41.15m,
                    IsNew = true,
                    Rating = 1.4,
                    InStock = true,
                    AvailableAmount = 72,
                    NumberOfReviews = 57,
                    NumberOfPurchases = 636,
                    NumberOfViews = 1997,
                    Article = "389813",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 1, 8, 6, 6 }),
                    Description = "Description of Product 67",
                },

                new Product
                {
                    ProductId = 68,
                    Name = "Product 68",
                    NameEng = "Product 68 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand Q",
                    Price = 226.74m,
                    Discount = 33.56m,
                    IsNew = false,
                    Rating = 2.3,
                    InStock = false,
                    AvailableAmount = 65,
                    NumberOfReviews = 77,
                    NumberOfPurchases = 637,
                    NumberOfViews = 1204,
                    Article = "241040",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5, 2, 9, 4 }),
                    Description = "Description of Product 68",
                },

                new Product
                {
                    ProductId = 69,
                    Name = "Product 69",
                    NameEng = "Product 69 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand R",
                    Price = 745.71m,
                    Discount = 36.31m,
                    IsNew = true,
                    Rating = 1.7,
                    InStock = false,
                    AvailableAmount = 46,
                    NumberOfReviews = 79,
                    NumberOfPurchases = 329,
                    NumberOfViews = 313,
                    Article = "611405",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 10 }),
                    Description = "Description of Product 69",
                },

                new Product
                {
                    ProductId = 70,
                    Name = "Product 70",
                    NameEng = "Product 70 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand S",
                    Price = 109.41m,
                    Discount = 29.26m,
                    IsNew = false,
                    Rating = 4.4,
                    InStock = false,
                    AvailableAmount = 79,
                    NumberOfReviews = 127,
                    NumberOfPurchases = 120,
                    NumberOfViews = 269,
                    Article = "432983",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 1, 6, 2, 9, 4 }),
                    Description = "Description of Product 70",
                },

                new Product
                {
                    ProductId = 71,
                    Name = "Product 71",
                    NameEng = "Product 71 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand T",
                    Price = 458.87m,
                    Discount = 31.12m,
                    IsNew = false,
                    Rating = 3.6,
                    InStock = true,
                    AvailableAmount = 45,
                    NumberOfReviews = 242,
                    NumberOfPurchases = 942,
                    NumberOfViews = 116,
                    Article = "629905",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 3, 9, 3 }),
                    Description = "Description of Product 71",
                },

                new Product
                {
                    ProductId = 72,
                    Name = "Product 72",
                    NameEng = "Product 72 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand U",
                    Price = 306.53m,
                    Discount = 0.52m,
                    IsNew = true,
                    Rating = 3.2,
                    InStock = true,
                    AvailableAmount = 47,
                    NumberOfReviews = 352,
                    NumberOfPurchases = 222,
                    NumberOfViews = 995,
                    Article = "613054",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 2, 8, 8 }),
                    Description = "Description of Product 72",
                },

                new Product
                {
                    ProductId = 73,
                    Name = "Product 73",
                    NameEng = "Product 73 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand V",
                    Price = 450.27m,
                    Discount = 0.08m,
                    IsNew = false,
                    Rating = 2.0,
                    InStock = true,
                    AvailableAmount = 54,
                    NumberOfReviews = 152,
                    NumberOfPurchases = 556,
                    NumberOfViews = 368,
                    Article = "873858",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 7, 9 }),
                    Description = "Description of Product 73",
                },

                new Product
                {
                    ProductId = 74,
                    Name = "Product 74",
                    NameEng = "Product 74 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand W",
                    Price = 388.65m,
                    Discount = 34.34m,
                    IsNew = false,
                    Rating = 4.8,
                    InStock = true,
                    AvailableAmount = 73,
                    NumberOfReviews = 342,
                    NumberOfPurchases = 320,
                    NumberOfViews = 1367,
                    Article = "537950",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 8, 7, 10, 4 }),
                    Description = "Description of Product 74",
                },

                new Product
                {
                    ProductId = 75,
                    Name = "Product 75",
                    NameEng = "Product 75 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand X",
                    Price = 363.45m,
                    Discount = 25.81m,
                    IsNew = true,
                    Rating = 4.6,
                    InStock = true,
                    AvailableAmount = 20,
                    NumberOfReviews = 354,
                    NumberOfPurchases = 205,
                    NumberOfViews = 1745,
                    Article = "855010",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 9 }),
                    Description = "Description of Product 75",
                },

                new Product
                {
                    ProductId = 76,
                    Name = "Product 76",
                    NameEng = "Product 76 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand Y",
                    Price = 709.89m,
                    Discount = 20.06m,
                    IsNew = false,
                    Rating = 3.2,
                    InStock = false,
                    AvailableAmount = 34,
                    NumberOfReviews = 356,
                    NumberOfPurchases = 168,
                    NumberOfViews = 1028,
                    Article = "996763",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 9, 3, 5 }),
                    Description = "Description of Product 76",
                },

                new Product
                {
                    ProductId = 77,
                    Name = "Product 77",
                    NameEng = "Product 77 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand Z",
                    Price = 916.75m,
                    Discount = 2.9m,
                    IsNew = false,
                    Rating = 4.5,
                    InStock = true,
                    AvailableAmount = 98,
                    NumberOfReviews = 117,
                    NumberOfPurchases = 844,
                    NumberOfViews = 399,
                    Article = "957008",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 3, 4 }),
                    Description = "Description of Product 77",
                },

                new Product
                {
                    ProductId = 78,
                    Name = "Product 78",
                    NameEng = "Product 78 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand A",
                    Price = 999.95m,
                    Discount = 2.26m,
                    IsNew = true,
                    Rating = 3.9,
                    InStock = false,
                    AvailableAmount = 55,
                    NumberOfReviews = 465,
                    NumberOfPurchases = 527,
                    NumberOfViews = 1115,
                    Article = "143819",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 8 }),
                    Description = "Description of Product 78",
                },

                new Product
                {
                    ProductId = 79,
                    Name = "Product 79",
                    NameEng = "Product 79 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand B",
                    Price = 287.75m,
                    Discount = 16.56m,
                    IsNew = true,
                    Rating = 1.1,
                    InStock = false,
                    AvailableAmount = 62,
                    NumberOfReviews = 82,
                    NumberOfPurchases = 785,
                    NumberOfViews = 893,
                    Article = "619789",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 2 }),
                    Description = "Description of Product 79",
                },

                new Product
                {
                    ProductId = 80,
                    Name = "Product 80",
                    NameEng = "Product 80 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand C",
                    Price = 648.04m,
                    Discount = 27.87m,
                    IsNew = true,
                    Rating = 2.1,
                    InStock = true,
                    AvailableAmount = 68,
                    NumberOfReviews = 497,
                    NumberOfPurchases = 426,
                    NumberOfViews = 85,
                    Article = "877245",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 8, 2, 3 }),
                    Description = "Description of Product 80",
                },

                new Product
                {
                    ProductId = 81,
                    Name = "Product 81",
                    NameEng = "Product 81 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand D",
                    Price = 278.02m,
                    Discount = 4.43m,
                    IsNew = true,
                    Rating = 4.5,
                    InStock = false,
                    AvailableAmount = 91,
                    NumberOfReviews = 151,
                    NumberOfPurchases = 827,
                    NumberOfViews = 1168,
                    Article = "909066",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 10, 3, 6 }),
                    Description = "Description of Product 81",
                },

                new Product
                {
                    ProductId = 82,
                    Name = "Product 82",
                    NameEng = "Product 82 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand E",
                    Price = 998.17m,
                    Discount = 37.65m,
                    IsNew = true,
                    Rating = 2.2,
                    InStock = true,
                    AvailableAmount = 1,
                    NumberOfReviews = 468,
                    NumberOfPurchases = 490,
                    NumberOfViews = 1174,
                    Article = "674809",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 10 }),
                    Description = "Description of Product 82",
                },

                new Product
                {
                    ProductId = 83,
                    Name = "Product 83",
                    NameEng = "Product 83 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand F",
                    Price = 236.58m,
                    Discount = 16.1m,
                    IsNew = false,
                    Rating = 3.5,
                    InStock = true,
                    AvailableAmount = 97,
                    NumberOfReviews = 14,
                    NumberOfPurchases = 176,
                    NumberOfViews = 1402,
                    Article = "732075",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 10, 9, 6 }),
                    Description = "Description of Product 83",
                },

                new Product
                {
                    ProductId = 84,
                    Name = "Product 84",
                    NameEng = "Product 84 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand G",
                    Price = 168.4m,
                    Discount = 44.37m,
                    IsNew = true,
                    Rating = 2.5,
                    InStock = true,
                    AvailableAmount = 7,
                    NumberOfReviews = 386,
                    NumberOfPurchases = 50,
                    NumberOfViews = 905,
                    Article = "908691",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 9, 8, 2, 9 }),
                    Description = "Description of Product 84",
                },

                new Product
                {
                    ProductId = 85,
                    Name = "Product 85",
                    NameEng = "Product 85 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand H",
                    Price = 382.09m,
                    Discount = 21.57m,
                    IsNew = true,
                    Rating = 4.0,
                    InStock = true,
                    AvailableAmount = 68,
                    NumberOfReviews = 195,
                    NumberOfPurchases = 60,
                    NumberOfViews = 853,
                    Article = "613174",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 6, 1, 10, 10 }),
                    Description = "Description of Product 85",
                },

                new Product
                {
                    ProductId = 86,
                    Name = "Product 86",
                    NameEng = "Product 86 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand I",
                    Price = 738.14m,
                    Discount = 4.93m,
                    IsNew = true,
                    Rating = 1.3,
                    InStock = true,
                    AvailableAmount = 32,
                    NumberOfReviews = 46,
                    NumberOfPurchases = 435,
                    NumberOfViews = 1945,
                    Article = "412688",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5, 7 }),
                    Description = "Description of Product 86",
                },

                new Product
                {
                    ProductId = 87,
                    Name = "Product 87",
                    NameEng = "Product 87 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand J",
                    Price = 182.63m,
                    Discount = 6.89m,
                    IsNew = false,
                    Rating = 3.6,
                    InStock = true,
                    AvailableAmount = 19,
                    NumberOfReviews = 67,
                    NumberOfPurchases = 971,
                    NumberOfViews = 1464,
                    Article = "503735",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5 }),
                    Description = "Description of Product 87",
                },

                new Product
                {
                    ProductId = 88,
                    Name = "Product 88",
                    NameEng = "Product 88 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg"
                    }),
                    Brand = "Brand K",
                    Price = 493.81m,
                    Discount = 3.97m,
                    IsNew = true,
                    Rating = 3.6,
                    InStock = false,
                    AvailableAmount = 90,
                    NumberOfReviews = 61,
                    NumberOfPurchases = 735,
                    NumberOfViews = 246,
                    Article = "905053",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 7, 7, 5, 8, 10 }),
                    Description = "Description of Product 88",
                },

                new Product
                {
                    ProductId = 89,
                    Name = "Product 89",
                    NameEng = "Product 89 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand L",
                    Price = 158.93m,
                    Discount = 49.17m,
                    IsNew = false,
                    Rating = 2.6,
                    InStock = true,
                    AvailableAmount = 71,
                    NumberOfReviews = 213,
                    NumberOfPurchases = 924,
                    NumberOfViews = 111,
                    Article = "271497",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 4, 5, 9 }),
                    Description = "Description of Product 89",
                },

                new Product
                {
                    ProductId = 90,
                    Name = "Product 90",
                    NameEng = "Product 90 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand M",
                    Price = 115.5m,
                    Discount = 6.93m,
                    IsNew = true,
                    Rating = 2.2,
                    InStock = false,
                    AvailableAmount = 91,
                    NumberOfReviews = 64,
                    NumberOfPurchases = 34,
                    NumberOfViews = 499,
                    Article = "780430",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5, 1, 2 }),
                    Description = "Description of Product 90",
                },

                new Product
                {
                    ProductId = 91,
                    Name = "Product 91",
                    NameEng = "Product 91 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand N",
                    Price = 131.05m,
                    Discount = 19.76m,
                    IsNew = false,
                    Rating = 3.7,
                    InStock = false,
                    AvailableAmount = 50,
                    NumberOfReviews = 301,
                    NumberOfPurchases = 160,
                    NumberOfViews = 1726,
                    Article = "367372",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 4 }),
                    Description = "Description of Product 91",
                },

                new Product
                {
                    ProductId = 92,
                    Name = "Product 92",
                    NameEng = "Product 92 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand O",
                    Price = 345.48m,
                    Discount = 11.52m,
                    IsNew = true,
                    Rating = 3.3,
                    InStock = false,
                    AvailableAmount = 59,
                    NumberOfReviews = 385,
                    NumberOfPurchases = 394,
                    NumberOfViews = 1945,
                    Article = "311099",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 4, 2, 8, 9, 7 }),
                    Description = "Description of Product 92",
                },

                new Product
                {
                    ProductId = 93,
                    Name = "Product 93",
                    NameEng = "Product 93 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand P",
                    Price = 180.93m,
                    Discount = 42.94m,
                    IsNew = false,
                    Rating = 1.0,
                    InStock = true,
                    AvailableAmount = 24,
                    NumberOfReviews = 22,
                    NumberOfPurchases = 631,
                    NumberOfViews = 1716,
                    Article = "369132",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 2, 9, 5, 3 }),
                    Description = "Description of Product 93",
                },

                new Product
                {
                    ProductId = 94,
                    Name = "Product 94",
                    NameEng = "Product 94 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand Q",
                    Price = 376.82m,
                    Discount = 5.98m,
                    IsNew = false,
                    Rating = 1.4,
                    InStock = true,
                    AvailableAmount = 82,
                    NumberOfReviews = 486,
                    NumberOfPurchases = 758,
                    NumberOfViews = 932,
                    Article = "899745",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 2 }),
                    Description = "Description of Product 94",
                },

                new Product
                {
                    ProductId = 95,
                    Name = "Product 95",
                    NameEng = "Product 95 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand R",
                    Price = 752.54m,
                    Discount = 6.85m,
                    IsNew = true,
                    Rating = 2.2,
                    InStock = true,
                    AvailableAmount = 83,
                    NumberOfReviews = 305,
                    NumberOfPurchases = 904,
                    NumberOfViews = 1433,
                    Article = "591809",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 6, 4, 7, 10 }),
                    Description = "Description of Product 95",
                },

                new Product
                {
                    ProductId = 96,
                    Name = "Product 96",
                    NameEng = "Product 96 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand S",
                    Price = 346.91m,
                    Discount = 43.46m,
                    IsNew = false,
                    Rating = 3.8,
                    InStock = false,
                    AvailableAmount = 43,
                    NumberOfReviews = 56,
                    NumberOfPurchases = 537,
                    NumberOfViews = 1972,
                    Article = "745752",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 4, 4 }),
                    Description = "Description of Product 96",
                },

                new Product
                {
                    ProductId = 97,
                    Name = "Product 97",
                    NameEng = "Product 97 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg"
                    }),
                    Brand = "Brand T",
                    Price = 175.04m,
                    Discount = 38.39m,
                    IsNew = false,
                    Rating = 4.8,
                    InStock = false,
                    AvailableAmount = 50,
                    NumberOfReviews = 337,
                    NumberOfPurchases = 54,
                    NumberOfViews = 908,
                    Article = "705782",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 6, 9, 10, 10 }),
                    Description = "Description of Product 97",
                },

                new Product
                {
                    ProductId = 98,
                    Name = "Product 98",
                    NameEng = "Product 98 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276164_28-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-31.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand U",
                    Price = 159.27m,
                    Discount = 0.09m,
                    IsNew = true,
                    Rating = 1.5,
                    InStock = true,
                    AvailableAmount = 92,
                    NumberOfReviews = 264,
                    NumberOfPurchases = 747,
                    NumberOfViews = 1362,
                    Article = "770542",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 5, 10 }),
                    Description = "Description of Product 98",
                },

                new Product
                {
                    ProductId = 99,
                    Name = "Product 99",
                    NameEng = "Product 99 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg"
                    }),
                    Brand = "Brand V",
                    Price = 633.53m,
                    Discount = 5.99m,
                    IsNew = false,
                    Rating = 1.0,
                    InStock = true,
                    AvailableAmount = 91,
                    NumberOfReviews = 37,
                    NumberOfPurchases = 616,
                    NumberOfViews = 1354,
                    Article = "547833",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2", "Option 3", "Option 4" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 9, 10 }),
                    Description = "Description of Product 99",
                },

                new Product
                {
                    ProductId = 100,
                    Name = "Product 100",
                    NameEng = "Product 100 (Eng)",
                    ImagesSerialized = JsonSerializer.Serialize(new List<string>
                    {
                        "https://kartinki.pics/uploads/posts/2022-12/1670276106_18-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-19.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276120_21-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-22.jpg",
                        "https://kartinki.pics/uploads/posts/2022-12/1670276142_25-kartinkin-net-p-kartinki-pro-vremya-so-smislom-krasivo-26.jpg"
                    }),
                    Brand = "Brand W",
                    Price = 224.01m,
                    Discount = 2.91m,
                    IsNew = false,
                    Rating = 1.2,
                    InStock = true,
                    AvailableAmount = 35,
                    NumberOfReviews = 467,
                    NumberOfPurchases = 447,
                    NumberOfViews = 1456,
                    Article = "613902",
                    OptionsSerialized = JsonSerializer.Serialize(new List<string> { "Option 1", "Option 2" }),
                    DieNumbersSerialized = JsonSerializer.Serialize(new List<int> { 7, 8, 9, 2, 1 }),
                    Description = "Description of Product 100",
                },
            };
        }
    }
}