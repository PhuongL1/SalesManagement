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
    public class OrderViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Order> _Orders;
        public ObservableCollection<Order> Orders
        {
            get { return _Orders; }
            set
            {
                _Orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        private Order _SelectedOrder;
        public Order SelectedOrder
        {
            get { return _SelectedOrder; }
            set
            {
                _SelectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        // Commands
        public ICommand AddOrderCommand { get; set; }
        public ICommand EditOrderCommand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }

        public OrderViewModel()
        {
            // Load danh sách đơn hàng từ DB
            Orders = new ObservableCollection<Order>(DataProvider.Ins.DB.Orders.ToList());

            // Khởi tạo các command
            AddOrderCommand = new RelayCommand<object>((p) => true, (p) => AddOrder());
            EditOrderCommand = new RelayCommand<object>((p) => SelectedOrder != null, (p) => EditOrder());
            DeleteOrderCommand = new RelayCommand<object>((p) => SelectedOrder != null, (p) => DeleteOrder());
        }

        private void AddOrder()
        {
            // Logic thêm đơn hàng vào database
            var newOrder = new Order()
            {
                CustomerID = 1, // Thay đổi giá trị theo thực tế
                TotalAmount = 500,
                Status = "Pending"
            };
            DataProvider.Ins.DB.Orders.Add(newOrder);
            DataProvider.Ins.DB.SaveChanges();
            Orders.Add(newOrder);
        }

        private void EditOrder()
        {
            // Logic chỉnh sửa đơn hàng đã chọn
            DataProvider.Ins.DB.SaveChanges();
        }

        private void DeleteOrder()
        {
            // Logic xóa đơn hàng khỏi database
            DataProvider.Ins.DB.Orders.Remove(SelectedOrder);
            DataProvider.Ins.DB.SaveChanges();
            Orders.Remove(SelectedOrder);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
