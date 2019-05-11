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
    public  class SuplierViewModel:BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private ObservableCollection<Suplier> _SuplierList;
        public ObservableCollection<Suplier> SuplierList
        {
            get => _SuplierList;
            set
            {
                _SuplierList = value;
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

        private Suplier _SelectedItem;
        public Suplier SelectedItem
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

        public SuplierViewModel()
        {
            SuplierList = new ObservableCollection<Suplier>(DataProvider.Ins.DB.Supliers);

            EditCommand = new RelayCommand<object>((p) =>
            {
                //điều kiện chạy button edit
                if (SelectedItem == null )
                    return false;
                var idList = DataProvider.Ins.DB.Supliers.Where(f => f.Id == SelectedItem.Id);
                if (idList != null && idList.Count() != 0)
                    return true;

                return false;
            }, (p) =>
            {
                var suplier = DataProvider.Ins.DB.Supliers.Where(f => f.Id == SelectedItem.Id).SingleOrDefault();
                suplier.DisplayName = DisplayName;
                suplier.Address = Address;
                suplier.ContractDate = ContractDate;
                suplier.Email = Email;
                suplier.Phone = Phone;
                suplier.MoreInfor = MoreInfo;
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem.DisplayName = DisplayName;
                SelectedItem.ContractDate = ContractDate;
                SelectedItem.Email = Email;
                SelectedItem.Phone = Phone;
                SelectedItem.MoreInfor = MoreInfo;
                OnPropertyChanged("SuplierList");
            });

            AddCommand = new RelayCommand<object>((p) =>
            {
                //điểu kiện chạy button add
                if (string.IsNullOrEmpty(DisplayName))
                    return false;

                var displayList = DataProvider.Ins.DB.Supliers.Where(k => k.DisplayName == DisplayName);
                if (displayList == null || displayList.Count() != 0)
                    return false;

                return true;

            }, (p) =>
            {
                var suplier = new Suplier() { DisplayName = DisplayName,Address=Address,Email=Email,Phone=Phone,MoreInfor=MoreInfo,ContractDate=ContractDate };
                DataProvider.Ins.DB.Supliers.Add(suplier);
                DataProvider.Ins.DB.SaveChanges();
                SuplierList.Add(suplier);
            });


        }
    }
}
