using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCoreDemo1.Models.ViewModels
{
    public class TopProductViewModel
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal ListPrice { get; set; }
        public decimal TotalValueSold { get; set; }
        public int TotalNumberSold { get; set; }
    }
}
