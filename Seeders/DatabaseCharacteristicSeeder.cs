using BlossomApi.Models;
using System.Collections.Generic;

namespace BlossomApi.Seeders
{
    public static class DatabaseCharacteristicSeeder
    {
        public static List<Characteristic> GetCharacteristics()
        {
            return new List<Characteristic>
            {
                // Color characteristics
                new Characteristic { CharacteristicId = 1, Title = "Color", Desc = "Red" },
                new Characteristic { CharacteristicId = 2, Title = "Color", Desc = "Blue" },
                new Characteristic { CharacteristicId = 3, Title = "Color", Desc = "Green" },
                new Characteristic { CharacteristicId = 4, Title = "Color", Desc = "Yellow" },
                new Characteristic { CharacteristicId = 5, Title = "Color", Desc = "Black" },

                // Size characteristics
                new Characteristic { CharacteristicId = 6, Title = "Size", Desc = "Small" },
                new Characteristic { CharacteristicId = 7, Title = "Size", Desc = "Medium" },
                new Characteristic { CharacteristicId = 8, Title = "Size", Desc = "Large" },
                new Characteristic { CharacteristicId = 9, Title = "Size", Desc = "Extra Large" },
                new Characteristic { CharacteristicId = 10, Title = "Size", Desc = "XXL" },

                // Material characteristics
                new Characteristic { CharacteristicId = 11, Title = "Material", Desc = "Cotton" },
                new Characteristic { CharacteristicId = 12, Title = "Material", Desc = "Wool" },
                new Characteristic { CharacteristicId = 13, Title = "Material", Desc = "Silk" },
                new Characteristic { CharacteristicId = 14, Title = "Material", Desc = "Polyester" },
                new Characteristic { CharacteristicId = 15, Title = "Material", Desc = "Leather" },

                // Warranty characteristics
                new Characteristic { CharacteristicId = 16, Title = "Warranty", Desc = "6 months" },
                new Characteristic { CharacteristicId = 17, Title = "Warranty", Desc = "1 year" },
                new Characteristic { CharacteristicId = 18, Title = "Warranty", Desc = "2 years" },
                new Characteristic { CharacteristicId = 19, Title = "Warranty", Desc = "3 years" },
                new Characteristic { CharacteristicId = 20, Title = "Warranty", Desc = "Lifetime" },

                // Weight characteristics
                new Characteristic { CharacteristicId = 21, Title = "Weight", Desc = "Light" },
                new Characteristic { CharacteristicId = 22, Title = "Weight", Desc = "Medium" },
                new Characteristic { CharacteristicId = 23, Title = "Weight", Desc = "Heavy" },
                new Characteristic { CharacteristicId = 24, Title = "Weight", Desc = "Very Heavy" },
                new Characteristic { CharacteristicId = 25, Title = "Weight", Desc = "Ultra Light" }
            };
        }
    }
}
