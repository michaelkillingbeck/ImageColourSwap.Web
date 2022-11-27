using ImageHelpers.Interfaces;
using Web.Interfaces;
using Web.Models;

namespace Web.Services;

public class GalleryResultsService : IGalleryResultsService
{
    private readonly IImageResultsRepository<PagedResultsModel> _imageResultsRepository;
    private readonly IUrlGenerator _urlGenerator;

    public GalleryResultsService(
        IImageResultsRepository<PagedResultsModel> imageResultsRepository,
        IUrlGenerator urlGenerator)
    {
        _imageResultsRepository = imageResultsRepository;
        _urlGenerator = urlGenerator;
    }

    public async Task<PagedResultsModel> GetPage(PagedSearchRequest searchRequest)
    {
        string nextPage = String.Empty;

        if(searchRequest.IsBackwards == false && searchRequest.PageMarkers.Any())
        {
            nextPage = searchRequest.PageMarkers.Last();
        }
        else
        {
            if(searchRequest.PageMarkers.Count > 2)
            {
                nextPage = searchRequest.PageMarkers[searchRequest.PageMarkers.Count - 3];
            }
        }

        var results = await _imageResultsRepository.LoadResults(nextPage);

        if(searchRequest.IsBackwards == true)
        {
            for(int i = 2; i > 0; i--)
            {
                if(searchRequest.PageMarkers.Count >= i)
                {
                    searchRequest.PageMarkers.RemoveAt(searchRequest.PageMarkers.Count - 1);
                }
            }
        }

        searchRequest.PageMarkers.AddRange(results.PageMarkers);
        results.PageMarkers = searchRequest.PageMarkers;

        if(results.Results.ToList().Count > 0)
        {
            foreach (var result in results.Results)
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