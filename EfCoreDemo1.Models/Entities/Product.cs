using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EfCoreDemo1.Models.Entities
{
    [Table("Product", Schema = "SalesLT")]
    [Index(nameof(Name), Name = "AK_Product_Name", IsUnique = true)]
    [Index(nameof(ProductNumber), Name = "AK_Product_ProductNumber", IsUnique = true)]
    [Index(nameof(Rowguid), Name = "AK_Product_rowguid", IsUnique = true)]
    public partial class Product
    {
        public Product()
        {
            SalesOrderDetails = new HashSet<SalesOrderDetail>();
        }

        [Key]
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(25)]
        public string ProductNumber { get; set; }
        [StringLength(15)]
        public string Color { get; set; }
        [Column(TypeName = "money")]
        public decimal StandardCost { get; set; }
        [Column(TypeName = "money")]
        public decimal ListPrice { get; set; }
        [StringLength(5)]
        public string Size { get; set; }
        [Column(TypeName = "decimal(8, 2)")]
        public decimal? Weight { get; set; }
        [Column("ProductCategoryID")]
        public int? ProductCategoryId { get; set; }
        [Column("ProductModelID")]
        public int? ProductModelId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime SellStartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SellEndDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DiscontinuedDate { get; set; }
        public byte[] ThumbNailPhoto { get; set; }
        [StringLength(50)]
        public string ThumbnailPhotoFileName { get; set; }
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey(nameof(ProductCategoryId))]
        [InverseProperty("Products")]
        public virtual ProductCategory ProductCategory { get; set; }
        [ForeignKey(nameof(ProductModelId))]
        [InverseProperty("Products")]
        public virtual ProductModel ProductModel { get; set; }
        [InverseProperty(nameof(SalesOrderDetail.Product))]
        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
    }
}
