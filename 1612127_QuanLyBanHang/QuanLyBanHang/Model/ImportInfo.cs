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
    using System;
    using System.Collections.Generic;
    
    public partial class ImportInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ImportInfo()
        {
            this.BillInfoes = new HashSet<BillInfo>();
        }
    
        public string Id { get; set; }
        public string IdObject { get; set; }
        public string IdImport { get; set; }
        public int Quantity { get; set; }
        public Nullable<double> InputPrice { get; set; }
        public Nullable<double> RetailPrice { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BillInfo> BillInfoes { get; set; }
        public virtual Import Import { get; set; }
        public virtual Object Object { get; set; }
    }
}
