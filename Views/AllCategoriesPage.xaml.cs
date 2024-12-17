using ShoppingList.Models;
using ShoppingList.Data;

namespace ShoppingList.Views;

public partial class AllCategoriesPage : ContentPage
{
    private AllCategories allCategories;

    public AllCategoriesPage()
    {
        InitializeComponent();
        allCategories = new Models.AllCategories();
        BindingContext = allCategories;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        allCategories.LoadCategories();
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CategoryPage));
    }

    private void DeleteButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        var category = (Category)imageButton.BindingContext;

        if (File.Exists(category.Filename))
        {
            File.Delete(category.Filename);
            
        }

        allCategories.Categories.Remove(category);
        allCategories.LoadCategories();
    }

    private async void EditButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;
        var category = (Category)imageButton.BindingContext;
        await Shell.Current.GoToAsync($"{nameof(CategoryPage)}?{nameof(CategoryPage.ItemId)}={category.Filename}");
    }

    private async void AddPredefinedCategories_Clicked(object sender, EventArgs e)
    {
        var predefinedCategories = DefaultData.GetPredefinedCategories();
        string appDataPath = FileSystem.AppDataDirectory;

        foreach (var category in predefinedCategories)
        {
            if (!allCategories.Categories.Any(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
            {
                category.Filename = Path.Combine(appDataPath, $"{Guid.NewGuid()}.json");
                category.Save();
                allCategories.Categories.Add(category);
            }
        }

        allCategories.LoadCategories();
        await DisplayAlert("Gotowe", "Dodano predefiniowane kategorie.", "OK");
    }
}
