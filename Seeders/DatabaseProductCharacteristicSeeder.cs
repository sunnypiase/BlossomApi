namespace BlossomApi.Seeders
{
    public static class DatabaseProductCharacteristicSeeder
    {
        public static IEnumerable<object> GetProductCharacteristicConnections()
        {
            var connections = new List<object>();
            var random = new Random();
            int numberOfCharacteristics = 25; // Assuming there are 10 characteristics

            for (int productId = 1; productId <= 100; productId++)
            {
                // Each product will have between 1 and 3 characteristics
                int numberOfConnections = random.Next(1, 4);

                var selectedCharacteristics = new HashSet<int>();
                while (selectedCharacteristics.Count < numberOfConnections)
                {
                    int characteristicId = random.Next(1, numberOfCharacteristics + 1);
                    selectedCharacteristics.Add(characteristicId);
                }

                foreach (var characteristicId in selectedCharacteristics)
                {
                    connections.Add(new { CharacteristicId = characteristicId, ProductId = productId });
                }
            }

            return connections;
        }
    }
}