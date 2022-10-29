using Web.Models;

namespace Web.Interfaces;

public interface IResultsModelMapper<T>
{
    ResultsModel Convert(T sourceModel);
    T ConvertFrom(ResultsModel resultsModel);
}