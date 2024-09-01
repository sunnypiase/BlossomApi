﻿using System.ComponentModel.DataAnnotations;

namespace BlossomApi.Dtos.Blogs
{
    public class BlogCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string? MetaKeywords { get; set; }
        public string? MetaDescription { get; set; }

        [Required]
        public IFormFile DesktopImage { get; set; }

        [Required]
        public IFormFile LaptopImage { get; set; }

        [Required]
        public IFormFile TabletImage { get; set; }

        [Required]
        public IFormFile PhoneImage { get; set; }

        [Required]
        public string DesktopAltText { get; set; }

        [Required]
        public string LaptopAltText { get; set; }

        [Required]
        public string TabletAltText { get; set; }

        [Required]
        public string PhoneAltText { get; set; }

        public List<int> ProductIds { get; set; } = new List<int>();
    }
}
