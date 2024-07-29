using System;
using System.Collections.Generic;
using System.Linq;

namespace BlossomApi.Seeders
{
    public static class DatabaseProductCategorySeeder
    {
        public static IEnumerable<object> GetProductCategoryConnections()
        {
            var connections = new List<object>();
            var random = new Random();
            int numberOfCategories = 157; // Assuming there are 157 categories

            for (int productId = 1; productId <= 100; productId++)
            {
                // Each product will have between 1 and 5 categories
                int numberOfConnections = random.Next(1, 6);

                var selectedCategories = new HashSet<int>();
                while (selectedCategories.Count < numberOfConnections)
                {
                    int categoryId = random.Next(1, numberOfCategories + 1);
                    selectedCategories.Add(categoryId);
                }

                foreach (var categoryId in selectedCategories)
                {
                    connections.Add(new { CategoryId = categoryId, ProductId = productId });
                }
            }

            return connections;
        }
    }
}