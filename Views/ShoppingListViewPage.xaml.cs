using ShoppingList.Models;
using System.Collections.ObjectModel;

namespace ShoppingList.Views;

public partial class ShoppingListViewPage : ContentPage
{
    public ObservableCollection<Product> ShoppingListItems { get; set; } = new();

    private AllCategories allCategories;

    public ShoppingListViewPage()
    {
        InitializeComponent();
        allCategories = new AllCategories();
        LoadShoppingList();
        BindingContext = this;
    }

    public void LoadShoppingList()
    {
        allCategories.LoadCategories();

        var items = allCategories.Categories
            .SelectMany(c => c.Products.Where(p => !p.IsPurchased))
            .OrderBy(p => p.ParentCategory?.Name)
            .ThenBy(p => p.Name);

        ShoppingListItems.Clear();
        foreach (var item in items)
        {
            ShoppingListItems.Add(item);
        }

        StorePicker.SelectedIndex = 0;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadShoppingList();
    }

    private async void MarkSelectedAsPurchased_Clicked(object sender, EventArgs e)
    {
        var selectedItems = ShoppingListItems.Where(p => p.IsSelected).ToList();

        if (!selectedItems.Any())
        {
            await DisplayAlert("Brak zaznaczenia", "Nie zaznaczono ¿adnych produktów.", "OK");
            return;
        }

        foreach (var product in selectedItems)
        {
            product.IsPurchased = true;
            product.IsSelected = false;
            product.Save();
            product.ParentCategory?.SortProducts();
        }

        await DisplayAlert("Gotowe", "Zaznaczone produkty zosta³y oznaczone jako kupione.", "OK");

        StorePicker.SelectedIndex = 0;

        LoadShoppingList();
    }

    private void StorePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedStore = StorePicker.SelectedItem as string;

        if (selectedStore == "Wszystkie" || string.IsNullOrEmpty(selectedStore))
        {
            LoadShoppingList();
        }
        else
        {
            var filteredItems = allCategories.Categories
                .SelectMany(c => c.Products)
                .Where(p => p.Store == selectedStore && !p.IsPurchased)
                .OrderBy(p => p.ParentCategory?.Name)
                .ThenBy(p => p.Name);

            ShoppingListItems.Clear();
            foreach (var item in filteredItems)
            {
                ShoppingListItems.Add(item);
            }
        }
    }
}
