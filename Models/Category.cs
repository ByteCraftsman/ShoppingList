using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace ShoppingList.Models
{
    public class Category   
    {
        [JsonIgnore]
        public string Filename { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Product> Products { get; set; } = new();
        [JsonIgnore]
        public DateTime Date { get; set; }
        public string ColorHex { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static Category FromFile(string filename)
        {
            string categorySerialized = File.ReadAllText(filename);
            Category category;

            try
            {
                category = JsonConvert.DeserializeObject<Category>(categorySerialized);
            }
            catch (JsonSerializationException)
            {
                category = new Category
                {
                    Name = "Kategoria",
                    Products = new ObservableCollection<Product>(),
                    Date = File.GetLastWriteTime(filename),
                    ColorHex = Colors.Transparent.ToHex()
                };
            }

            if (category == null)
            {
                category = new Category();
            }

            category.Filename = filename;

            foreach (var product in category.Products)
            {
                product.ParentCategory = category;
            }

            return category;
        }

        public void Save()
        {
            File.WriteAllText(Filename, ToJson());
        }

        public void SortProducts()
        {
            var sortedProducts = Products
                .OrderBy(p => p.IsPurchased)
                .ThenBy(p => p.Name)
                .ToList();

            Products = new ObservableCollection<Product>(sortedProducts);
            Save();
        }
    }

}