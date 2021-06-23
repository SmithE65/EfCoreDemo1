using EfCoreDemo1.Dal;
using EfCoreDemo1.Models.Extensions;
using EfCoreDemo1.Models.Projections;
using EfCoreDemo1.Models.ViewModels;
using EfCoreDemo1.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace EfCoreDemo1.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly AdventureWorksDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AdventureWorksDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("Something went wrong.", null);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        ///////////////////////////////////////////////////////////////////////
        /// Demo Methods
        ///////////////////////////////////////////////////////////////////////

        [HttpGet]
        public IActionResult FirstOrDefault()
        {
            var data = _context.Products.FirstOrDefault(x => x.ProductId == 680);

            var data2 = (from product in _context.Products
                        where product.ProductId == 680
                        select product)
                        .FirstOrDefault();

            return View("Products", data);
        }

        [HttpGet]
        public IActionResult FromSql()
        {
            var data = _context.ProductCategories
                .FromSqlRaw(@"SELECT * FROM SalesLT.ProductCategory");

            return Ok(data);
        }

        [HttpGet]
        public IActionResult GroupBy()
        {
            var popularColors = _context.SalesOrderDetails
                .GroupBy(x => x.Product.Color)
                .Select(x => new { Color = x.Key, UnitsSold = x.Sum(s => s.OrderQty) })
                .OrderByDescending(x => x.UnitsSold);

            var popularModels = _context.SalesOrderDetails
                .Select(x => new { x.Product.Color, x.Product.ProductModelId, Bob = _context.Addresses.FirstOrDefault() })
                .Distinct()
                .GroupBy(x => x.Color)
                .Select(x => new { Color = x.Key, ModelsSold = x.Count() })
                .OrderByDescending(x => x.ModelsSold);

            var data = popularColors.Join(popularModels, o => o.Color, i => i.Color, (colorUnitsSold, colorModelsSold) => new { colorUnitsSold.Color, colorUnitsSold.UnitsSold, colorModelsSold.ModelsSold });

            //var popularColors = from sod in _context.SalesOrderDetails
            //                    group sod by sod.Product.Color into colors
            //                    select new { Color = colors.Key, UnitsSold = colors.Sum(c => c.OrderQty) };

            //var popularModels = from modelColors in _context.SalesOrderDetails.Select(x => new { x.Product.Color, x.Product.ProductModelId }).Distinct()
            //                    group modelColors by modelColors.Color into colors
            //                    select new { Color = colors.Key, ModelsSold = colors.Count() };

            //var data = from popColor in popularColors
            //           join popModel in popularModels on popColor.Color equals popModel.Color
            //           select new { popColor.Color, popColor.UnitsSold, popModel.ModelsSold };

            return Ok(data);
        }

        [HttpGet]
        public IActionResult GroupJoin()
        {
            var data = _context.Products
                .GroupJoin(
                    inner: _context.ProductModelProductDescriptions,
                    outerKeySelector: o => o.ProductModelId,
                    innerKeySelector: i => i.ProductModelId,
                    resultSelector: (o, i) => new { Product = o, ProductModelProductDescription = i })
                .SelectMany(x => x.ProductModelProductDescription.DefaultIfEmpty(), (productGrouping, pmpd) => new ProductDescriptionViewModel
                {
                    ProductName = productGrouping.Product.Name,
                    Description = pmpd.ProductDescription.Description
                });

            var data2 = from product in _context.Products
                       join pmpd in _context.ProductModelProductDescriptions
                        on product.ProductModelId equals pmpd.ProductModelId
                        into productDescriptions
                       from pmpd in productDescriptions.DefaultIfEmpty()
                       select new ProductDescriptionViewModel
                       {
                           ProductName = product.Name,
                           Description = pmpd.ProductDescription.Description
                       };

            //var data = _context.Products
            //    .LeftJoin(inner: _context.ProductModelProductDescriptions,
            //        outerKeySelector: o => o.ProductModelId,
            //        innerKeySelector: i => i.ProductModelId,
            //        resultSelector: (grouping, pmpd) => new ProductDescriptionViewModel
            //        {
            //            ProductName = grouping.Outer.Name,
            //            Description = pmpd.ProductDescription.Description
            //        });

            return Ok(data.Take(5));
        }

        [HttpGet]
        public IActionResult Join()
        {
            var data = _context.Products
                .Join(_context.ProductModelProductDescriptions,
                    o => o.ProductModelId,
                    i => i.ProductModelId,
                    (o, i) => new { Product = o, Description = i })
                .Where(x => x.Description.Culture.Contains("en"))
                .Select(x => new ProductDescriptionViewModel
                {
                    ProductName = x.Product.Name,
                    Description = x.Description.ProductDescription.Description
                })
                .Take(5);

            //var data = (from product in _context.Products
            //            join pmpd in _context.ProductModelProductDescriptions on product.ProductModelId equals pmpd.ProductModelId
            //            where pmpd.Culture.Contains("en")
            //            select new ProductDescriptionViewModel
            //            {
            //                ProductName = product.Name,
            //                Description = pmpd.ProductDescription.Description
            //            })
            //           .Take(5);

            return Ok(new { Result = data, Query = data.ToQueryString() });
        }

        [HttpGet]
        public IActionResult Include()
        {
            var data = _context.Products
                .Include(x => x.ProductCategory)
                .FirstOrDefault();

            return Ok(new
            {
                Result = data
            });
        }

        [HttpGet]
        public IActionResult MultipleIncludes()
        {
            var product = _context.Products
                .Include(x => x.ProductCategory)
                .Include(x => x.ProductModel.ProductModelProductDescriptions)
                    .ThenInclude(x => x.ProductDescription)
                .FirstOrDefault();

            return Ok(product);
        }

        [HttpGet]
        public IActionResult MultiTableProjection()
        {
            var data = _context.Products
                .Select(x => new ProductCategoryViewModel
                {
                    ProductName = x.Name,
                    CategoryName = x.ProductCategory.Name
                })
                .FirstOrDefault();

            return Ok(data);
        }

        [HttpGet]
        public IActionResult SimpleProjection()
        {
            var data = _context.Products
                .Where(x => x.ProductId == 680)
                .Select(x => new ProductViewModel
                {
                    ProductName = x.Name,
                    ProductNumber = x.ProductNumber,
                    ListPrice = x.ListPrice
                });

            var data2 = from product in _context.Products
                        where product.ProductId == 680
                        select new ProductViewModel
                        {
                            ProductName = product.Name,
                            ProductNumber = product.ProductNumber,
                            ListPrice = product.ListPrice
                        };

            return Ok(new
            {
                MethodData = data.ToList(),
                QueryData = data2.ToList(),
                MethodQuery = data.ToQueryString(),
                QueryQuery = data2.ToQueryString()
            });
        }

        [HttpGet]
        public IActionResult SelectStar()
        {
            var data = _context.ProductCategories;

            var data = from productCategory in _context.ProductCategories
                       select productCategory;

            ViewData["Query"] = data.ToQueryString();

            return View("ProductCategories", data);
        }

        [HttpGet]
        public IActionResult TopTenProducts()
        {
            var top10 = _context.Products
                .OrderByDescending(x => x.ListPrice)
                .Take(10);

            //var sortedProducts = from p in _context.Products
            //                     orderby p.ListPrice descending
            //                     select p;
            //var top10 = sortedProducts.Take(10).AsNoTracking();

            ViewData["Query"] = top10.ToQueryString();

            return View("Products", top10);
        }

        [HttpGet]
        public IActionResult TopTenProductsByValue()
        {
            var vm = _context.SalesOrderDetails
                .GroupBy(x => new
                {
                    x.ProductId,
                    x.Product.Name,
                    x.Product.ProductModelId,
                    x.Product.ListPrice
                })
                .Select(x => new TopProductViewModel
                {
                    ProductName = x.Key.Name,
                    Description = _context.ProductModelProductDescriptions.FirstOrDefault(pmpd => pmpd.ProductModelId == x.Key.ProductModelId && pmpd.Culture == "en").ProductDescription.Description,
                    ListPrice = x.Key.ListPrice,
                    TotalNumberSold = x.Sum(y => y.OrderQty),
                    TotalValueSold = x.Sum(y => y.LineTotal)
                })
                .OrderByDescending(x => x.TotalValueSold)
                .Where(x => x.ListPrice < 1000)
                .Take(10);

            ViewData["Query"] = vm.ToQueryString();

            return View("TopTenProducts", vm);
        }

        [HttpGet]
        public IActionResult Where()
        {
            var data = _context.ProductCategories
                .Where(x => x.ParentProductCategoryId == null);

            //var data = from category in _context.ProductCategories
            //           where category.ParentProductCategoryId == null
            //           select category;

            ViewData["Query"] = data.ToQueryString();

            return View("ProductCategories", data);
        }
    }
}
