using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using Web.Interfaces;
using Web.Models;

namespace Web.Services;

public class S3UrlGenerator(IOptions<SettingsModel> settings) : IUrlGenerator
{
    private readonly SettingsModel _settings = settings.Value;

    public string GetUrl(string objectName)
    {
        GetPreSignedUrlRequest request = new()
        {
            BucketName = _settings.BucketName,
            Expires = DateTime.UtcNow.AddMinutes(1),
            Key = objectName,
        };

        AmazonS3Client client = new(Amazon.RegionEndpoint.EUWest2);

        return client.GetPreSignedURL(request);
    }
}