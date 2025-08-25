using SystoQ.Domain.Common.Enums;
using SystoQ.Domain.Common.Pagination;

namespace SystoQ.Domain.Filters.Products
{
    public class ProductsPriceFilter : QueryStringParameters
    {
        public decimal? Price { get; set; }
        public PriceCriteria? Criteria { get; set; }
    }
}
