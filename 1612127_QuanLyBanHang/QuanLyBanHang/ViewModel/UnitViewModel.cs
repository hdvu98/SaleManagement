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
    class UnitViewModel:BaseViewModel
    {

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        private ObservableCollection<Unit> _UnitList;
        public ObservableCollection<Unit> UnitList
        {
            get => _UnitList;
            set {
                _UnitList = value;
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

        private Unit _SelectedItem;
        public Unit SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                }
            }
        }

        public UnitViewModel()
        {
            UnitList = new ObservableCollection<Unit>(DataProvider.Ins.DB.Units);

            EditCommand = new RelayCommand<object>((p)=>
            {
                //điều kiện chạy button edit
                if (SelectedItem == null )
                    return false;
                var displayList = DataProvider.Ins.DB.Units.Where(f => f.DisplayName == DisplayName);
                if (displayList == null || displayList.Count() != 0)
                    return false;

                return true;
            },(p)=>
            {
                var findUnit = DataProvider.Ins.DB.Units.Where(f => f.Id == SelectedItem.Id).SingleOrDefault();
                findUnit.DisplayName = DisplayName;
                DataProvider.Ins.DB.SaveChanges();
                SelectedItem.DisplayName = DisplayName;
                OnPropertyChanged("UnitList");
            });

            AddCommand = new RelayCommand<object>((p) =>
            {
                return true;

            }, (p) =>
            {
                var unit = new Unit() { DisplayName = DisplayName };

                DataProvider.Ins.DB.Units.Add(unit);
                DataProvider.Ins.DB.SaveChanges();
                UnitList.Add(unit);
            });


        }

    }
}
