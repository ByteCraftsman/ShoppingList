using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace ShoppingList.Models
{
    public class Product : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Unit { get; set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Store { get; set; }

        private bool _isOptional;
        public bool IsOptional
        {
            get => _isOptional;
            set
            {
                if (_isOptional != value)
                {
                    _isOptional = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsPurchased { get; set; }

        [JsonIgnore]
        public bool IsSelected { get; set; }

        [JsonIgnore]
        public Category ParentCategory { get; set; }     

        public void Save()
        {
            ParentCategory.Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
