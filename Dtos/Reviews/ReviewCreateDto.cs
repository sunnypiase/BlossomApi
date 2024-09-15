using System.ComponentModel.DataAnnotations;

namespace BlossomApi.Dtos.Reviews
{
    public class ReviewCreateDto
    {
        [Required(ErrorMessage = "Текст відгуку є обов'язковим.")]
        [MaxLength(500, ErrorMessage = "Максимальна довжина тексту відгуку — 500 символів.")]
        public string ReviewText { get; set; }

        [Required(ErrorMessage = "Рейтинг є обов'язковим.")]
        [Range(1, 5, ErrorMessage = "Рейтинг повинен бути між 1 та 5.")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "ID продукту є обов'язковим.")]
        public int ProductId { get; set; }
    }
}
