using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BlossomApi.Models
{
    public class SiteUser
    {
        [Key] public int SiteUserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // Foreign key for User
        public int UserId { get; set; }

        // Navigation property
        public User User { get; set; }
    }

    public class User
    {
        [Key] public int UserId { get; set; }
        public string Username { get; set; }

        // Navigation properties
        public SiteUser SiteUser { get; set; }
        public ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
    }

    public class ShoppingCart
    {
        [Key] public int ShoppingCartId { get; set; }
        public DateTime CreatedDate { get; set; }

        public int UserId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new List<ShoppingCartProduct>();

        // Navigation property for the relationship with Order
        public Order Order { get; set; }
    }

    public class ShoppingCartProduct
    {
        [Key] public int ShoppingCartProductId { get; set; }

        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Navigation properties
        public ShoppingCart ShoppingCart { get; set; }
        public Product Product { get; set; }
    }

    public class Product
    {
        [Key] public int ProductId { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public string ImagesSerialized { get; set; } // Serialized images
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public bool IsNew { get; set; }
        public double Rating { get; set; }
        public bool InStock { get; set; }
        public int AvailableAmount { get; set; }
        public int NumberOfReviews { get; set; }
        public int NumberOfPurchases { get; set; }
        public int NumberOfViews { get; set; }
        public string Article { get; set; }
        public string OptionsSerialized { get; set; } // Serialized options
        public string DieNumbersSerialized { get; set; } // Serialized die numbers
        public string Description { get; set; }

        // Navigation properties
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Characteristic> Characteristics { get; set; } = new List<Characteristic>();
        public ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new List<ShoppingCartProduct>();

        // Not mapped properties for lists
        [NotMapped]
        public List<string> Images
        {
            get => JsonSerializer.Deserialize<List<string>>(ImagesSerialized) ?? new List<string>();
            set => ImagesSerialized = JsonSerializer.Serialize(value);
        }

        [NotMapped]
        public List<string> Options
        {
            get => JsonSerializer.Deserialize<List<string>>(OptionsSerialized) ?? new List<string>();
            set => OptionsSerialized = JsonSerializer.Serialize(value);
        }

        [NotMapped]
        public List<int> DieNumbers
        {
            get => JsonSerializer.Deserialize<List<int>>(DieNumbersSerialized) ?? new List<int>();
            set => DieNumbersSerialized = JsonSerializer.Serialize(value);
        }
    }

    public class Review
    {
        [Key] public int ReviewId { get; set; }
        public string Name { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }

    public class Characteristic
    {
        [Key] public int CharacteristicId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

    public class Category
    {
        [Key] public int CategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Characteristic> Characteristics { get; set; } = new List<Characteristic>();
    }

    public class Order
    {
        [Key] public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        public int ShoppingCartId { get; set; }

        // Navigation properties
        public ShoppingCart ShoppingCart { get; set; }
        public DeliveryInfo DeliveryInfo { get; set; }
    }

    public class DeliveryInfo
    {
        [Key] public int DeliveryInfoId { get; set; }

        public int OrderId { get; set; }
        public string Address { get; set; }
        public DateTime DeliveryDate { get; set; }

        // Navigation property
        public Order Order { get; set; }
    }
}
