using SalesManagement.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
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
        public ICommand AddProductCommand { get; private set; }
        public ICommand UpdateProductCommand { get; private set; }
        public ICommand DeleteProductCommand { get; private set; }

        public ProductViewModel()
        {
            // Load danh sách sản phẩm từ DB
            Products = new ObservableCollection<Product>(DataProvider.Ins.DB.Products.ToList());

            // Khởi tạo các command
            AddProductCommand = new RelayCommand<object>((p) => true, (p) => AddProduct());
            UpdateProductCommand = new RelayCommand<object>((p) => SelectedProduct != null, (p) => UpdateProduct());
            DeleteProductCommand = new RelayCommand<object>((p) => SelectedProduct != null, (p) => DeleteProduct());

            // Khởi tạo đối tượng SelectedProduct ban đầu (nếu cần)
            SelectedProduct = new Product();
        }

        private void AddProduct()
        {
            if (SelectedProduct == null || string.IsNullOrWhiteSpace(SelectedProduct.ProductName) ||
                string.IsNullOrWhiteSpace(SelectedProduct.Category) || SelectedProduct.Price <= 0 || SelectedProduct.Stock < 0)
            {
                MessageBox.Show("Please ensure all fields are filled out correctly.");
                return;
            }

            var newProduct = new Product()
            {
                ProductName = SelectedProduct.ProductName,
                Category = SelectedProduct.Category,
                Price = SelectedProduct.Price,
                Stock = SelectedProduct.Stock
            };

            DataProvider.Ins.DB.Products.Add(newProduct);
            try
            {
                DataProvider.Ins.DB.SaveChanges();
                Products.Add(newProduct);
                SelectedProduct = new Product(); // Reset SelectedProduct sau khi thêm
            }
            catch (DbEntityValidationException ex)
            {
                // Handle validation errors
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        MessageBox.Show($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
            }
        }

        private void UpdateProduct()
        {
            if (SelectedProduct == null || string.IsNullOrWhiteSpace(SelectedProduct.ProductName))
            {
                MessageBox.Show("Please select a product to update.");
                return;
            }

            var productToUpdate = DataProvider.Ins.DB.Products.Find(SelectedProduct.ProductID);
            if (productToUpdate != null)
            {
                productToUpdate.ProductName = SelectedProduct.ProductName;
                productToUpdate.Category = SelectedProduct.Category;
                productToUpdate.Price = SelectedProduct.Price;
                productToUpdate.Stock = SelectedProduct.Stock;

                try
                {
                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Product updated successfully!");
                }
                catch (DbEntityValidationException ex)
                {
                    // Handle validation errors
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            MessageBox.Show($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Product not found in the database.");
            }
        }

        private void DeleteProduct()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            var productToDelete = DataProvider.Ins.DB.Products.Find(SelectedProduct.ProductID);
            if (productToDelete != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this product?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DataProvider.Ins.DB.Products.Remove(productToDelete);
                    try
                    {
                        DataProvider.Ins.DB.SaveChanges();
                        Products.Remove(productToDelete);
                        SelectedProduct = new Product(); // Reset SelectedProduct sau khi xóa
                        MessageBox.Show("Product deleted successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting product: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Product not found in the database.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // Class RelayCommand
        public class RelayCommand<T> : ICommand
        {
            private readonly Predicate<T> _canExecute;
            private readonly Action<T> _execute;

            public event EventHandler CanExecuteChanged;

            public RelayCommand(Predicate<T> canExecute, Action<T> execute)
            {
                _canExecute = canExecute;
                _execute = execute;
            }

            public bool CanExecute(object parameter)
            {
                return _canExecute == null || _canExecute((T)parameter);
            }

            public void Execute(object parameter)
            {
                _execute((T)parameter);
            }

            public void RaiseCanExecuteChanged()
            {
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
