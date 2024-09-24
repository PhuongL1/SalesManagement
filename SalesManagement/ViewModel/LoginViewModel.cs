using SalesManagement.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using SalesManagement.View;
using System.Security.Cryptography;

namespace SalesManagement.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public bool IsLogin { get; set; }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        public LoginViewModel()
        {
            IsLogin = false;
            Password = "";
            Username = "";

            // Command để xử lý đăng nhập
            LoginCommand = new RelayCommand<Window>((p) =>
            {
                return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
            }, (p) => { Login(p); });

            // Command để đóng cửa sổ
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });

            // Command để cập nhật Password từ PasswordBox
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
        }

        // Hàm đăng nhập
        private void Login(Window p)
        {
            // Xử lý logic đăng nhập
            if (p == null)
                return;

            var accCount = DataProvider.Ins.DB.Users.Where(x => x.Username == Username && x.Password == Password).Count();

            if (accCount > 0)  // Nếu tìm thấy tài khoản hợp lệ
            {
                IsLogin = true;

                // Mở giao diện MainView (thay vì ProductView)
                MainView mainView = new MainView();
                mainView.Show();

                // Đóng giao diện đăng nhập
                p.Close();
            }
            else
            {
                IsLogin = false;
                MessageBox.Show("Sai tài khoản hoặc mật khẩu");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
