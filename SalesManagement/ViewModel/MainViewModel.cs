using SalesManagement.Model;
using SalesManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SalesManagement.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        // Thuộc tính ngày giờ hiện tại
        private string _currentDateTime;
        public string CurrentDateTime
        {
            get { return _currentDateTime; }
            set
            {
                _currentDateTime = value;
                OnPropertyChanged(nameof(CurrentDateTime));
            }
        }

        // Thuộc tính địa chỉ công ty
        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        // Thuộc tính tóm tắt sản phẩm
        private ObservableCollection<Product> _productSummary;
        public ObservableCollection<Product> ProductSummary
        {
            get { return _productSummary; }
            set
            {
                _productSummary = value;
                OnPropertyChanged(nameof(ProductSummary));
            }
        }

        // Các Command để mở các View khác
        public ICommand OpenProductViewCommand { get; set; }
        public ICommand OpenCustomerViewCommand { get; set; }
        public ICommand OpenOrderViewCommand { get; set; }
        public ICommand OpenOrderDetailViewCommand { get; set; }
        public ICommand OpenUserViewCommand { get; set; }

        // Constructor
        public MainViewModel()
        {
            // Khởi tạo giá trị ngày giờ hiện tại
            CurrentDateTime = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");

            // Địa chỉ công ty giả định
            Address = "123 Main Street, Cityville, Country";

            // Lấy tóm tắt sản phẩm từ DataProvider (giả sử DataProvider đã kết nối CSDL)
            ProductSummary = new ObservableCollection<Product>(DataProvider.Ins.DB.Products.ToList());

            // Khởi tạo các Command
            OpenProductViewCommand = new RelayCommand<object>((p) => true, (p) => OpenProductView());
            OpenCustomerViewCommand = new RelayCommand<object>((p) => true, (p) => OpenCustomerView());
            OpenOrderViewCommand = new RelayCommand<object>((p) => true, (p) => OpenOrderView());
            OpenOrderDetailViewCommand = new RelayCommand<object>((p) => true, (p) => OpenOrderDetailView());
            OpenUserViewCommand = new RelayCommand<object>((p) => true, (p) => OpenUserView());
        }

        // Các hàm mở View khác
        private void OpenProductView()
        {
            ProductView productView = new ProductView();
            productView.Show();
        }

        private void OpenCustomerView()
        {
            CustomerView customerView = new CustomerView();
            customerView.Show();
        }

        private void OpenOrderView()
        {
            OrderView orderView = new OrderView();
            orderView.Show();
        }

        private void OpenOrderDetailView()
        {
            OrderDetailView orderDetailView = new OrderDetailView();
            orderDetailView.Show();
        }

        private void OpenUserView()
        {
            UserView userView = new UserView();
            userView.Show();
        }
    }
}
