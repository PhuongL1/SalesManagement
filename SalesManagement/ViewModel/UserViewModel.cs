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
    public class UserViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; set; }
        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }

        public ICommand AddUserCommand { get; set; }
        public ICommand UpdateUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }

        public UserViewModel()
        {
            Users = new ObservableCollection<User>(DataProvider.Ins.DB.Users.ToList());

            AddUserCommand = new RelayCommand<object>((p) => true, (p) => AddUser());
            UpdateUserCommand = new RelayCommand<object>((p) => SelectedUser != null, (p) => UpdateUser());
            DeleteUserCommand = new RelayCommand<object>((p) => SelectedUser != null, (p) => DeleteUser());
        }

        private void AddUser()
        {
            var newUser = new User { Username = "newuser", Password = "password", Role = "User" };
            DataProvider.Ins.DB.Users.Add(newUser);
            DataProvider.Ins.DB.SaveChanges();
            Users.Add(newUser);
        }

        private void UpdateUser()
        {
            DataProvider.Ins.DB.SaveChanges();
        }

        private void DeleteUser()
        {
            DataProvider.Ins.DB.Users.Remove(SelectedUser);
            DataProvider.Ins.DB.SaveChanges();
            Users.Remove(SelectedUser);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
