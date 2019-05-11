using QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyBanHang.ViewModel
{
    class CustomerViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private ObservableCollection<Customer> _CustomerList;
        public ObservableCollection<Customer> CustomerList
        {
            get => _CustomerList;
            set
            {
                _CustomerList = value;
                OnPropertyChanged();
            }
        }

        private string _Address;
        public string Address
        {
            get => _Address;
            set
            {
                _Address = value;
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

        private string _Phone;
        public string Phone
        {
            get => _Phone;
            set
            {
                _Phone = value;
                OnPropertyChanged();
            }
        }

        private string _MoreInfo;
        public string MoreInfo
        {
            get => _MoreInfo;
            set
            {
                _MoreInfo = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _ContractDate;
        public DateTime? ContractDate
        {
            get => _ContractDate;
            set
            {
                _ContractDate = value;
                OnPropertyChanged();
            }
        }

        private string _Email;
        public string Email
        {
            get => _Email;
            set
            {
                _Email = value;
                OnPropertyChanged();
            }
        }

        private Customer _SelectedItem;
        public Customer SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    Address = SelectedItem.Address;
                    ContractDate = SelectedItem.ContractDate;
                    Email = SelectedItem.Email;
                    Phone = SelectedItem.Phone;
                    MoreInfo = SelectedItem.MoreInfor;
                    ContractDate = SelectedItem.ContractDate;

                }
            }
        }

        public CustomerViewModel()
        {
            CustomerList = new ObservableCollection<Customer>(DataProvider.Ins.DB.Customers);

            EditCommand = new RelayCommand<object>((p) =>
            {
                //điều kiện chạy button edit
                if (SelectedItem == null)
                    return false;
                var idList = DataProvider.Ins.DB.Customers.Where(f => f.Id == SelectedItem.Id);
                if (idList != null && idList.Count() != 0)
                    return true;

                return false;
            }, (p) =>
            {
                var customer = DataProvider.Ins.DB.Customers.Where(f => f.Id == SelectedItem.Id).SingleOrDefault();
                customer.DisplayName = DisplayName;
                customer.Address = Address;
                customer.ContractDate = ContractDate;
                customer.Email = Email;
                customer.Phone = Phone;
                customer.MoreInfor = MoreInfo;
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem.DisplayName = DisplayName;
                SelectedItem.ContractDate = ContractDate;
                SelectedItem.Email = Email;
                SelectedItem.Phone = Phone;
                SelectedItem.MoreInfor = MoreInfo;
                OnPropertyChanged("CustomerList");
            });

            AddCommand = new RelayCommand<object>((p) =>
            {
                //điểu kiện chạy button add
                if (string.IsNullOrEmpty(DisplayName))
                    return false;

                var displayList = DataProvider.Ins.DB.Customers.Where(k => k.DisplayName == DisplayName);
                if (displayList == null || displayList.Count() != 0)
                    return false;

                return true;

            }, (p) =>
            {
                var customer = new Customer() { DisplayName = DisplayName, Address = Address, Email = Email, Phone = Phone, MoreInfor = MoreInfo, ContractDate = ContractDate };
                DataProvider.Ins.DB.Customers.Add(customer);
                DataProvider.Ins.DB.SaveChanges();
                CustomerList.Add(customer);
            });

          
        }
    }
}
