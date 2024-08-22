using System.ComponentModel.DataAnnotations;

namespace BlossomApi.Dtos
{
    public class ProductCreateDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "NameEng is required.")]
        public string NameEng { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "PurchasePrice is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "PurchasePrice must be greater than 0.")]
        public decimal PurchasePrice { get; set; }

        [Required(ErrorMessage = "AvailableAmount is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "AvailableAmount must be non-negative.")]
        public int AvailableAmount { get; set; }

        [Required(ErrorMessage = "Article is required.")]
        public string Article { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Discount is required.")]
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "IsNew is required.")]
        public bool IsNew { get; set; }

        [Required(ErrorMessage = "IsHit is required.")]
        public bool IsHit { get; set; }

        [Required(ErrorMessage = "IsShown is required.")]
        public bool IsShown { get; set; }

        [Required(ErrorMessage = "MainCategoryId is required.")]
        public int MainCategoryId { get; set; }

        public List<int> AdditionalCategoryIds { get; set; } = new();
        public List<int> CharacteristicIds { get; set; } = new();
        public List<string> Images { get; set; } = new();

        public string? Ingridients { get; set; }
        public string? UnitOfMeasurement { get; set; }
        public string? Group { get; set; }
        public string? Type { get; set; }
        public string? ManufacturerBarcode { get; set; }
        public string? UKTZED { get; set; }
        public decimal? Markup { get; set; }
        public decimal? VATRate { get; set; }
        public decimal? ExciseTaxRate { get; set; }
        public decimal? PensionFundRate { get; set; }
        public string? VATLetter { get; set; }
        public string? ExciseTaxLetter { get; set; }
        public string? PensionFundLetter { get; set; }
        public decimal? DocumentQuantity { get; set; }
        public decimal? ActualQuantity { get; set; }
    }
}
