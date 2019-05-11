using QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuanLyBanHang.ViewModel
{
    class WarehouseViewModel : BaseViewModel
    {


        private ObservableCollection<Inventory> _InventoryList;
        public ObservableCollection<Inventory> InventoryList
        {
            get => _InventoryList;
            set
            {
                _InventoryList = value;
                OnPropertyChanged();
            }
        }
        void LoadInventoryData()
        {
            InventoryList = new ObservableCollection<Inventory>();

            var objectList = DataProvider.Ins.DB.Objects;

            int i = 1;
            foreach (var item in objectList)
            {
                var importList = DataProvider.Ins.DB.ImportInfoes.Where(p => p.IdObject == item.Id);
                var outputList = DataProvider.Ins.DB.BillInfoes.Where(p => p.IdObject == item.Id);

                int importQuantity = 0;
                int outQuantity = 0;

                if (importList != null && importList.Count() > 0)
                {
                    importQuantity = (int)importList.Sum(p => p.Quantity);
                }
                if (outputList != null && outputList.Count() > 0)
                {
                    outQuantity = (int)outputList.Sum(p => p.Count);
                }

                Inventory inventory = new Inventory();
                inventory.Ordinal = i;
                inventory.Quantity = importQuantity - outQuantity;
                inventory.Product = item;

                InventoryList.Add(inventory);
                i++;
            }

        }

        public WarehouseViewModel()
        {
            LoadInventoryData();
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(InventoryList);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Product.Suplier.DisplayName");
            view.GroupDescriptions.Add(groupDescription);
        }


    }
}
