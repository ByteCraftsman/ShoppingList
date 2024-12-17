using System.Collections.ObjectModel;

namespace ShoppingList.Models
{
    public class Store
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<Product> Products { get; set; } = new();
    }
}
