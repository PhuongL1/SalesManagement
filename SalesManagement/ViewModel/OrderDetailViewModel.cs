using SalesManagement.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.ViewModel
{
    public class OrderDetailViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<OrderDetail> OrderDetails { get; set; }
        private OrderDetail _selectedOrderDetail;
        public OrderDetail SelectedOrderDetail
        {
            get { return _selectedOrderDetail; }
            set
            {
                _selectedOrderDetail = value;
                OnPropertyChanged(nameof(SelectedOrderDetail));
            }
        }

        public OrderDetailViewModel(int orderId)
        {
            OrderDetails = new ObservableCollection<OrderDetail>(DataProvider.Ins.DB.OrderDetails.Where(od => od.OrderID == orderId).ToList());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
