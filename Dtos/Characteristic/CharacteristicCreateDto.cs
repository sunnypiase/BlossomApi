using System.ComponentModel.DataAnnotations;

namespace BlossomApi.Dtos.Characteristic
{
    public class CharacteristicCreateDto
    {
        [Required]
        public string Title { get; set; }
        public string Desc { get; set; }
    }
}
