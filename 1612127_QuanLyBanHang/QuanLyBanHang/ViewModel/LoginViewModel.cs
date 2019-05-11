using QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLyBanHang.ViewModel
{
    class LoginViewModel : BaseViewModel
    {
        private string _UserName;
        public string UserName { get => _UserName; set { _UserName = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
        public ICommand LoginCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public bool isLogin { get; set; }
        private User _User;
        public User User { get => _User; set { _User = value; OnPropertyChanged(); } }

        //xử lý mã hóa mật khẩu

        public static string Base64Encode(string plainText)//string to base64
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }



        public static string MD5Hash(string input)//base64 to MD5 code
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public LoginViewModel()
        {
            isLogin = false;
            Password = "";
            UserName = "";

            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });//thoát chương trình
            LoginCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { login(p); });//click Login Button xử lý đăng nhập
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });//xử lý sự kiện lấy mật khẩu

        }

        void login(Window p)
        {
            if (p!=null)
            {
                string encodePass = MD5Hash(Base64Encode(Password));

                var count = DataProvider.Ins.DB.Users.Where(k => k.Username == UserName && k.Password == encodePass).Count();

                if (count > 0)
                {
                    User = DataProvider.Ins.DB.Users.Where(k => k.Username == UserName && k.Password == encodePass).SingleOrDefault();
                    isLogin = true;

                    p.Close();
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!");
                }
            }
        }





    }
    
}
