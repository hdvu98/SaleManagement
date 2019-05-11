using QuanLyBanHang.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Controls;

namespace QuanLyBanHang.ViewModel
{
    class StatisticalViewModel : BaseViewModel 
    {
        private SeriesCollection _SeriesCollection; 
        public SeriesCollection SeriesCollection { get=>_SeriesCollection; set { _SeriesCollection = value; OnPropertyChanged(); } }
        public string[] _Labels;
        public string[] Labels
        {
            get=>_Labels;
            set
            {
                _Labels = value;
                OnPropertyChanged();
            }
        }
        public Func<double, string> _YFormatter;
        public Func<double, string> YFormatter
        {
            get => _YFormatter;
            set
            {
                _YFormatter = value;
                OnPropertyChanged();
            }
        }

        public Func<double, string> _XFormatter;
        public Func<double, string> XFormatter
        {
            get => _XFormatter;
            set
            {
                _XFormatter = value;
                OnPropertyChanged();
            }
        }


        public ICommand ShowCommand { get; set; }
        private ObservableCollection<BillStatistic> _BillStatisticList;
        public ObservableCollection<BillStatistic> BillStatisticList
        {
            get => _BillStatisticList;
            set
            {
                _BillStatisticList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<BillByDate> _BillByDateList;
        public ObservableCollection<BillByDate> BillByDateList
        {
            get => _BillByDateList;
            set
            {
                _BillByDateList = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _DateBegin;
        public DateTime? DateBegin
        {
            get => _DateBegin;
            set
            {
                _DateBegin = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _DateEnd;
        public DateTime? DateEnd
        {
            get => _DateEnd;
            set
            {
                _DateEnd = value;
                OnPropertyChanged();
            }
        }

        private float _SumFunds;
        public float SumFunds
        {
            get => _SumFunds;
            set
            {
                _SumFunds = value;
                OnPropertyChanged();
            }
        }

        private float _SumRevenue;
        public float SumRevenue
        {
            get => _SumRevenue;
            set
            {
                _SumRevenue = value;
                OnPropertyChanged();
            }
        }

        private float _SumProfit;
        public float SumProfit
        {
            get => _SumProfit;
            set
            {
                _SumProfit = value;
                OnPropertyChanged();
            }
        }
        void LoadBillData()
        {
            BillStatisticList = new ObservableCollection<BillStatistic>();
            SeriesCollection = new SeriesCollection();
            var profitValue = new ChartValues<float>();
            var RevenueValue = new ChartValues<float>();
            var fundsValue = new ChartValues<float>();
            List<string> _Labels = new List<string>();
            var billList = DataProvider.Ins.DB.Bills;

            int i = 1;
            foreach (var item in billList)
            {
                float importPrice = 0;
                float outPrice = 0;
                var outputList = DataProvider.Ins.DB.BillInfoes.Where(p => p.IdBill == item.Id);

                if (outputList != null && outputList.Count() > 0)
                {
                    outPrice = (float)outputList.Sum(p => p.RetailPrice);
                    importPrice = (float)outputList.Sum(p => p.ImportInfo.InputPrice);
                }

                BillStatistic billStatistic = new BillStatistic();
                billStatistic.bill = item;
                billStatistic.Ordinal = i;
                billStatistic.Revenue = outPrice;
                billStatistic.Funds = importPrice;
                billStatistic.Profit = outPrice - importPrice;
                BillStatisticList.Add(billStatistic);
                i++;
                //tong
                SumRevenue += outPrice;
                SumFunds += importPrice;
                profitValue.Add(outPrice - importPrice);
                RevenueValue.Add(outPrice);
                fundsValue.Add(importPrice);
                _Labels.Add(item.DateCreate.ToString());
            }
            SumProfit = SumRevenue - SumFunds;
            Labels = _Labels.ToArray();
            SeriesCollection.Add(new LineSeries
            {

                Title = "Lợi nhuận",
                Values = profitValue
            });
            SeriesCollection.Add(new LineSeries
            {

                Title = "Doanh thu",
                Values = RevenueValue
            });
            SeriesCollection.Add(new LineSeries
            {

                Title = "Vốn",
                Values = fundsValue
            });
        }

        void LoadBillDataByDate()
        {
            
            BillStatisticList = new ObservableCollection<BillStatistic>();

            var billList = DataProvider.Ins.DB.Bills.Where(p=>p.DateCreate<=DateEnd&& p.DateCreate >= DateBegin);
            
            int i = 1;
            foreach (var item in billList)
            {
                float importPrice = 0;
                float outPrice = 0;
                var outputList = DataProvider.Ins.DB.BillInfoes.Where(p => p.IdBill == item.Id);//danh sách đã bán

                

                if (outputList != null && outputList.Count() > 0)
                {
                    outPrice = (float)outputList.Sum(p => p.RetailPrice);
                    importPrice = (float)outputList.Sum(p => p.ImportInfo.InputPrice);
                }

                BillStatistic billStatistic = new BillStatistic();
                billStatistic.bill = item;
                billStatistic.Ordinal = i;
                billStatistic.Revenue = outPrice;
                billStatistic.Funds = importPrice;
                billStatistic.Profit = outPrice - importPrice;
                BillStatisticList.Add(billStatistic);
                i++;
               
                //tong
               

            }

        }

        void LoadDataByDate()
        {

            SumFunds = 0;
            SumProfit = 0;
            SumRevenue = 0;

            SeriesCollection = new SeriesCollection();
            List<string> _Labels = new List<string>();
            var profitValue = new ChartValues<float>();
            var RevenueValue = new ChartValues<float>();
            var fundsValue = new ChartValues<float>();
            int i = 1;

            BillByDateList = new ObservableCollection<BillByDate>();
            foreach (var item in BillStatisticList)
            {
                if (BillByDateList.Where(p => p.DateCreate == item.bill.DateCreate).Count()==0)
                {
                    var bill = new BillByDate() { DateCreate = item.bill.DateCreate };
                    var k = BillStatisticList.Where(p => p.bill.DateCreate == item.bill.DateCreate);
                    
                    if(k!=null&&k.Count()!=0)
                    {
                        bill.Ordinal = i;
                        bill.SumFunds = k.Sum(p => p.Funds);
                        bill.SumProfit = k.Sum(p => p.Profit);
                        bill.SumRevenue = k.Sum(p => p.Revenue);
                        SumFunds +=bill.SumFunds;
                        SumProfit +=bill.SumProfit;
                        SumRevenue += bill.SumRevenue;

                        profitValue.Add(bill.SumProfit);
                        RevenueValue.Add(bill.SumRevenue);
                        fundsValue.Add(bill.SumFunds);
                        _Labels.Add(String.Format("{0:MM/dd/yyyy}", bill.DateCreate));
                    }
                    BillByDateList.Add(bill);
                    
                }
                i++;
            }
            Labels = _Labels.ToArray();

            SeriesCollection.Add(new LineSeries
            {

                Title = "Lợi nhuận",
                Values = profitValue
            });
            SeriesCollection.Add(new LineSeries
            {

                Title = "Doanh thu",
                Values = RevenueValue
            });
            SeriesCollection.Add(new LineSeries
            {

                Title = "Vốn",
                Values = fundsValue
            });

        }

        public StatisticalViewModel()
        {
            LoadBillDataByDate();
            LoadDataByDate();
            YFormatter = value => value.ToString("C");
            ShowCommand = new RelayCommand<object>((p) =>
            {
                    //điểu kiện chạy button add
                    if (DateBegin == null || DateEnd == null)
                        return false;

                    return true;

            }, (p) =>
            {
                LoadBillDataByDate();
                LoadDataByDate();
                OnPropertyChanged("BillByDateList");
            });

        }

    }
}
