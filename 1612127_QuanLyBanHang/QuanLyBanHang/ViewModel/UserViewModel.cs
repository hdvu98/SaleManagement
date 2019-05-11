using QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace QuanLyBanHang.ViewModel
{
    public class UserViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private ObservableCollection<User> _UserList;
        public ObservableCollection<User> UserList
        {
            get => _UserList;
            set
            {
                _UserList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<UserRole> _UserRole;
        public ObservableCollection<UserRole> UserRole
        {
            get => _UserRole;
            set
            {
                _UserRole = value;
                OnPropertyChanged();
            }
        }


        private string _DisplayName;
        public string DisplayName
        {
            get => _DisplayName;
            set
            {
                _DisplayName = value;
                OnPropertyChanged();
            }
        }

        private string _DisplayRoleName;
        public string DisplayRoleName
        {
            get => _DisplayRoleName;
            set
            {
                _DisplayRoleName = value;
                OnPropertyChanged();
            }
        }      private string _Username;
 
         public string Username
        {
            get => _Username;
            set
            {
                _Username = value;
                OnPropertyChanged();
            }
        }

        private User _SelectedItem;
        public User SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    Username = SelectedItem.Username;
                    SelectedRole = SelectedItem.UserRole;   

                }
            }
        }

        private UserRole _SelectedRole;
        public UserRole SelectedRole
        {
            get => _SelectedRole;
            set
            {
                _SelectedRole = value;
                OnPropertyChanged();
           
            }
        }

        public UserViewModel()
        {
            UserList = new ObservableCollection<User>(DataProvider.Ins.DB.Users);
            UserRole = new ObservableCollection<UserRole>(DataProvider.Ins.DB.UserRoles);
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(UserList);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("UserRole.DisplayName");
            view.GroupDescriptions.Add(groupDescription);
            EditCommand = new RelayCommand<object>((p) =>
            {
                //điều kiện chạy button edit
                if (SelectedItem == null||SelectedItem.IdRole==1)
                    return false;
                var idList = DataProvider.Ins.DB.Users.Where(f => f.Id == SelectedItem.Id);
                if (idList != null && idList.Count() != 0)
                    return true;
                return false;
            }, (p) =>
            {
                var user = DataProvider.Ins.DB.Users.Where(f => f.Id == SelectedItem.Id).SingleOrDefault();
                user.DisplayName = DisplayName;
                user.Username = Username;
                user.UserRole = SelectedRole;
                DataProvider.Ins.DB.SaveChanges();
                OnPropertyChanged("UserList");
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                //điều kiện chạy button edit
                if (SelectedItem == null)
                    return false;
                if (SelectedItem.Id == 1)
                    return false;
                var idList = DataProvider.Ins.DB.Users.Where(f => f.Id == SelectedItem.Id);
                if (idList != null && idList.Count() != 0)
                    return true;
                return false;
            }, (p) =>
            {
                var user = DataProvider.Ins.DB.Users.Where(f => f.Id == SelectedItem.Id).SingleOrDefault();
                if(user!=null)
                {
                    DataProvider.Ins.DB.Users.Remove(user);
                }
                DataProvider.Ins.DB.SaveChanges();
                UserList.Remove(user);
                OnPropertyChanged("UserList");
            });

            AddCommand = new RelayCommand<object>((p) =>
            {
                //điểu kiện chạy button add
                if (string.IsNullOrEmpty(DisplayName))
                    return false;

                var list = DataProvider.Ins.DB.Users.Where(k => k.Username == Username);
                if (list == null || list.Count() != 0)
                    return false;

                return true;

            }, (p) =>
            {
                var user = new User() { DisplayName = DisplayName, Username=Username,UserRole=SelectedRole };
                user.Password= MD5Hash(Base64Encode(Username));
                DataProvider.Ins.DB.Users.Add(user);
                DataProvider.Ins.DB.SaveChanges();
                UserList.Add(user);
                MessageBox.Show("Tạo tài khoản thành công!\nMật khẩu mặc định trùng với Username");
            });

        }

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
    }
}
