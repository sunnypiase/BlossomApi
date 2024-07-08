using BlossomApi.Models;

namespace BlossomApi.Seeders
{
    public static class DatabaseCategorySeeder
    {
        public static List<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category { CategoryId = 1, ParentCategoryId = 0, Name = "Парфумерія" },
                new Category { CategoryId = 2, ParentCategoryId = 1, Name = "жіноча парфумерія" },
                new Category { CategoryId = 3, ParentCategoryId = 1, Name = "чоловіча парфумерія" },
                new Category { CategoryId = 4, ParentCategoryId = 1, Name = "дитяча парфумерія" },
                new Category { CategoryId = 5, ParentCategoryId = 1, Name = "парфумована вода" },
                new Category { CategoryId = 6, ParentCategoryId = 1, Name = "нішева парфумерія" },

                new Category { CategoryId = 7, ParentCategoryId = 0, Name = "Макіяж" },
                new Category { CategoryId = 8, ParentCategoryId = 7, Name = "очі" },
                new Category { CategoryId = 9, ParentCategoryId = 8, Name = "туш для вій" },
                new Category { CategoryId = 10, ParentCategoryId = 8, Name = "тіні для повік" },
                new Category { CategoryId = 11, ParentCategoryId = 8, Name = "олівець для очей" },
                new Category { CategoryId = 12, ParentCategoryId = 8, Name = "підводка для очей" },
                new Category { CategoryId = 13, ParentCategoryId = 8, Name = "коректор для очей" },

                new Category { CategoryId = 14, ParentCategoryId = 7, Name = "брови" },
                new Category { CategoryId = 15, ParentCategoryId = 14, Name = "тіні для брів" },
                new Category { CategoryId = 16, ParentCategoryId = 14, Name = "гель для брів" },
                new Category { CategoryId = 17, ParentCategoryId = 14, Name = "олівець для брів" },
                new Category { CategoryId = 18, ParentCategoryId = 14, Name = "помада для брів" },
                new Category { CategoryId = 19, ParentCategoryId = 14, Name = "фарба для брів" },
                new Category { CategoryId = 20, ParentCategoryId = 14, Name = "віск для брів" },

                new Category { CategoryId = 21, ParentCategoryId = 7, Name = "губи" },
                new Category { CategoryId = 22, ParentCategoryId = 21, Name = "помада для губ" },
                new Category { CategoryId = 23, ParentCategoryId = 21, Name = "блиск для губ" },
                new Category { CategoryId = 24, ParentCategoryId = 21, Name = "олійка для губ" },
                new Category { CategoryId = 25, ParentCategoryId = 21, Name = "контурний олівець для губ" },
                new Category { CategoryId = 26, ParentCategoryId = 21, Name = "тінт для губ" },

                new Category { CategoryId = 27, ParentCategoryId = 7, Name = "обличчя" },
                new Category { CategoryId = 28, ParentCategoryId = 27, Name = "база під макіяж" },
                new Category { CategoryId = 29, ParentCategoryId = 27, Name = "СС-крем" },
                new Category { CategoryId = 30, ParentCategoryId = 27, Name = "ВВ-крем" },
                new Category { CategoryId = 31, ParentCategoryId = 27, Name = "румʼяна" },
                new Category { CategoryId = 32, ParentCategoryId = 27, Name = "бронзер" },
                new Category { CategoryId = 33, ParentCategoryId = 27, Name = "Хайлайтер" },
                new Category { CategoryId = 34, ParentCategoryId = 27, Name = "консилер" },
                new Category { CategoryId = 35, ParentCategoryId = 27, Name = "пудра для обличчя" },
                new Category { CategoryId = 36, ParentCategoryId = 27, Name = "скульптор для обличчя" },
                new Category { CategoryId = 37, ParentCategoryId = 27, Name = "кушони" },
                new Category { CategoryId = 38, ParentCategoryId = 27, Name = "тональний крем" },

                new Category { CategoryId = 39, ParentCategoryId = 7, Name = "пензлі" },

                new Category { CategoryId = 40, ParentCategoryId = 0, Name = "Волосся" },
                new Category { CategoryId = 41, ParentCategoryId = 40, Name = "шампуні" },
                new Category { CategoryId = 42, ParentCategoryId = 41, Name = "сухий шампунь" },
                new Category { CategoryId = 43, ParentCategoryId = 41, Name = "шампуні від лупи" },
                new Category { CategoryId = 44, ParentCategoryId = 41, Name = "безсульфатні шампуні" },
                new Category { CategoryId = 45, ParentCategoryId = 41, Name = "тверді шампуні" },
                new Category { CategoryId = 46, ParentCategoryId = 41, Name = "для блондинок" },

                new Category { CategoryId = 47, ParentCategoryId = 40, Name = "бальзам для волосся" },
                new Category { CategoryId = 48, ParentCategoryId = 40, Name = "кондиціонер" },
                new Category { CategoryId = 49, ParentCategoryId = 40, Name = "маски для волосся" },
                new Category { CategoryId = 50, ParentCategoryId = 40, Name = "скраб і пілінг для волосся" },
                new Category { CategoryId = 51, ParentCategoryId = 40, Name = "лосьйон для волосся" },
                new Category { CategoryId = 52, ParentCategoryId = 40, Name = "олія та масло для волосся" },
                new Category { CategoryId = 53, ParentCategoryId = 40, Name = "крем для волосся" },
                new Category { CategoryId = 54, ParentCategoryId = 40, Name = "вітаміни в капсулах для волосся" },
                new Category { CategoryId = 55, ParentCategoryId = 40, Name = "світле волосся" },
                new Category { CategoryId = 56, ParentCategoryId = 40, Name = "для дітей" },

                new Category { CategoryId = 57, ParentCategoryId = 40, Name = "аксесуари для волосся" },
                new Category { CategoryId = 58, ParentCategoryId = 57, Name = "гребінці" },
                new Category { CategoryId = 59, ParentCategoryId = 57, Name = "резинки" },
                new Category { CategoryId = 60, ParentCategoryId = 57, Name = "заколки" },
                new Category { CategoryId = 61, ParentCategoryId = 57, Name = "бігуді" },
                new Category { CategoryId = 62, ParentCategoryId = 57, Name = "масажні щітки" },
                new Category { CategoryId = 63, ParentCategoryId = 57, Name = "щітки для шампуню" },

                new Category { CategoryId = 64, ParentCategoryId = 0, Name = "Обличчя" },
                new Category { CategoryId = 65, ParentCategoryId = 64, Name = "крем" },
                new Category { CategoryId = 66, ParentCategoryId = 64, Name = "маски" },
                new Category { CategoryId = 67, ParentCategoryId = 64, Name = "бальзам" },
                new Category { CategoryId = 68, ParentCategoryId = 64, Name = "нічні засоби для обличчя" },
                new Category { CategoryId = 69, ParentCategoryId = 64, Name = "точкові засоби" },

                new Category { CategoryId = 70, ParentCategoryId = 64, Name = "сироватки" },
                new Category { CategoryId = 71, ParentCategoryId = 70, Name = "есенції" },
                new Category { CategoryId = 72, ParentCategoryId = 70, Name = "сироватки" },
                new Category { CategoryId = 73, ParentCategoryId = 70, Name = "емульсії" },

                new Category { CategoryId = 74, ParentCategoryId = 64, Name = "гідрофільні олії" },
                new Category { CategoryId = 75, ParentCategoryId = 64, Name = "тонер" },
                new Category { CategoryId = 76, ParentCategoryId = 64, Name = "скраби, пілінги" },

                new Category { CategoryId = 77, ParentCategoryId = 64, Name = "очищення" },
                new Category { CategoryId = 78, ParentCategoryId = 77, Name = "пінка" },
                new Category { CategoryId = 79, ParentCategoryId = 77, Name = "гелі" },
                new Category { CategoryId = 80, ParentCategoryId = 77, Name = "міцелярна вода" },

                new Category { CategoryId = 81, ParentCategoryId = 64, Name = "догляд для очей" },
                new Category { CategoryId = 82, ParentCategoryId = 81, Name = "крем для шкіри навколо очей" },
                new Category { CategoryId = 83, ParentCategoryId = 81, Name = "патчі під очі" },
                new Category { CategoryId = 84, ParentCategoryId = 81, Name = "гель для повік" },
                new Category { CategoryId = 85, ParentCategoryId = 81, Name = "сироватка для вій" },

                new Category { CategoryId = 86, ParentCategoryId = 64, Name = "догляд за губами" },
                new Category { CategoryId = 87, ParentCategoryId = 86, Name = "олійка" },
                new Category { CategoryId = 88, ParentCategoryId = 86, Name = "бальзам" },
                new Category { CategoryId = 89, ParentCategoryId = 86, Name = "крем" },
                new Category { CategoryId = 90, ParentCategoryId = 86, Name = "маска для губ" },

                new Category { CategoryId = 91, ParentCategoryId = 64, Name = "косметичні щітки" },
                new Category { CategoryId = 92, ParentCategoryId = 64, Name = "масажери для обличчя" },
                new Category { CategoryId = 93, ParentCategoryId = 64, Name = "автозасмага для обличчя" },
                new Category { CategoryId = 94, ParentCategoryId = 64, Name = "сонцезахист" },

                new Category { CategoryId = 95, ParentCategoryId = 0, Name = "Тіло і ванна" },
                new Category { CategoryId = 96, ParentCategoryId = 95, Name = "Для ванни та душу" },
                new Category { CategoryId = 97, ParentCategoryId = 96, Name = "гелі" },
                new Category { CategoryId = 98, ParentCategoryId = 96, Name = "лосьйони" },
                new Category { CategoryId = 99, ParentCategoryId = 96, Name = "мило" },
                new Category { CategoryId = 100, ParentCategoryId = 96, Name = "скраби та пілінги" },
                new Category { CategoryId = 101, ParentCategoryId = 96, Name = "молочко" },
                new Category { CategoryId = 102, ParentCategoryId = 96, Name = "для інтимної гігієни" },
                new Category { CategoryId = 103, ParentCategoryId = 96, Name = "сіль для ванни" },
                new Category { CategoryId = 104, ParentCategoryId = 96, Name = "піна для ванни" },

                new Category { CategoryId = 105, ParentCategoryId = 95, Name = "Гігієна" },
                new Category { CategoryId = 106, ParentCategoryId = 105, Name = "ватні диски" },
                new Category { CategoryId = 107, ParentCategoryId = 105, Name = "ватні палички" },
                new Category { CategoryId = 108, ParentCategoryId = 105, Name = "серветки для інтимної гігієни" },
                new Category { CategoryId = 109, ParentCategoryId = 105, Name = "зубні щітки" },
                new Category { CategoryId = 110, ParentCategoryId = 105, Name = "зубна нитка" },
                new Category { CategoryId = 111, ParentCategoryId = 105, Name = "зубна паста" },
                new Category { CategoryId = 112, ParentCategoryId = 105, Name = "таблетки для індикації зубного нальоту" },

                new Category { CategoryId = 113, ParentCategoryId = 95, Name = "Дезодоранти, антиперспіранти" },
                new Category { CategoryId = 114, ParentCategoryId = 95, Name = "Ароматична вода, спреї для тіла" },
                new Category { CategoryId = 115, ParentCategoryId = 95, Name = "Масло та олії для тіла" },
                new Category { CategoryId = 116, ParentCategoryId = 95, Name = "Корекція фігури" },
                new Category { CategoryId = 117, ParentCategoryId = 95, Name = "Засоби для рук" },
                new Category { CategoryId = 118, ParentCategoryId = 95, Name = "Сонцезахисні засоби" },
                new Category { CategoryId = 119, ParentCategoryId = 95, Name = "Автозасмага" },

                new Category { CategoryId = 120, ParentCategoryId = 0, Name = "Чоловікам" },
                new Category { CategoryId = 121, ParentCategoryId = 120, Name = "Чоловіча парфумерія" },
                new Category { CategoryId = 122, ParentCategoryId = 120, Name = "Дезодоранти" },
                new Category { CategoryId = 123, ParentCategoryId = 120, Name = "Догляд за тілом" },
                new Category { CategoryId = 124, ParentCategoryId = 120, Name = "Догляд за обличчям" },
                new Category { CategoryId = 125, ParentCategoryId = 120, Name = "Засоби для та після гоління" },
                new Category { CategoryId = 126, ParentCategoryId = 120, Name = "Догляд за волоссям" },
                new Category { CategoryId = 127, ParentCategoryId = 120, Name = "Догляд за бородою" },

                new Category { CategoryId = 128, ParentCategoryId = 0, Name = "Аксесуари" },
                new Category { CategoryId = 129, ParentCategoryId = 128, Name = "Для волосся" },
                new Category { CategoryId = 130, ParentCategoryId = 129, Name = "гребінці" },
                new Category { CategoryId = 131, ParentCategoryId = 129, Name = "бігуді" },
                new Category { CategoryId = 132, ParentCategoryId = 129, Name = "заколки" },
                new Category { CategoryId = 133, ParentCategoryId = 129, Name = "повʼязки" },
                new Category { CategoryId = 134, ParentCategoryId = 129, Name = "резинки" },

                new Category { CategoryId = 135, ParentCategoryId = 128, Name = "Для макіяжу" },
                new Category { CategoryId = 136, ParentCategoryId = 135, Name = "пензлі" },
                new Category { CategoryId = 137, ParentCategoryId = 135, Name = "спонжи" },

                new Category { CategoryId = 138, ParentCategoryId = 128, Name = "Гігієна та догляд" },
                new Category { CategoryId = 139, ParentCategoryId = 138, Name = "зубні щітки" },
                new Category { CategoryId = 140, ParentCategoryId = 138, Name = "серветки" },
                new Category { CategoryId = 141, ParentCategoryId = 138, Name = "ватні диски та палички" },

                new Category { CategoryId = 142, ParentCategoryId = 128, Name = "Для душу" },
                new Category { CategoryId = 143, ParentCategoryId = 142, Name = "щітка для масажу" },
                new Category { CategoryId = 144, ParentCategoryId = 142, Name = "мочалка" },
                new Category { CategoryId = 145, ParentCategoryId = 142, Name = "рушник" },

                new Category { CategoryId = 146, ParentCategoryId = 128, Name = "Дорожні набори" },
                new Category { CategoryId = 147, ParentCategoryId = 128, Name = "Косметички" },

                new Category { CategoryId = 148, ParentCategoryId = 0, Name = "Подарунки" },
                new Category { CategoryId = 149, ParentCategoryId = 148, Name = "Подарункові набори" },
                new Category { CategoryId = 150, ParentCategoryId = 148, Name = "Подарункові сертифікати" },
                new Category { CategoryId = 151, ParentCategoryId = 148, Name = "Подарункове пакування" },

                new Category { CategoryId = 152, ParentCategoryId = 0, Name = "Для майстрів" },
                new Category { CategoryId = 153, ParentCategoryId = 152, Name = "Манікюр" },
                new Category { CategoryId = 154, ParentCategoryId = 152, Name = "Вії" },
                new Category { CategoryId = 155, ParentCategoryId = 152, Name = "Брови" },
                new Category { CategoryId = 156, ParentCategoryId = 152, Name = "Масаж" },
                new Category { CategoryId = 157, ParentCategoryId = 152, Name = "Депіляція" }
            };
        }
    }
}