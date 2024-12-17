using System;
using System.Collections.ObjectModel;
using ShoppingList.Models;

namespace ShoppingList.Data
{
    public static class DefaultData
    {
        public static ObservableCollection<Category> GetPredefinedCategories()
        {
            return new ObservableCollection<Category>
            {
                new Category
                {
                    Name = "Mięso",
                    ColorHex = Colors.Red.ToHex(),
                    Date = DateTime.Now,
                    Products = new ObservableCollection<Product>()
                },
                new Category
                {
                    Name = "Nabiał",
                    ColorHex = Colors.LightBlue.ToHex(),
                    Date = DateTime.Now,
                    Products = new ObservableCollection<Product>()
                },
                new Category
                {
                    Name = "Pieczywo",
                    ColorHex = Colors.SandyBrown.ToHex(),
                    Date = DateTime.Now,
                    Products = new ObservableCollection<Product>()
                },
                new Category
                {
                    Name = "Owoce",
                    ColorHex = Colors.Orange.ToHex(),
                    Date = DateTime.Now,
                    Products = new ObservableCollection<Product>()
                },
                new Category
                {
                    Name = "Warzywa",
                    ColorHex = Colors.Green.ToHex(),
                    Date = DateTime.Now,
                    Products = new ObservableCollection<Product>()
                }
            };
        }
        public static Dictionary<string, ObservableCollection<Product>> GetDefaultProductsByCategory()
        {
            return new Dictionary<string, ObservableCollection<Product>>
            {

                { "Mięso", new ObservableCollection<Product>
                    {
                        new Product { Name = "Udko z kurczaka", Unit = "szt.", Quantity = 1 },
                        new Product { Name = "Schab wieprzowy", Unit = "kg", Quantity = 1 },
                        new Product { Name = "Parówki z indyka", Unit = "opak.", Quantity = 1 }
                    }
                },
                { "Pieczywo", new ObservableCollection<Product>
                    {
                        new Product { Name = "Chleb", Unit = "szt.", Quantity = 1 },
                        new Product { Name = "Bułki", Unit = "szt.", Quantity = 1 },
                        new Product { Name = "Bagietki", Unit = "szt.", Quantity = 1 }
                    }
                },
                { "Nabiał", new ObservableCollection<Product>
                    {
                        new Product { Name = "Mleko", Unit = "l", Quantity = 1 },
                        new Product { Name = "Jajka", Unit = "opak.", Quantity = 1 },
                        new Product { Name = "Śmietana", Unit = "opak.", Quantity = 1 }
                    }
                },
                { "Owoce", new ObservableCollection<Product>
                    {
                        new Product { Name = "Jabłka", Unit = "kg", Quantity = 1 },
                        new Product { Name = "Pomarańcze", Unit = "kg", Quantity = 1 },
                        new Product { Name = "Banany", Unit = "kg", Quantity = 1 }
                    }
                },
                { "Warzywa", new ObservableCollection<Product>
                    {
                        new Product { Name = "Pomidor", Unit = "kg", Quantity = 1 },
                        new Product { Name = "Ogórek", Unit = "kg", Quantity = 1 },
                        new Product { Name = "Cebula", Unit = "kg", Quantity = 1 }
                    }
                }
            };
        }
    }
}
