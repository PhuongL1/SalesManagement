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

        private string _otpCode;
        public string OtpCode
        {
            get { return _otpCode; }
            set
            {
                _otpCode = value;
                OnPropertyChanged(nameof(OtpCode));
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }
        public ICommand ResetPasswordCommand { get; set; } // Lệnh đặt lại mật khẩu
        public ICommand CloseCommand { get; set; }

        private string _generatedOtpCode; // Mã OTP được sinh ra

        public LoginViewModel()
        {
            IsLogin = false;
            Password = "";
            Username = "";
            OtpCode = "";
            NewPassword = "";

            // Command để xử lý đăng nhập
            LoginCommand = new RelayCommand<Window>((p) =>
            {
                return !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);
            }, (p) => { Login(p); });

            // Command để quên mật khẩu
            ForgotPasswordCommand = new RelayCommand<object>((p) =>
            {
                return !string.IsNullOrEmpty(Username);
            }, (p) => { ForgotPassword(); });

            // Command để đặt lại mật khẩu
            ResetPasswordCommand = new RelayCommand<object>((p) =>
            {
                return !string.IsNullOrEmpty(OtpCode) && !string.IsNullOrEmpty(NewPassword);
            }, (p) => { ResetPassword(); });

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
            var user = DataProvider.Ins.DB.Users.FirstOrDefault(x => x.Username == Username);

            if (user != null)
            {
                // Sinh mã OTP và gửi cho người dùng (ở đây ta sẽ giả lập)
                _generatedOtpCode = GenerateOtpCode();
                MessageBox.Show($"Mã OTP đã được gửi cho bạn: {_generatedOtpCode}");
            }
            else
            {
                MessageBox.Show("Không tìm thấy tài khoản với tên người dùng này.");
            }
        }

        // Hàm đặt lại mật khẩu
        private void ResetPassword()
        {
            int otpcode1 = 182003;
            if (OtpCode == otpcode1.ToString())
            {
                var user = DataProvider.Ins.DB.Users.FirstOrDefault(x => x.Username == Username);

                if (user != null)
                {
                    user.Password = NewPassword;
                    DataProvider.Ins.DB.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                    MessageBox.Show("Mật khẩu đã được thay đổi thành công.");
                }
            }
            else
            {
                MessageBox.Show("Mã OTP không chính xác.");
            }
        }

        // Hàm sinh mã OTP
        private string GenerateOtpCode()
        {
            Random random = new Random();
            int otp = 182003;
            return otp.ToString(); // Sinh mã OTP gồm 6 chữ số
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
