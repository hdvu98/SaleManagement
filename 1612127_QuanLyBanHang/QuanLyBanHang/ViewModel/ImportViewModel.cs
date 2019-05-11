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
    class ImportViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

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

        private ObservableCollection<Import> _Import;
        public ObservableCollection<Import> Import
        {
            get => _Import;
            set
            {
                _Import = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ImportInfo> _ImportInfoList;
        public ObservableCollection<ImportInfo> ImportInfoList
        {
            get => _ImportInfoList;
            set
            {
                _ImportInfoList = value;
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
                OnPropertyChanged();
            }
        }

        private DateTime? _DateImport;
        public DateTime? DateImport
        {
            get => _DateImport;
            set
            {
                _DateImport = value;
                OnPropertyChanged();
            }
        }


        private int _Quantity;
        public int Quantity
        {
            get => _Quantity;
            set
            {
                _Quantity = value;
                OnPropertyChanged();
            }
        }

        private Nullable<double> _InputPrice;
        public Nullable<double> InputPrice
        {
            get => _InputPrice;
            set
            {
                _InputPrice = value;
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



        private ImportInfo _SelectedItem;
        public ImportInfo SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    SelectedProduct = SelectedItem.Object;
                    Quantity = SelectedItem.Quantity;
                    InputPrice = SelectedItem.InputPrice;
                    RetailPrice = SelectedItem.RetailPrice;
                    DateImport = SelectedItem.Import.DateImport;

                }
            }
        }

        public ImportViewModel()
        {
            Product = new ObservableCollection<Model.Object>(DataProvider.Ins.DB.Objects);
            ImportInfoList = new ObservableCollection<ImportInfo>(DataProvider.Ins.DB.ImportInfoes);

            //tạo mã id cho hóa đơn nhập hàng
            string generateImportID = Guid.NewGuid().ToString();
            var import = new Import() { Id = generateImportID };
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ImportInfoList);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("IdImport");
            view.GroupDescriptions.Add(groupDescription);

            EditCommand = new RelayCommand<object>((p) =>
            {
                //điều kiện chạy button edit
                if (SelectedItem == null)
                    return false;
                var list = DataProvider.Ins.DB.ImportInfoes.Where(f => f.Id == SelectedItem.Id);
                if (list != null || list.Count() != 0)
                    return true;

                return false;
            }, (p) =>
            {

                var importUpdate = DataProvider.Ins.DB.Imports.Where(f => f.Id == SelectedItem.IdImport).SingleOrDefault();
                if (importUpdate != null)
                {
                    importUpdate.DateImport = DateImport;
                    DataProvider.Ins.DB.SaveChanges();

                }
                var importInfo = DataProvider.Ins.DB.ImportInfoes.Where(f => f.Id == SelectedItem.Id).SingleOrDefault();
                importInfo.Object = SelectedProduct;
                importInfo.Import.DateImport = DateImport;
                importInfo.Import = import;
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem.Object = SelectedProduct;
                SelectedItem.InputPrice = InputPrice;
                SelectedItem.RetailPrice = RetailPrice;
                SelectedItem.Import.DateImport = DateImport;
                OnPropertyChanged("ImportInfoList");
            });

            AddCommand = new RelayCommand<object>((p) =>
            {

                //điểu kiện chạy button add
                if (string.IsNullOrEmpty(SelectedProduct.DisplayName))
                    return false;

            if (Quantity == null || Quantity <= 0 || InputPrice == null||InputPrice<=0||RetailPrice<=0||RetailPrice==null)
                    return false;

                var displayList = DataProvider.Ins.DB.ImportInfoes.Where(k => k.Object.DisplayName== SelectedProduct.DisplayName);
                if (displayList == null)
                    return false;

                return true;

            }, (p) =>
            {
                //đã tạo biên lai nhập hàng
                import.DateImport = DateImport;
                var isAppearedImportID = DataProvider.Ins.DB.Imports.Where(k => k.Id == generateImportID);
                if (isAppearedImportID != null && isAppearedImportID.Count() == 0)
                {
                    DataProvider.Ins.DB.Imports.Add(import);
                    DataProvider.Ins.DB.SaveChanges();
                }
                DataProvider.Ins.DB.SaveChanges();

                var importInfo = new ImportInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    Object = SelectedProduct,
                    IdObject = SelectedProduct.Id,
                    InputPrice = InputPrice,
                    RetailPrice = RetailPrice,
                    Import = import,
                    IdImport = import.Id,
                    Quantity = Quantity
                };

                DataProvider.Ins.DB.ImportInfoes.Add(importInfo);
                DataProvider.Ins.DB.SaveChanges();
                ImportInfoList.Add(importInfo);

            });
        }
    }
}
