using System.ComponentModel.DataAnnotations;

namespace BlossomApi.Dtos.Characteristic
{
    public class CharacteristicUpdateDto
    {
        [Required]
        public string Title { get; set; }
        public string Desc { get; set; }
    }
}
