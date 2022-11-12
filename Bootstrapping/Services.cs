using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Image_Colour_Swap.Interfaces;
using Web.Helpers;
using Web.Interfaces;
using Web.Models;
using Web.Services;

namespace Web.Bootstrapping;

internal class ServicesBootstrapping
{
    internal static void AddServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IImageLoader, ImageSharpImageLoader>();
        builder.Services.AddScoped<IImageSaver, S3ImageSaver>();
        builder.Services.AddScoped<AmazonDynamoDBClient>(provider => {
            return new AmazonDynamoDBClient(RegionEndpoint.EUWest2);
        });
        builder.Services.AddScoped<IResultsModelMapper<Document>, DynamoDocumentResultsModelMapper>();
        builder.Services.AddScoped<IImageResultsRepository<ResultsModel>, DynamoDbResultsRepository>();
        builder.Services.AddScoped<IImageResultsRepository<PagedResultsModel>, DynamoDbPagedResultsRepository>();
        builder.Services.AddScoped<IUrlGenerator, S3UrlGenerator>();
        builder.Services.AddScoped<IGalleryResultsService, GalleryResultsService>();
    }
}