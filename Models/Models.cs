﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using BlossomApi.AttributeValidations;
using BlossomApi.Services;
using Microsoft.AspNetCore.Identity;

namespace BlossomApi.Models
{
    public class SiteUser
    {
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? DepartmentNumber { get; set; }

        // Navigation properties
        public string IdentityUserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public IdentityUser IdentityUser { get; set; }

        public ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();
        public ICollection<Product> FavoriteProducts { get; set; } = new List<Product>();

        // One-to-one relationship with Cashback
        public Cashback Cashback { get; set; }

        // Додаємо колекцію відгуків
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }

    public class Cashback
    {
        [Key] public int CashbackId { get; set; }

        [Required]
        [PhoneNumber(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [Required]
        public decimal Balance { get; set; }

        // Optional relationship with SiteUser for online accounts
        public int? SiteUserId { get; set; }
        public SiteUser SiteUser { get; set; }
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
        [Key] public int ProductId { get; set; } //не нужно
        public string Name { get; set; } //+
        public string NameEng { get; set; }// +
        public string ImagesSerialized { get; set; } = "[]"; // Default to an empty JSON array +
        public decimal Price { get; set; } //+
        public decimal Discount { get; set; } // Percentage of discount +
        public double Rating { get; set; } = 0.0;
        private int _availableAmount;
        public int AvailableAmount
        {
            get => _availableAmount;
            set
            {
                _availableAmount = value;
                InStock = _availableAmount > 0;
            }
        }
        public int NumberOfReviews { get; set; } = 0;
        public int NumberOfPurchases { get; set; }//-
        public int NumberOfViews { get; set; }//-
        public string Article { get; set; } //+
        public string DieNumbersSerialized { get; set; } = "[]"; // Serialized die numbers-
        public string Description { get; set; } //+
        public string? Ingridients { get; set; }//+
        public bool InStock { get; set;  }
        public bool IsNew { get; set; } // +
        public bool IsHit { get; set; }// +
        public bool IsShown { get; set; } //+

        // New properties for synchronization with Cassa
        public string? UnitOfMeasurement { get; set; } // Одиниця виміру +
        public string? Group { get; set; } // Група +
        public string? Type { get; set; } // Тип +
        public string? ManufacturerBarcode { get; set; } // Штрихкод виробника +
        public string? UKTZED { get; set; } // УКТЗЕД +
        public decimal? Markup { get; set; } // Націнка+
        public decimal PurchasePrice { get; set; } // Ціна придбання +
        public decimal? VATRate { get; set; } // Ставка ПДВ +
        public decimal? ExciseTaxRate { get; set; } // Ставка акцизн. податку
        public decimal? PensionFundRate { get; set; } // Ставка збору ПФ +
        public string? VATLetter { get; set; } // Літера ставки ПДВ +
        public string? ExciseTaxLetter { get; set; } // Літера ставки акцизного податку +
        public string? PensionFundLetter { get; set; } // Літера ставки збору ПФ + 
        public decimal? DocumentQuantity { get; set; } // Кількість згідно документу +
        public decimal? ActualQuantity { get; set; } // Фактична кількість +
        public string? MetaKeys { get; set; }
        public string? MetaDescription { get; set; }

        // Explicit foreign key for main category
        public int MainCategoryId { get; set; }

        // Navigation properties
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Characteristic> Characteristics { get; set; } = new List<Characteristic>();
        public ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new List<ShoppingCartProduct>();
        public ICollection<SiteUser> UsersWhoFavorited { get; set; } = new List<SiteUser>(); // Many-to-many with SiteUser
        public ICollection<Banner> Banners { get; set; } = new List<Banner>();
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
        // Foreign key for Brand
        public int? BrandId { get; set; }

        // Navigation property for Brand
        public Brand? Brand { get; set; }
        // Not mapped properties for lists
        [NotMapped]
        public List<string> Images
        {
            get
            {
                return string.IsNullOrEmpty(ImagesSerialized)
                    ? new List<string>()
                    : JsonSerializer.Deserialize<List<string>>(ImagesSerialized) ?? new List<string>();
            }
            set => ImagesSerialized = JsonSerializer.Serialize(value ?? new List<string>());
        }

        [NotMapped]
        public List<int> DieNumbers
        {
            get
            {
                return string.IsNullOrEmpty(DieNumbersSerialized)
                    ? new List<int>()
                    : JsonSerializer.Deserialize<List<int>>(DieNumbersSerialized) ?? new List<int>();
            }
            set => DieNumbersSerialized = JsonSerializer.Serialize(value ?? new List<int>());
        }

        [NotMapped]
        public Category MainCategory
        {
            get
            {
                return Categories.FirstOrDefault(c => c.CategoryId == MainCategoryId);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(MainCategory), "Main category cannot be null.");
                }
                var categoryToRemove = MainCategoryId;

                MainCategoryId = value.CategoryId;

                Categories = [value, .. Categories.Where(c => c.CategoryId != MainCategoryId && c.CategoryId != categoryToRemove)];
            }
        }

        [NotMapped]
        public List<Category> AdditionalCategories
        {
            get => Categories.Where(c => c.CategoryId != MainCategoryId).ToList();
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(AdditionalCategories), "Additional categories cannot be null.");
                }

                // Remove all non-main categories
                Categories = [MainCategory, .. value];
            }
        }
    }

    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string LogoImageUrl { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaDescription { get; set; }

        // Navigation properties
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Максимальна довжина тексту відгуку — 500 символів.")]
        public string ReviewText { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Рейтинг повинен бути між 1 та 5.")]
        public int Rating { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Зв'язок з продуктом
        [Required]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // Зв'язок з користувачем
        [Required]
        public int SiteUserId { get; set; }
        public SiteUser SiteUser { get; set; }
    }

    public class Characteristic : IHasProducts
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
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
        public decimal DiscountFromCashback { get; set; } // Discount from cashback
        public decimal CashbackEarned { get; set; } // Cashback earned from this order

        // Navigation properties
        public int? CashbackId { get; set; }
        public Cashback Cashback { get; set; }
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
        Refund, // User refunded the order
        Offline // User made an offline order
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

    public class Banner : IHasProducts
    {
        [Key] public int BannerId { get; set; }

        [Required] public string Title { get; set; } // Banner title
        [Required] public string Description { get; set; } // Banner description

        // Images for different screen sizes
        [Required] public string DesktopImageUrl { get; set; } // Image for desktop
        [Required] public string LaptopImageUrl { get; set; } // Image for laptop
        [Required] public string TabletImageUrl { get; set; } // Image for tablet
        [Required] public string PhoneImageUrl { get; set; } // Image for phone

        // Alt text for each image size
        [Required] public string AltText { get; set; }
        public string? MetaKeywords { get; set; }
        public string? MetaDescription { get; set; }

        // Navigation properties for associated products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
    public class Blog : IHasProducts
    {
        [Key] public int BlogId { get; set; }

        [Required] public string Title { get; set; } // Blog title
        [Required] public string Description { get; set; } // Blog description

        // Images for different screen sizes
        [Required] public string DesktopImageUrl { get; set; } // Image for desktop
        [Required] public string LaptopImageUrl { get; set; } // Image for laptop
        [Required] public string TabletImageUrl { get; set; } // Image for tablet
        [Required] public string PhoneImageUrl { get; set; } // Image for phone

        // Alt text for each image size
        [Required] public string AltText { get; set; }

        // SEO fields
        public string? MetaKeywords { get; set; }
        public string? MetaDescription { get; set; }

        // Navigation properties for associated products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}