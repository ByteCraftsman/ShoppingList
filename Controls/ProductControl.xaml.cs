using ShoppingList.Views;
using ShoppingList.Models;

namespace ShoppingList.Controls;

public partial class ProductControl : ContentView
{
    private AllCategories allCategories;
    public ProductControl()
    {
        InitializeComponent();
        allCategories = new AllCategories();
    }

    private async void EditButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Product product && Parent.BindingContext is Category category)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new EditProductPage(product, category));
        }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BindingContext is Product product)
        {
            product.Save();
            product.ParentCategory?.SortProducts();

            if (Parent is ShoppingListViewPage shoppingListView)
            {
                shoppingListView.LoadShoppingList();
            }
        }
    }

    private void IncreaseQuantity_Clicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        var product = (Product)button.BindingContext;
        if (product.Quantity > 0)
        {
            product.Quantity++;
            product.Save();
        }
    }

    private void DecreaseQuantity_Clicked(object sender, EventArgs e)
    {
        var button = (ImageButton)sender;
        var product = (Product)button.BindingContext;
        if (product.Quantity > 0)
        {
            product.Quantity--;
            product.Save();
        }
        else
        {
            product.ParentCategory.Products.Remove(product);
        }
    }
}