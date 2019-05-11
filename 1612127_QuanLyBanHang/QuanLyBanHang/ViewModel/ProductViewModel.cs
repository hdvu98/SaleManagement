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
    class ProductViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private ObservableCollection<Model.Object> _ProductList;
        public ObservableCollection<Model.Object> ProductList
        {
            get => _ProductList;
            set
            {
                _ProductList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Unit> _Unit;
        public ObservableCollection<Unit> Unit
        {
            get => _Unit;
            set
            {
                _Unit = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Suplier> _Suplier;
        public ObservableCollection<Suplier> Suplier
        {
            get => _Suplier;
            set
            {
                _Suplier = value;
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

        private Suplier _SelectedSuplier;
        public Suplier SelectedSuplier
        {
            get => _SelectedSuplier;
            set
            {
                _SelectedSuplier = value;
                OnPropertyChanged();
            }
        }

        private Unit _SelectedUnit;
        public Unit SelectedUnit
        {
            get => _SelectedUnit;
            set
            {
                _SelectedUnit = value;
                OnPropertyChanged();
            }
        }



        private Model.Object _SelectedItem;
        public Model.Object SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    SelectedSuplier = SelectedItem.Suplier;
                    SelectedUnit = SelectedItem.Unit;

                }
            }
        }

        public ProductViewModel()
        {
            ProductList = new ObservableCollection<Model.Object>(DataProvider.Ins.DB.Objects);
            Unit = new ObservableCollection<Unit>(DataProvider.Ins.DB.Units);
            Suplier = new ObservableCollection<Suplier>(DataProvider.Ins.DB.Supliers);

            EditCommand = new RelayCommand<object>((p) =>
            {
                //điều kiện chạy button edit
                if (SelectedItem == null)
                    return false;
                var list = DataProvider.Ins.DB.Objects.Where(f => f.Id == SelectedItem.Id);
                if (list != null &&list.Count() != 0)
                    return true;

                return false;
            }, (p) =>
            {
                var suplier = DataProvider.Ins.DB.Objects.Where(f => f.Id == SelectedItem.Id).SingleOrDefault();
                suplier.DisplayName = DisplayName;
                suplier.Unit = SelectedUnit;
                suplier.Suplier = _SelectedSuplier;      
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem.DisplayName = DisplayName;
                SelectedItem.Unit = SelectedUnit;
                SelectedItem.Suplier = _SelectedSuplier;
                OnPropertyChanged("ProductList");
            });

            AddCommand = new RelayCommand<object>((p) =>
            {
                //điểu kiện chạy button add
                if (string.IsNullOrEmpty(DisplayName))
                    return false;

                var displayList = DataProvider.Ins.DB.Objects.Where(k => k.DisplayName == DisplayName);
                if (displayList == null || displayList.Count() != 0)
                    return false;

                return true;

            }, (p) =>
            {
                var product = new Model.Object() {Id=Guid.NewGuid().ToString(), DisplayName = DisplayName,IdUnit=SelectedUnit.Id,IdSuplier=SelectedSuplier.Id, Unit=SelectedUnit,Suplier=SelectedSuplier};
                DataProvider.Ins.DB.Objects.Add(product);
                DataProvider.Ins.DB.SaveChanges();
                ProductList.Add(product);
            });


        }
    }
}
