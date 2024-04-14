using ImageHelpers.Interfaces;
using Web.Interfaces;
using Web.Models;

namespace Web.Services;

public class GalleryResultsService(
    IImageResultsRepository<PagedResultsModel> imageResultsRepository,
    IUrlGenerator urlGenerator) : IGalleryResultsService
{
    private readonly IImageResultsRepository<PagedResultsModel> _imageResultsRepository = imageResultsRepository;
    private readonly IUrlGenerator _urlGenerator = urlGenerator;

    public async Task<PagedResultsModel> GetPage(PagedSearchRequest request)
    {
        string nextPage = string.Empty;

        if (!request.IsBackwards && request.PageMarkers.Count != 0)
        {
            nextPage = request.PageMarkers[^1];
        }
        else
        {
            if (request.PageMarkers.Count > 2)
            {
                nextPage = request.PageMarkers[^3];
            }
        }

        PagedResultsModel results = await _imageResultsRepository.LoadResults(nextPage);

        if (request.IsBackwards)
        {
            for (int i = 2; i > 0; i--)
            {
                if (request.PageMarkers.Count >= i)
                {
                    request.PageMarkers.RemoveAt(request.PageMarkers.Count - 1);
                }
            }
        }

        request.PageMarkers.AddRange(results.PageMarkers);
        results.PageMarkers = request.PageMarkers;

        if (results.Results.ToList().Count > 0)
        {
            foreach (ResultsModel result in results.Results)
            {
                result.OutputImage = _urlGenerator.GetUrl(result.OutputImage);
                result.PalletteImage = _urlGenerator.GetUrl(result.PalletteImage);
                result.SourceImage = _urlGenerator.GetUrl(result.SourceImage);
            }

            return results;
        }

        return new PagedResultsModel();
    }
}