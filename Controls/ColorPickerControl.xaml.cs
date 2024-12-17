using System.Diagnostics;

namespace ShoppingList.Controls
{
    public partial class ColorPickerControl : ContentView
    {
        public static readonly BindableProperty SelectedColorProperty =
            BindableProperty.Create(nameof(SelectedColor), typeof(string), typeof(ColorPickerControl), default(string), BindingMode.TwoWay);

        public string SelectedColor
        {
            get => (string)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        private Button previousSelectedButton;

        public ColorPickerControl()
        {
            InitializeComponent();
        }

        private void OnColorButtonClicked(object sender, EventArgs e)
        {
            var clickedButton = sender as Button;

            if (previousSelectedButton != null)
            {
                previousSelectedButton.BorderColor = Colors.Transparent;
                previousSelectedButton.BorderWidth = 0;
            }

            clickedButton.BorderColor = Colors.White;
            clickedButton.BorderWidth = 4;

            previousSelectedButton = clickedButton;

            SelectedColor = clickedButton.BackgroundColor.ToHex();
            Debug.WriteLine($"Wybrany kolor: {SelectedColor}");
        }
    }
}