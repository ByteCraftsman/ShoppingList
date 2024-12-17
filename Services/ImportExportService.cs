using Newtonsoft.Json;
using System.Collections.ObjectModel;
using ShoppingList.Models;

namespace ShoppingList.Services
{
    internal class ImportExportService
    {
        public void ExportToFile(ObservableCollection<Category> categories, string folderPath, string fileName)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName);

            var allCategoriesJson = JsonConvert.SerializeObject(categories, Formatting.Indented);
            File.WriteAllText(filePath, allCategoriesJson);
        }

        public void ImportFromFile(string inputFilePath, ObservableCollection<Category> categories)
        {
            if (!File.Exists(inputFilePath)) return;

            var importedCategories = JsonConvert.DeserializeObject<ObservableCollection<Category>>(
                File.ReadAllText(inputFilePath));

            if (importedCategories != null)
            {
                string appDataPath = FileSystem.AppDataDirectory;
                foreach (var category in importedCategories)
                {
                    category.Filename = Path.Combine(appDataPath, $"{Guid.NewGuid()}.json");
                    category.Save();
                }

                categories.Clear();
                foreach (var category in importedCategories)
                {
                    categories.Add(category);
                }
            }
        }
    }
}
