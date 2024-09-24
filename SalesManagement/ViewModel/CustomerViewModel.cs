using SalesManagement.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

                // Khi chọn một khách hàng, các giá trị sẽ được tự động gán vào các ô nhập liệu
                if (SelectedCustomer != null)
                {
                    NewCustomerName = SelectedCustomer.FullName;
                    NewCustomerPhone = SelectedCustomer.Phone;
                    NewCustomerEmail = SelectedCustomer.Email;
                    NewCustomerAddress = SelectedCustomer.Address;
                }
            }
        }

        // Thuộc tính cho các ô nhập liệu
        private string _NewCustomerName;
        public string NewCustomerName
        {
            get { return _NewCustomerName; }
            set
            {
                _NewCustomerName = value;
                OnPropertyChanged(nameof(NewCustomerName));
            }
        }

        private string _NewCustomerPhone;
        public string NewCustomerPhone
        {
            get { return _NewCustomerPhone; }
            set
            {
                _NewCustomerPhone = value;
                OnPropertyChanged(nameof(NewCustomerPhone));
            }
        }

        private string _NewCustomerEmail;
        public string NewCustomerEmail
        {
            get { return _NewCustomerEmail; }
            set
            {
                _NewCustomerEmail = value;
                OnPropertyChanged(nameof(NewCustomerEmail));
            }
        }

        private string _NewCustomerAddress;
        public string NewCustomerAddress
        {
            get { return _NewCustomerAddress; }
            set
            {
                _NewCustomerAddress = value;
                OnPropertyChanged(nameof(NewCustomerAddress));
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
            AddCustomerCommand = new RelayCommand<object>((p) => CanAddCustomer(), (p) => AddCustomer());
            EditCustomerCommand = new RelayCommand<object>((p) => SelectedCustomer != null, (p) => EditCustomer());
            DeleteCustomerCommand = new RelayCommand<object>((p) => SelectedCustomer != null, (p) => DeleteCustomer());
        }

        private bool CanAddCustomer()
        {
            // Kiểm tra nếu các trường nhập liệu không rỗng thì cho phép thêm khách hàng
            return !string.IsNullOrEmpty(NewCustomerName) &&
                   !string.IsNullOrEmpty(NewCustomerPhone) &&
                   !string.IsNullOrEmpty(NewCustomerEmail) &&
                   !string.IsNullOrEmpty(NewCustomerAddress);
        }

        private void AddCustomer()
        {
            // Thêm khách hàng mới vào database
            var newCustomer = new Customer()
            {
                FullName = NewCustomerName,
                Phone = NewCustomerPhone,
                Email = NewCustomerEmail,
                Address = NewCustomerAddress
            };

            DataProvider.Ins.DB.Customers.Add(newCustomer);
            DataProvider.Ins.DB.SaveChanges();
            Customers.Add(newCustomer);

            // Xóa các trường nhập sau khi thêm
            NewCustomerName = "";
            NewCustomerPhone = "";
            NewCustomerEmail = "";
            NewCustomerAddress = "";
        }

        private void EditCustomer()
        {
            // Cập nhật thông tin khách hàng đã chọn
            // Kiểm tra nếu khách hàng đã chọn không null
            if (SelectedCustomer != null)
            {
                // Cập nhật thông tin khách hàng đã chọn với dữ liệu từ các ô nhập liệu
                SelectedCustomer.FullName = NewCustomerName;
                SelectedCustomer.Phone = NewCustomerPhone;
                SelectedCustomer.Email = NewCustomerEmail;
                SelectedCustomer.Address = NewCustomerAddress;

                // Lưu thay đổi vào cơ sở dữ liệu
                DataProvider.Ins.DB.SaveChanges();

                // Cập nhật lại danh sách Customers từ cơ sở dữ liệu
                Customers = new ObservableCollection<Customer>(DataProvider.Ins.DB.Customers.ToList());

                // Ghi chú: Cần đảm bảo rằng SelectedCustomer là khách hàng đã chỉnh sửa
                // Cập nhật lại SelectedCustomer để phù hợp với danh sách
                SelectedCustomer = Customers.FirstOrDefault(c => c.CustomerID == SelectedCustomer.CustomerID);
            }
        }

        private void DeleteCustomer()
        {
            // Xóa khách hàng đã chọn
            if (SelectedCustomer != null)
            {
                DataProvider.Ins.DB.Customers.Remove(SelectedCustomer);
                DataProvider.Ins.DB.SaveChanges();
                Customers.Remove(SelectedCustomer);
                SelectedCustomer = null;

                // Xóa các trường nhập sau khi xóa
                NewCustomerName = "";
                NewCustomerPhone = "";
                NewCustomerEmail = "";
                NewCustomerAddress = "";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
