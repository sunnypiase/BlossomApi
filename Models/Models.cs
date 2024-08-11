using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;

namespace BlossomApi.Models
{
    public class SiteUser
    {
        [Key] public int UserId { get; set; }
        public string Username { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? DepartmentNumber { get; set; }

        // Navigation properties
        public string IdentityUserId { get; set; }

        // Navigation properties
        [ForeignKey("IdentityUserId")] public IdentityUser IdentityUser { get; set; }
        public ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
    }

    public class ShoppingCart
    {
        [Key] public int ShoppingCartId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int? SiteUserId { get; set; } // Can be null if the user is not logged in

        // Navigation properties
        public SiteUser? SiteUser { get; set; } // Can be null if the user is not logged in
        public ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new List<ShoppingCartProduct>();

        // Navigation property for the relationship with Order
        public Order? Order { get; set; }
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
        public string ImagesSerialized { get; set; } = "[]"; // Default to an empty JSON array
        public string Brand { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; } // Precentage of discount
        public bool IsNew { get; set; }
        public double Rating { get; set; }
        public bool InStock { get; set; }
        public int AvailableAmount { get; set; }
        public int NumberOfReviews { get; set; }
        public int NumberOfPurchases { get; set; }
        public int NumberOfViews { get; set; }
        public string Article { get; set; }
        public string DieNumbersSerialized { get; set; } = "[]"; // Serialized die numbers
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
            get
            {
                return string.IsNullOrEmpty(ImagesSerialized)
                    ? []
                    : JsonSerializer.Deserialize<List<string>>(ImagesSerialized) ?? [];
            }
            set => ImagesSerialized = JsonSerializer.Serialize(value ?? []);
        }


        [NotMapped]
        public List<int> DieNumbers
        {
            get
            {
                return string.IsNullOrEmpty(DieNumbersSerialized)
                    ? []
                    : JsonSerializer.Deserialize<List<int>>(DieNumbersSerialized) ?? [];
            }
            set => DieNumbersSerialized = JsonSerializer.Serialize(value ?? []);
        }
    }

    public class Review
    {
        [Key] public int ReviewId { get; set; }
        public string Name { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }

    public class Characteristic
    {
        [Key] public int CharacteristicId { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

    public class Category
    {
        [Key] public int CategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public string Name { get; set; }

        // Navigation properties
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

    public class Order
    {
        [Key] public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string Username { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool DontCallMe { get; set; }
        public bool EcoPackaging { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; } // Discount in value
        public decimal TotalPriceWithDiscount { get; set; } // Total price - discount value
        public decimal DiscountFromPromocode { get; set; } // Discount in value from promocode
        public decimal DiscountFromProductAction { get; set; } // Discount in value from product action
        
        public int ShoppingCartId { get; set; }

        // Navigation properties
        public int? PromocodeId { get; set; }
        public Promocode Promocode { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public DeliveryInfo DeliveryInfo { get; set; }
    }

    public class Promocode
    {
        [Key] public int PromocodeId { get; set; }
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public int UsageLeft { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow;
    }

    public enum OrderStatus
    {
        Created, //  User created the order
        Accepted, // Admin accepted the order after calling the user
        Declined, // Admin declined the order after calling the user
        NeedToShip, // Admin accepted the order and need to ship it
        Shipped, // Admin shipped the order
        Completed, // User received the order
        Refund // User refunded the order
    }

    public class DeliveryInfo
    {
        [Key] public int DeliveryInfoId { get; set; }

        public string City { get; set; }
        public string DepartmentNumber { get; set; }
        public int OrderId { get; set; }
        // Navigation property
        public Order Order { get; set; }
    }
}