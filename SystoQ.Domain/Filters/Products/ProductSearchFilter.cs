using SystoQ.Domain.Common.Pagination;

namespace SystoQ.Domain.Filters.Products
{
    public class ProductSearchFilter : QueryStringParameters
    {
        public string? Name { get; set; }
    }
}
