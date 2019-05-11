using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLyBanHang.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public bool Isloaded = false;
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand UnitCommand { get; set; }
        public ICommand SuplierCommand{ get; set; }
        public ICommand WarehouseCommand{ get; set; }
        public ICommand CustomerCommand{ get; set; }
        public ICommand BillCommand{ get; set; }
        public ICommand UserCommand{ get; set; }
        public ICommand ProductCommand{ get; set; }
        public ICommand ImportCommand{ get; set; }
        public ICommand StatisticalCommand{ get; set; }
        public ICommand LogoutCommand{ get; set; }
        private string _Name;
        public string Name { get => _Name; set { _Name = value;OnPropertyChanged(); } }

        private string _RoleName;
        public string RoleName { get => _RoleName; set { _RoleName = value; OnPropertyChanged(); } }

        public MainViewModel()
        {
            var loginVM = new LoginViewModel();  
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) => {
                Isloaded = true;
                if (p != null)
                {
                    p.Hide();
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.ShowDialog();

                    if (loginWindow.DataContext == null)
                        return;
                    loginVM = loginWindow.DataContext as LoginViewModel;
                    

                    if (loginVM.isLogin)
                    {
                        Name = loginVM.User.DisplayName;
                        RoleName = loginVM.User.UserRole.DisplayName;
                        p.Show();
                    }
                    else
                    {
                        p.Close();
                    }
                }
            }
            );
            UnitCommand = new RelayCommand<object>((p) => {
                    return true; }, (p) => { UnitWindow wd = new UnitWindow(); wd.ShowDialog(); });
            SuplierCommand = new RelayCommand<object>((p) => { return true; }, (p) => { SuplierWindow wd = new SuplierWindow(); wd.ShowDialog(); });
            WarehouseCommand = new RelayCommand<object>((p) => { return true; }, (p) => { WarehouseWindow wd = new WarehouseWindow(); wd.ShowDialog(); });
            CustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) => { CustomerWindow wd = new CustomerWindow(); wd.ShowDialog(); });
            BillCommand = new RelayCommand<object>((p) => { return true; }, (p) => { BillWindow wd = new BillWindow(); wd.ShowDialog(); });
            UserCommand = new RelayCommand<object>((p) => 
            {
                if (loginVM.User.UserRole.Id != 1)
                    return false;
                return true;
            }, (p) => { UserWindow wd = new UserWindow(); wd.ShowDialog(); });
            ProductCommand = new RelayCommand<object>((p) => { return true; }, (p) => { ProductWindow wd = new ProductWindow(); wd.ShowDialog(); });
            ImportCommand = new RelayCommand<object>((p) => { return true; }, (p) => { ImportWindow wd = new ImportWindow(); wd.ShowDialog(); });
            StatisticalCommand = new RelayCommand<object>((p) => { return true; }, (p) => { StatisticalWindow wd = new StatisticalWindow(); wd.ShowDialog(); });
            LogoutCommand = new RelayCommand<object>((p) => { return true; }, (p) => { LoginWindow wd = new LoginWindow(); wd.ShowDialog(); });


        }
    }
}
