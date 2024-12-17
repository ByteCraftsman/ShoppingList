using ShoppingList.Views;

namespace ShoppingList
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.CategoryPage), typeof(Views.CategoryPage));
            Routing.RegisterRoute(nameof(ShoppingListViewPage), typeof(ShoppingListViewPage));
        }
    }
}
