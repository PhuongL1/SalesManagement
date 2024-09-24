using SalesManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SalesManagement.ViewModel
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Product> _Products;
        public ObservableCollection<Product> Products
        {
            get { return _Products; }
            set
            {
                _Products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        private Product _SelectedProduct;
        public Product SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                _SelectedProduct = value;
                OnPropertyChanged(nameof(SelectedProduct));
            }
        }

        // Commands
        public ICommand AddProductCommand { get; set; }
        public ICommand EditProductCommand { get; set; }
        public ICommand DeleteProductCommand { get; set; }

        public ProductViewModel()
        {
            // Load danh sách sản phẩm từ DB
            Products = new ObservableCollection<Product>(DataProvider.Ins.DB.Products.ToList());

            // Khởi tạo các command
            AddProductCommand = new RelayCommand<object>((p) => true, (p) => AddProduct());
            EditProductCommand = new RelayCommand<object>((p) => SelectedProduct != null, (p) => EditProduct());
            DeleteProductCommand = new RelayCommand<object>((p) => SelectedProduct != null, (p) => DeleteProduct());
        }

        private void AddProduct()
        {
            // Logic thêm sản phẩm vào database
            var newProduct = new Product()
            {
                ProductName = "New Product",
                Category = "Category",
                Price = 100,
                Stock = 10
            };
            DataProvider.Ins.DB.Products.Add(newProduct);
            DataProvider.Ins.DB.SaveChanges();
            Products.Add(newProduct);
        }

        private void EditProduct()
        {
            // Logic chỉnh sửa sản phẩm đã chọn
            DataProvider.Ins.DB.SaveChanges();
        }

        private void DeleteProduct()
        {
            // Logic xóa sản phẩm khỏi database
            DataProvider.Ins.DB.Products.Remove(SelectedProduct);
            DataProvider.Ins.DB.SaveChanges();
            Products.Remove(SelectedProduct);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
