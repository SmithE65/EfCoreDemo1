using EfCoreDemo1.Models.Entities;
using EfCoreDemo1.Models.ViewModels;
using System.Linq;

namespace EfCoreDemo1.Models.Projections
{
    public static class ProductProjections
    {
        public static IQueryable<ProductViewModel> ProjectToProductViewModel(this IQueryable<Product> products)
            => products.Select(x => new ProductViewModel
            {
                ProductName = x.Name,
                ProductNumber = x.ProductNumber,
                ListPrice = x.ListPrice
            });
    }
}
