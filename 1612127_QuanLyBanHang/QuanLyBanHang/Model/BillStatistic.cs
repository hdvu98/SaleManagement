using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanHang.Model
{
    class BillStatistic
    {
        public Bill bill { get; set; }
        public int Ordinal { get; set; }//stt
        public float Funds { get; set; }
        public float Revenue { get; set; }//thu
        public float Profit { get; set; }//lợi nhận
    }
}
