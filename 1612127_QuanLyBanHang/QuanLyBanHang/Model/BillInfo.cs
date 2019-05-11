//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyBanHang.Model
{
    using QuanLyBanHang.ViewModel;
    using System;
    using System.Collections.Generic;
    
    public partial class BillInfo:BaseViewModel
    {
        private string _Id;
        public string Id { get=>_Id; set { _Id = value; OnPropertyChanged(); } }
        private string _IdObject;
        public string IdObject { get=>_IdObject; set {_IdObject=value;OnPropertyChanged(); } }
        private string _IdBill; 
        public string IdBill { get=>_IdBill; set {_IdBill=value; OnPropertyChanged(); } }
        private Nullable<int> _Count;
        public Nullable<int> Count { get=>_Count; set {_Count=value;OnPropertyChanged(); } }
        private Nullable<double> _RetailPrice;
        public Nullable<double> RetailPrice { get=>_RetailPrice; set {_RetailPrice=value; OnPropertyChanged(); } }
        private Nullable<double> _Sum; 
        public Nullable<double> Sum { get=>_Sum; set {_Sum=value; OnPropertyChanged(); } }
        private string _IdImportInfo;
        public string IdImportInfo { get=>_IdImportInfo; set {_IdImportInfo=value; OnPropertyChanged(); } }
    
        public virtual Bill Bill { get; set; }
        public virtual ImportInfo ImportInfo { get; set; }
        public virtual Object Object { get; set; }
    }
}