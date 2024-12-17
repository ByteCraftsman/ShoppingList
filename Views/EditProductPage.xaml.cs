using ShoppingList.Models;

namespace ShoppingList.Views;

public partial class EditProductPage : ContentPage
{
    private Product product;
    private Category category;

    public EditProductPage(Product selectedProduct, Category parentCategory)
    {
        InitializeComponent();

        product = selectedProduct;
        category = parentCategory;

        InitializePickers();
        BindingContext = product;
    }

    private void InitializePickers()
    {
        var units = new List<string> { "szt.", "kg", "l", "opak." };
        UnitPicker.ItemsSource = units;

        var stores = new List<string> { "Biedronka", "Selgros", "Lidl", "Auchan", "Inne" };
        StorePicker.ItemsSource = stores;
    }

    private void DecreaseQuantity_Clicked(object sender, EventArgs e)
    {
        if (product.Quantity > 0)
            product.Quantity--;
    }

    private void IncreaseQuantity_Clicked(object sender, EventArgs e)
    {
        if (product.Quantity > 0)
            product.Quantity++;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        product.Save();
        await Navigation.PopModalAsync();
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Potwierdzenie", "Czy na pewno usun¹æ produkt?", "Tak", "Nie");
        if (confirm && product.ParentCategory != null)
        {
            product.ParentCategory.Products.Remove(product);
            product.Save();
            await Navigation.PopModalAsync();
        }
    }

}
