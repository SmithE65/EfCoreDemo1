using EfCoreDemo1.Models.Entities;
using EfCoreDemo1.Models.ViewModels;
using System.Linq;

namespace EfCoreDemo1.Models.Projections
{
    public static class SalesOrderProjections
    {
        public static IQueryable<TopProductViewModel> ProjectToTopProductViewModels(this IQueryable<SalesOrderDetail> details, IQueryable<ProductModelProductDescription> productModelProductDescriptions)
            => details.GroupBy(x => new
            {
                x.ProductId,
                x.Product.Name,
                x.Product.ProductModelId,
                x.Product.ListPrice
            })
            .Select(x => new TopProductViewModel
            {
                ProductName = x.Key.Name,
                Description = productModelProductDescriptions.FirstOrDefault(pmpd => pmpd.ProductModelId == x.Key.ProductModelId && pmpd.Culture == "en").ProductDescription.Description,
                ListPrice = x.Key.ListPrice,
                TotalNumberSold = x.Sum(y => y.OrderQty),
                TotalValueSold = x.Sum(y => y.LineTotal)
            });
    }
}
