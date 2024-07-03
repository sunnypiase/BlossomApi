using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        // Add this navigation property for the relationship with Order
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
        public string NameEng { get; set; } // Added for English name
        public string Image { get; set; } // Image URL
        public string Brand { get; set; } // Brand name
        public decimal Price { get; set; }
        public decimal Discount { get; set; } // Discount amount
        public bool IsNew { get; set; } // Whether the product is new
        public double Rating { get; set; } // Product rating
        public bool InStock { get; set; } // Whether the product is in stock
        public int AvailableAmount { get; set; }
        public string Description { get; set; }

        // Navigation properties
        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<ShoppingCartProduct> ShoppingCartProducts { get; set; } = new List<ShoppingCartProduct>();
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
