using SalesManagement.Model;
using SalesManagement.View;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
        public ICommand ForgotPasswordCommand { get; set; } // Thêm lệnh ForgotPassword
        public ICommand CloseCommand { get; set; }

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

            // Command để quên mật khẩu
            ForgotPasswordCommand = new RelayCommand<object>((p) =>
            {
                return !string.IsNullOrEmpty(Username); // Điều kiện để nút có thể nhấn là Username không rỗng
            }, (p) => { ForgotPassword(); });

            // Command để đóng cửa sổ
            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });
        }

        // Hàm đăng nhập
        private void Login(Window p)
        {
            if (p == null)
                return;

            var accCount = DataProvider.Ins.DB.Users.Where(x => x.Username == Username && x.Password == Password).Count();

            if (accCount > 0)
            {
                IsLogin = true;
                MainView mainView = new MainView();
                mainView.Show();
                p.Close();
            }
            else
            {
                IsLogin = false;
                MessageBox.Show("Sai tài khoản hoặc mật khẩu");
            }
        }

        // Hàm quên mật khẩu
        private void ForgotPassword()
        {
            // Giả sử bạn muốn tìm người dùng theo Username để gửi lại mật khẩu
            var user = DataProvider.Ins.DB.Users.FirstOrDefault(x => x.Username == Username);

            if (user != null)
            {
                // Giả sử mật khẩu là dạng plain text (không an toàn), bạn có thể gửi lại
                MessageBox.Show($"Mật khẩu của bạn là: {user.Password}");
            }
            else
            {
                MessageBox.Show("Không tìm thấy tài khoản với tên người dùng này.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
