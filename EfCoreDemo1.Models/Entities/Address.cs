using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EfCoreDemo1.Models.Entities
{
    [Table("Address", Schema = "SalesLT")]
    [Index(nameof(Rowguid), Name = "AK_Address_rowguid", IsUnique = true)]
    [Index(nameof(AddressLine1), nameof(AddressLine2), nameof(City), nameof(StateProvince), nameof(PostalCode), nameof(CountryRegion), Name = "IX_Address_AddressLine1_AddressLine2_City_StateProvince_PostalCode_CountryRegion")]
    [Index(nameof(StateProvince), Name = "IX_Address_StateProvince")]
    public partial class Address
    {
        public Address()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            SalesOrderHeaderBillToAddresses = new HashSet<SalesOrderHeader>();
            SalesOrderHeaderShipToAddresses = new HashSet<SalesOrderHeader>();
        }

        [Key]
        [Column("AddressID")]
        public int AddressId { get; set; }
        [Required]
        [StringLength(60)]
        public string AddressLine1 { get; set; }
        [StringLength(60)]
        public string AddressLine2 { get; set; }
        [Required]
        [StringLength(30)]
        public string City { get; set; }
        [Required]
        [StringLength(50)]
        public string StateProvince { get; set; }
        [Required]
        [StringLength(50)]
        public string CountryRegion { get; set; }
        [Required]
        [StringLength(15)]
        public string PostalCode { get; set; }
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [InverseProperty(nameof(CustomerAddress.Address))]
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        [InverseProperty(nameof(SalesOrderHeader.BillToAddress))]
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaderBillToAddresses { get; set; }
        [InverseProperty(nameof(SalesOrderHeader.ShipToAddress))]
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaderShipToAddresses { get; set; }
    }
}
