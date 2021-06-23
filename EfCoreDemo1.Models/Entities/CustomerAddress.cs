using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EfCoreDemo1.Models.Entities
{
    [Table("CustomerAddress", Schema = "SalesLT")]
    [Index(nameof(Rowguid), Name = "AK_CustomerAddress_rowguid", IsUnique = true)]
    public partial class CustomerAddress
    {
        [Key]
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Key]
        [Column("AddressID")]
        public int AddressId { get; set; }
        [Required]
        [StringLength(50)]
        public string AddressType { get; set; }
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey(nameof(AddressId))]
        [InverseProperty("CustomerAddresses")]
        public virtual Address Address { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("CustomerAddresses")]
        public virtual Customer Customer { get; set; }
    }
}
