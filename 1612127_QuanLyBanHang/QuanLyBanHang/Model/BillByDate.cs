using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanHang.Model
{
    public class BillByDate
    {
        public int Ordinal { get; set; }
        public DateTime? DateCreate { get; set; }
        public float SumRevenue { get; set; }
        public float SumFunds { get; set; }
        public  float SumProfit { get; set; }
    }
}
