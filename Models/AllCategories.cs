using System.Collections.ObjectModel;

namespace ShoppingList.Models
{
    internal class AllCategories
    {
        public ObservableCollection<Category> Categories { get; set; } = new();

        public void LoadCategories()
        {
            Categories.Clear();
            string appDataPath = FileSystem.AppDataDirectory;

            var categoryFiles = Directory.EnumerateFiles(appDataPath, "*.json").ToList();

            if (categoryFiles.Count > 0)
            {
                var categories = categoryFiles
                    .Select(Category.FromFile)
                    .OrderByDescending(c => c.Date);

                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
            }
        }
    }
}
