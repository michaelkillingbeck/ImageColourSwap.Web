using Web.Models;

namespace Web.Interfaces;

public interface IResultsModelMapper<T>
{
    IEnumerable<ResultsModel> Convert(IEnumerable<T> sourceCollection);

    ResultsModel Convert(T sourceModel);

    T ConvertFrom(ResultsModel resultsModel);
}