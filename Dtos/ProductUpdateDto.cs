using System.Collections.Generic;

namespace BlossomApi.Dtos
{
    public class ProductUpdateDto
    {
        public string? Name { get; set; }
        public string? NameEng { get; set; }
        public List<string>? Images { get; set; }
        public string? Brand { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }
        public int? AvailableAmount { get; set; }
        public string? Article { get; set; }
        public string? Description { get; set; }
        public string? Ingridients { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsHit { get; set; }
        public bool? IsShown { get; set; }
        public string? UnitOfMeasurement { get; set; }
        public string? Group { get; set; }
        public string? Type { get; set; }
        public string? ManufacturerBarcode { get; set; }
        public string? UKTZED { get; set; }
        public decimal? Markup { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? VATRate { get; set; }
        public decimal? ExciseTaxRate { get; set; }
        public decimal? PensionFundRate { get; set; }
        public string? VATLetter { get; set; }
        public string? ExciseTaxLetter { get; set; }
        public string? PensionFundLetter { get; set; }
        public decimal? DocumentQuantity { get; set; }
        public decimal? ActualQuantity { get; set; }
        public int? MainCategoryId { get; set; }
        public List<int>? AdditionalCategoryIds { get; set; }
        public List<int>? CharacteristicIds { get; set; }
    }
}
