using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Web.Interfaces;
using Web.Models;

namespace Web.Services;

public class S3UrlGenerator : IUrlGenerator
{
    private readonly SettingsModel _settings;

    public S3UrlGenerator(IConfiguration configuration, IOptions<SettingsModel> settings)
    {
        _settings = settings.Value;
    }

    public string GetUrl(string objectName)
    {
        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
        {
            BucketName = _settings.BucketName,
            Expires = DateTime.UtcNow.AddMinutes(1),
            Key = objectName
        };

        AmazonS3Client client = new AmazonS3Client(Amazon.RegionEndpoint.EUWest2);
        
        return client.GetPreSignedURL(request);
    }
}