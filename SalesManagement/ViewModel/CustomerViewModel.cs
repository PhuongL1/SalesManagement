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
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Customer> _Customers;
        public ObservableCollection<Customer> Customers
        {
            get { return _Customers; }
            set
            {
                _Customers = value;
                OnPropertyChanged(nameof(Customers));
            }
        }

        private Customer _SelectedCustomer;
        public Customer SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }

        // Commands
        public ICommand AddCustomerCommand { get; set; }
        public ICommand EditCustomerCommand { get; set; }
        public ICommand DeleteCustomerCommand { get; set; }

        public CustomerViewModel()
        {
            // Load danh sách khách hàng từ DB
            Customers = new ObservableCollection<Customer>(DataProvider.Ins.DB.Customers.ToList());

            // Khởi tạo các command
            AddCustomerCommand = new RelayCommand<object>((p) => true, (p) => AddCustomer());
            EditCustomerCommand = new RelayCommand<object>((p) => SelectedCustomer != null, (p) => EditCustomer());
            DeleteCustomerCommand = new RelayCommand<object>((p) => SelectedCustomer != null, (p) => DeleteCustomer());
        }

        private void AddCustomer()
        {
            // Logic thêm khách hàng vào database
            var newCustomer = new Customer()
            {
                FullName = "New Customer",
                Phone = "000000000",
                Email = "example@mail.com",
                Address = "123 Street"
            };
            DataProvider.Ins.DB.Customers.Add(newCustomer);
            DataProvider.Ins.DB.SaveChanges();
            Customers.Add(newCustomer);
        }

        private void EditCustomer()
        {
            // Logic chỉnh sửa khách hàng đã chọn
            DataProvider.Ins.DB.SaveChanges();
        }

        private void DeleteCustomer()
        {
            // Logic xóa khách hàng khỏi database
            DataProvider.Ins.DB.Customers.Remove(SelectedCustomer);
            DataProvider.Ins.DB.SaveChanges();
            Customers.Remove(SelectedCustomer);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
