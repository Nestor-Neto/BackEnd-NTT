
namespace Ambev.DeveloperEvaluation.Application.Sales.Get;
public class GetSalesQuery
    {
        public int Page { get; set; }
        public int Size { get; set; }

        public GetSalesQuery(int page, int size)
        {
            Page = page;
            Size = size;
        }
    }