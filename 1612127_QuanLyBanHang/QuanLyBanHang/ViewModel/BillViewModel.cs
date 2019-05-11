using QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace QuanLyBanHang.ViewModel
{
    class BillViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ExportCommand { get; set; }
        public ICommand CreateNewCommand { get; set; }

        private ObservableCollection<Model.Object> _Product;
        public ObservableCollection<Model.Object> Product
        {
            get => _Product;
            set
            {
                _Product = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Customer> _Customer;
        public ObservableCollection<Customer> Customer
        {
            get => _Customer;
            set
            {
                _Customer = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<ImportInfo> _ImportInfo;
        public ObservableCollection<ImportInfo> ImportInfo
        {
            get => _ImportInfo;
            set
            {
                _ImportInfo = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<BillInfo> _BillInfoList;
        public ObservableCollection<BillInfo> BillInfoList
        {
            get => _BillInfoList;
            set
            {
                _BillInfoList = value;
                OnPropertyChanged();
            }
        }


        private Model.Object _SelectedProduct;
        public Model.Object SelectedProduct
        {
            get => _SelectedProduct;
            set
            {
                _SelectedProduct = value;
                ImportInfo = new ObservableCollection<ImportInfo>(DataProvider.Ins.DB.ImportInfoes.Where(f=>f.IdObject==SelectedProduct.Id));
                OnPropertyChanged();
            }
        }

        private ImportInfo _SelectedImportInfo;//Nguồn hàng được chọn
        public ImportInfo SelectedImportInfo
        {
            get => _SelectedImportInfo;
            set
            {
                _SelectedImportInfo = value;
                RetailPrice = SelectedImportInfo.RetailPrice;
                OnPropertyChanged();
               
             }
        }

        private Customer _SelectedCustomer;
        public Customer SelectedCustomer
        {
            get => _SelectedCustomer;
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _DateCreate;
        public DateTime? DateCreate
        {
            get => _DateCreate;
            set
            {
                _DateCreate = value;
                OnPropertyChanged();
            }
        }


        private Nullable<int> _Quantity;
        public Nullable<int> Quantity
        {
            get => _Quantity;
            set
            {
                _Quantity = value;
                OnPropertyChanged();
            }
        }


        private Nullable<double> _RetailPrice;
        public Nullable<double> RetailPrice
        {
            get => _RetailPrice;
            set
            {
                _RetailPrice = value;
                OnPropertyChanged();
            }
        }

        private Nullable<double> _Sum;
        public Nullable<double> Sum
        {
            get => _Sum;
            set
            {
                _Sum = value;
                OnPropertyChanged();
            }
        }



        private BillInfo _SelectedItem;
        public BillInfo SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    SelectedProduct = SelectedItem.Object;
                    Quantity = Quantity;
                    RetailPrice = SelectedItem.ImportInfo.RetailPrice;
                    SelectedCustomer = SelectedItem.Bill.Customer;
                    SelectedImportInfo = SelectedItem.ImportInfo;
                    DateCreate = SelectedItem.Bill.DateCreate;
                    Quantity = SelectedItem.Count;
                }
            }
        }

        public BillViewModel()
        {
            BillInfoList = new ObservableCollection<BillInfo>(DataProvider.Ins.DB.BillInfoes);
            Customer = new ObservableCollection<Customer>(DataProvider.Ins.DB.Customers);
            Product = new ObservableCollection<Model.Object>(DataProvider.Ins.DB.Objects);
            //ImportInfo = new ObservableCollection<ImportInfo>(DataProvider.Ins.DB.ImportInfoes);

            //tạo mã id cho hóa đơn nhập hàng
            string generateImportID = Guid.NewGuid().ToString();
            var bill = new Bill() { Id = generateImportID };

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(BillInfoList);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("IdBill");
            view.GroupDescriptions.Add(groupDescription);

            EditCommand = new RelayCommand<object>((p) =>
            {
                //điều kiện chạy button edit
                if (SelectedItem == null)
                    return false;
                var list = DataProvider.Ins.DB.BillInfoes.Where(f => f.Id == SelectedItem.Id);
                if (list != null || list.Count() != 0)
                    return true;

                return false;
            }, (p) =>
            {

                var billUpdate = DataProvider.Ins.DB.Bills.Where(f => f.Id == SelectedItem.IdBill).SingleOrDefault();
                if (billUpdate != null)
                {
                    billUpdate.Customer = SelectedCustomer;
                    billUpdate.DateCreate = DateCreate;
                    DataProvider.Ins.DB.SaveChanges();

                }
                var billInfo = DataProvider.Ins.DB.BillInfoes.Where(f => f.Id == SelectedItem.Id).SingleOrDefault();
                billInfo.Object = SelectedProduct;
                billInfo.Bill.DateCreate = DateCreate;
                billInfo.ImportInfo = SelectedImportInfo;
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem.Object = SelectedProduct;
                SelectedItem.RetailPrice = RetailPrice;
                SelectedItem.Bill.DateCreate = DateCreate;
                SelectedItem.Count = Quantity;
                SelectedItem.Sum = RetailPrice * Quantity;
                OnPropertyChanged("BillInfoList");
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                //điều kiện chạy button edit
                if (SelectedItem == null)
                    return false;
                var list = DataProvider.Ins.DB.BillInfoes.Where(f => f.Id == SelectedItem.Id);
                if (list != null || list.Count() != 0)
                    return true;

                return false;
            }, (p) =>
            {

                var billRemove = DataProvider.Ins.DB.BillInfoes.Where(f => f.Id == SelectedItem.Id).SingleOrDefault();

                if (billRemove != null)
                {
                    string billId = billRemove.IdBill;
                    var _bill = new Bill();
                    _bill = billRemove.Bill;
                    DataProvider.Ins.DB.BillInfoes.Remove(billRemove);
                    DataProvider.Ins.DB.SaveChanges();
                    BillInfoList.Remove(billRemove);
                    if(DataProvider.Ins.DB.BillInfoes.Where(k=>k.IdBill==billId).Count()==0)
                    {
                        DataProvider.Ins.DB.Bills.Remove(_bill);
                        DataProvider.Ins.DB.SaveChanges();
                    }
                }               
                OnPropertyChanged("BillInfoList");
            });

            ExportCommand = new RelayCommand<object>((p) =>
            {
                //điều kiện chạy button edit
                if (SelectedItem == null)
                    return false;

                var list = DataProvider.Ins.DB.BillInfoes.Where(f => f.Id == SelectedItem.Id);
                if (list != null || list.Count() != 0)
                    return true;

                return false;
            }, (p) =>
            {
                ViewBillWindow wd = new ViewBillWindow(); wd.ShowDialog();

            });

            CreateNewCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                generateImportID = Guid.NewGuid().ToString();
                bill = new Bill() { Id = generateImportID };

            });

            AddCommand = new RelayCommand<object>((p) =>
            {

            //điểu kiện chạy button add
            if (SelectedProduct == null || _SelectedImportInfo == null || Quantity == null|| Quantity<=0)
                    return false;

                var displayList = DataProvider.Ins.DB.BillInfoes.Where(k => k.Object.DisplayName == SelectedProduct.DisplayName &&k.IdBill==generateImportID);
                if (displayList == null || displayList.Count() != 0)
                    return false;

                return true;

            }, (p) =>
            {
                //đã tạo hóa đơn

                var isAppeareBillID = DataProvider.Ins.DB.Bills.Where(k => k.Id == generateImportID);
                if (isAppeareBillID != null && isAppeareBillID.Count() == 0)
                {
                    if (_SelectedCustomer != null)
                    {
                        bill.Customer = SelectedCustomer;
                        bill.IdCustomer = SelectedCustomer.Id;
                    }
                    else
                    {
                        var defaultCustomer = DataProvider.Ins.DB.Customers.Where(f => f.Id == 3).SingleOrDefault();
                        if(defaultCustomer!=null)
                        {
                            Customer customer = new Customer { DisplayName = "<Khách hàng mặc định>" };
                            bill.IdCustomer = customer.Id;
                            bill.Customer = customer;
                        }

                    }
     
                    DataProvider.Ins.DB.Bills.Add(bill);
                    DataProvider.Ins.DB.SaveChanges();
                }
                if (_SelectedCustomer != null)
                {
                    bill.Customer = SelectedCustomer;
                    bill.IdCustomer = SelectedCustomer.Id;
                }
                else
                {
                    var defaultCustomer = DataProvider.Ins.DB.Customers.Where(f => f.Id == 3).SingleOrDefault();
                    if (defaultCustomer != null)
                    {
                        Customer customer = new Customer { DisplayName = "<Khách hàng mặc định>" };
                        bill.IdCustomer = customer.Id;
                        bill.Customer = customer;
                    }

                }
                bill.DateCreate = DateCreate;
                DataProvider.Ins.DB.SaveChanges();

                var billInfo = new BillInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Object = SelectedProduct,
                    IdObject = SelectedProduct.Id,
                    RetailPrice = RetailPrice,
                    ImportInfo = SelectedImportInfo,
                    IdBill = generateImportID,
                    Count = Quantity,
                    Sum = Quantity * RetailPrice,
                    IdImportInfo = SelectedImportInfo.Id
                };

                DataProvider.Ins.DB.BillInfoes.Add(billInfo);
                DataProvider.Ins.DB.SaveChanges();
                BillInfoList.Add(billInfo);
            });


        }
    }
}
