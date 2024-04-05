using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using ImageHelpers.Interfaces;
using ImageHelpers.Services.ImageSharp;
using Web.Helpers;
using Web.Interfaces;
using Web.Models;
using Web.Services;

namespace Web.Bootstrapping;

internal static class ServicesBootstrapping
{
    internal static void AddServices(WebApplicationBuilder builder)
    {
        _ = builder.Services.AddScoped<IImageHandler, ImageSharpImageHandler>();
        _ = builder.Services.AddScoped<IImageSaver, S3ImageSaver>();
        _ = builder.Services.AddScoped(provider =>
            {
                return new AmazonDynamoDBClient(RegionEndpoint.EUWest2);
            });
        _ = builder.Services.AddScoped<IResultsModelMapper<Document>, DynamoDocumentResultsModelMapper>();
        _ = builder.Services.AddScoped<IImageResultsRepository<ResultsModel>, DynamoDbResultsRepository>();
        _ = builder.Services.AddScoped<IImageResultsRepository<PagedResultsModel>, DynamoDbPagedResultsRepository>();
        _ = builder.Services.AddScoped<IUrlGenerator, S3UrlGenerator>();
        _ = builder.Services.AddScoped<IGalleryResultsService, GalleryResultsService>();
    }
}