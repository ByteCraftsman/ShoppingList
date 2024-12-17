namespace ShoppingList.Views;     
using ShoppingList.Models;
using System.Diagnostics;
using System.Collections.ObjectModel;

[QueryProperty(nameof(ItemId), nameof(ItemId))]

public partial class CategoryPage : ContentPage
{
    public string ItemId
    {
        set { LoadCategory(value); }
    }

    public CategoryPage()
    {
        InitializeComponent();
        String appDataPath = FileSystem.AppDataDirectory;
        String randomFileName = $"{Guid.NewGuid()}.json";

        LoadCategory(Path.Combine(appDataPath, randomFileName));
       
    }

    private void LoadCategory(string fileName)
    {
        Category categoryModel;

        if (File.Exists(fileName))
        {
            categoryModel = Category.FromFile(fileName);
        }
        else
        {
            categoryModel = new Category
            {
                Filename = fileName,
                Name = "",
                Products = new ObservableCollection<Product>(),
                ColorHex = Colors.Blue.ToHex(),
                Date = DateTime.Now,
            };
        }

        BindingContext = categoryModel;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Category category)
        {
            int numOfCategories = GetNumOfCategories();
            string defaultName = GenerateDefaultName();

            if (string.IsNullOrWhiteSpace(TextEntry.Text))
            {
                category.Name = $"{defaultName}";
            }
            else if (!IsNameUnique(TextEntry.Text) && !File.Exists(category.Filename))
            {
                await DisplayAlert("B³¹d", "Nazwa kategorii musi byæ unikalna.", "OK");
                return;
            }
            else
            {
                category.Name = TextEntry.Text;
            }

            category.Products = new ObservableCollection<Product>();

            category.Save();
        }

        await Shell.Current.GoToAsync("..");
    }


    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Category category)
            if (File.Exists(category.Filename))
                File.Delete(category.Filename);
        await Shell.Current.GoToAsync("..");
    }

    private Button previousButton;
    private void ColorButton_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;

        if (previousButton != null)
        {
            previousButton.BorderColor = Colors.Transparent;
            previousButton.BorderWidth = 0;
        }

        button.BorderColor = Colors.White;
        button.BorderWidth = 4;

        previousButton = button;

        var selectedColor = button.BackgroundColor.ToHex();

        if (BindingContext is Category category)
        {
            category.ColorHex = selectedColor;
            Debug.WriteLine(category.ColorHex);
        }
    }

    private bool IsNameUnique(string name)
    {
        var allCategories = new Models.AllCategories();
        allCategories.LoadCategories();
        return !allCategories.Categories.Any(category => category.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    private int GetNumOfCategories()
    {
        string appDataPath = FileSystem.AppDataDirectory;

        var categoryFiles = Directory.GetFiles(appDataPath, "*.json");
        return categoryFiles.Length;
    }

    private string GenerateDefaultName()
    {
        int numOfCategories = GetNumOfCategories();
        return $"Kategoria {numOfCategories + 1}";
    }
}