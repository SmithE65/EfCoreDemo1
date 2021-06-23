using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace EfCoreDemo1.Models.Entities
{
    [Table("ProductCategory", Schema = "SalesLT")]
    [Index(nameof(Name), Name = "AK_ProductCategory_Name", IsUnique = true)]
    [Index(nameof(Rowguid), Name = "AK_ProductCategory_rowguid", IsUnique = true)]
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            InverseParentProductCategory = new HashSet<ProductCategory>();
            Products = new HashSet<Product>();
        }

        [Key]
        [Column("ProductCategoryID")]
        public int ProductCategoryId { get; set; }
        [Column("ParentProductCategoryID")]
        public int? ParentProductCategoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(ParentProductCategoryId))]
        [InverseProperty(nameof(ProductCategory.InverseParentProductCategory))]
        public virtual ProductCategory ParentProductCategory { get; set; }
        [InverseProperty(nameof(ProductCategory.ParentProductCategory))]
        public virtual ICollection<ProductCategory> InverseParentProductCategory { get; set; }
        [JsonIgnore]
        [InverseProperty(nameof(Product.ProductCategory))]
        public virtual ICollection<Product> Products { get; set; }
    }
}
