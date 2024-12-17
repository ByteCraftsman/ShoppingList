using ShoppingList.Models;
using ShoppingList.Services;
using ShoppingList.Data;
using System.Collections.ObjectModel;

namespace ShoppingList.Views;

public partial class ShoppingPage : ContentPage
{

    private AllCategories allCategories;
    private readonly ImportExportService importExportService;
    public ObservableCollection<Category> FilteredCategories { get; set; } = new ObservableCollection<Category>();

    public ShoppingPage()
    {
        InitializeComponent();
        allCategories = new AllCategories();
        importExportService = new ImportExportService();
        BindingContext = this;
    }

    private void LoadCategories()
    {
        allCategories.LoadCategories();
        FilterCategories();
    }

    private void FilterCategories()
    {
        FilteredCategories.Clear();
        foreach (var category in allCategories.Categories.Where(c => c.Products.Any()))
        {
            FilteredCategories.Add(category);
        }

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadCategories();
    }

    private async void AddProduct_Clicked(object sender, EventArgs e)
    {
        var predefinedProductNames = DefaultData.GetDefaultProductsByCategory()
            .SelectMany(kvp => kvp.Value.Select(p => p.Name))
            .ToList();

        predefinedProductNames.Add("Dodaj w³asny produkt");

        string selectedProductName = await DisplayActionSheet("Wybierz produkt", "Anuluj", null, predefinedProductNames.ToArray());

        if (string.IsNullOrWhiteSpace(selectedProductName) || selectedProductName == "Anuluj")
            return;

        if (selectedProductName == "Dodaj w³asny produkt")
        {
            AddCustomProduct();
            return;
        }

        AddPredefinedProduct(selectedProductName);
    }

    private async void AddCustomProduct()
    {
        string name = await DisplayPromptAsync("Nowy produkt", "Podaj nazwê produktu:");

        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }  
        else if (!IsNameUnique(name))
        {
            await DisplayAlert("B³¹d", "Taki produkt ju¿ istnieje!", "OK");
            return;
        }

        string unit = await DisplayActionSheet("Wybierz jednostkê", "Anuluj", null, "szt.", "kg", "l", "opak.");
        if (string.IsNullOrWhiteSpace(unit) || unit == "Anuluj")
            return;

        string categoryName = await DisplayActionSheet("Wybierz kategoriê", "Anuluj", null,
            allCategories.Categories.Select(c => c.Name).ToArray());

        string store = await DisplayActionSheet("Wybierz sklep", "Anuluj", null, "Biedronka", "Selgros", "Lidl", "Auchan", "Inne");
        if (string.IsNullOrWhiteSpace(store) || store == "Anuluj")
            return;

        if (string.IsNullOrWhiteSpace(categoryName) || categoryName == "Anuluj")
            return;


        var category = allCategories.Categories.FirstOrDefault(c => c.Name == categoryName);

        if (category != null)
        {
            var newProduct = new Product
            {
                Name = name,
                Unit = unit,
                Quantity = 1,
                Store = store
            };

            category.Products.Add(newProduct);
            category.Save();
            LoadCategories();
        }
        else
        {
            await DisplayAlert("B³¹d", "Wybrana kategoria nie istnieje!", "OK");
            return;
        }
    }

    private async void AddPredefinedProduct(string selectedProductName)
    {

        if (!IsNameUnique(selectedProductName))
        {
            await DisplayAlert("B³¹d", "Taki produkt ju¿ istnieje!", "OK");
            return;
        }


        Product selectedProduct = null;
        Category selectedCategory = null;

        foreach (var category in DefaultData.GetDefaultProductsByCategory())
        {
            selectedProduct = category.Value.FirstOrDefault(p => p.Name == selectedProductName);
            if (selectedProduct != null)
            {
                selectedCategory = allCategories.Categories.FirstOrDefault(c => c.Name == category.Key);
                break;
            }
        }

        string store = await DisplayActionSheet("Wybierz sklep", "Anuluj", null, "Biedronka", "Selgros", "Lidl", "Auchan", "Inne");
        if (string.IsNullOrWhiteSpace(store) || store == "Anuluj")
            return;


        if (selectedProduct != null && selectedCategory != null)
        {
            var newProduct = new Product
            {
                Name = selectedProduct.Name,
                Unit = selectedProduct.Unit,
                Quantity = selectedProduct.Quantity,
                Store = store
            };

            selectedCategory.Products.Add(newProduct);
            selectedCategory.Save();
            LoadCategories();
        } 
        else if(selectedCategory == null)
        {
            await DisplayAlert("B³¹d", "Nie znaleziono kategorii!", "OK");
            return;
        }
        else
        {
            await DisplayAlert("B³¹d", "Nie znaleziono produktu!", "OK");
            return;
        }
    }

    private bool IsNameUnique(string name)
    {
        foreach (var category in allCategories.Categories)
        {
            if (category.Products.Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }
        }

        return true;
    }


    private void MarkAsPurchased_Clicked(object sender, EventArgs e)
    {
        foreach (var category in allCategories.Categories)
        {
            var selectedProducts = category.Products.Where(p => p.IsSelected).ToList();
            foreach (var product in selectedProducts)
            {
                product.IsPurchased = true;
                product.IsSelected = false;
            }
            category.SortProducts();
            category.Save();
        }
        LoadCategories();
    }


    private async void DeleteSelected_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usun¹æ zaznaczone produkty?", "Tak", "Nie");
        if (!confirm)
            return;

        foreach (var category in allCategories.Categories)
        {
            var selectedProducts = category.Products.Where(p => p.IsSelected).ToList();
            foreach (var product in selectedProducts)
            {
                category.Products.Remove(product);
            }
            category.Save();
        }
        LoadCategories();
    }

    private async void Export_Clicked(object sender, EventArgs e)
    {
        try
        {
            string exportFolderPath = Path.Combine(FileSystem.AppDataDirectory, "Exports");

            string fileName = $"ShoppingListExport_{DateTime.Now:yyyyMMddHHmmss}.json";

            importExportService.ExportToFile(allCategories.Categories, exportFolderPath, fileName);

            await DisplayAlert("Eksport zakoñczony", $"Eksportowano do folderu: {exportFolderPath}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("B³¹d", $"Nie uda³o siê eksportowaæ danych: {ex.Message}", "OK");
        }
    }

    private async void Import_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Wybierz plik JSON z list¹ zakupów",
                FileTypes = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.iOS, new[] { "public.json" } },
                        { DevicePlatform.Android, new[] { "application/json" } },
                        { DevicePlatform.WinUI, new[] { ".json" } },
                        { DevicePlatform.MacCatalyst, new[] { "json" } }
                    })
            });

            if (result != null)
            {
                string filePath = result.FullPath;
                importExportService.ImportFromFile(filePath, allCategories.Categories);
                await DisplayAlert("Import zakoñczony", $"Zaimportowano dane z pliku: {filePath}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("B³¹d", $"Nie uda³o siê zaimportowaæ danych: {ex.Message}", "OK");
        }

        LoadCategories();
    }

}