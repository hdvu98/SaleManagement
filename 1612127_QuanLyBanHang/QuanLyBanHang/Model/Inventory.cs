using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanHang.Model
{
    class Inventory
    {
        public Object Product { get; set; }
        public int Ordinal{ get; set; }
        public int Quantity { get; set; }
        public ImportInfo info { get; set; }
    }
}
