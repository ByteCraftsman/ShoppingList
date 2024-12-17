using System.Diagnostics;

namespace ShoppingList
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }

}
