using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EfCoreDemo1.Models.Entities
{
    [Table("SalesOrderHeader", Schema = "SalesLT")]
    [Index(nameof(SalesOrderNumber), Name = "AK_SalesOrderHeader_SalesOrderNumber", IsUnique = true)]
    [Index(nameof(Rowguid), Name = "AK_SalesOrderHeader_rowguid", IsUnique = true)]
    [Index(nameof(CustomerId), Name = "IX_SalesOrderHeader_CustomerID")]
    public partial class SalesOrderHeader
    {
        public SalesOrderHeader()
        {
            SalesOrderDetails = new HashSet<SalesOrderDetail>();
        }

        [Key]
        [Column("SalesOrderID")]
        public int SalesOrderId { get; set; }
        public byte RevisionNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DueDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ShipDate { get; set; }
        public byte Status { get; set; }
        [Required]
        public bool? OnlineOrderFlag { get; set; }
        [Required]
        [StringLength(25)]
        public string SalesOrderNumber { get; set; }
        [StringLength(25)]
        public string PurchaseOrderNumber { get; set; }
        [StringLength(15)]
        public string AccountNumber { get; set; }
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Column("ShipToAddressID")]
        public int? ShipToAddressId { get; set; }
        [Column("BillToAddressID")]
        public int? BillToAddressId { get; set; }
        [Required]
        [StringLength(50)]
        public string ShipMethod { get; set; }
        [StringLength(15)]
        public string CreditCardApprovalCode { get; set; }
        [Column(TypeName = "money")]
        public decimal SubTotal { get; set; }
        [Column(TypeName = "money")]
        public decimal TaxAmt { get; set; }
        [Column(TypeName = "money")]
        public decimal Freight { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalDue { get; set; }
        public string Comment { get; set; }
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey(nameof(BillToAddressId))]
        [InverseProperty(nameof(Address.SalesOrderHeaderBillToAddresses))]
        public virtual Address BillToAddress { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("SalesOrderHeaders")]
        public virtual Customer Customer { get; set; }
        [ForeignKey(nameof(ShipToAddressId))]
        [InverseProperty(nameof(Address.SalesOrderHeaderShipToAddresses))]
        public virtual Address ShipToAddress { get; set; }
        [InverseProperty(nameof(SalesOrderDetail.SalesOrder))]
        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
    }
}
