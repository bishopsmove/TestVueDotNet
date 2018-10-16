using System.Collections.Generic;

namespace TestVueDotNet.Contract
{
    public interface ISearchResult<TOut>
    {
        IEnumerable<TOut> Items { get; }
        int Page { get; }
        int PageSize { get; }
        long Total { get; }
    }
}
