using Web.Models;

namespace Web.Interfaces;

public interface IGalleryResultsService
{
    Task<PagedResultsModel> GetPage(PagedSearchRequest request);
}