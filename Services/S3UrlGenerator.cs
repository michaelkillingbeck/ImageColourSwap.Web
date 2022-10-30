using Amazon.S3;
using Amazon.S3.Model;
using Web.Interfaces;

namespace Web.Services;

public class S3UrlGenerator : IUrlGenerator
{
    private readonly string _bucketName;

    public S3UrlGenerator(IConfiguration configuration)
    {
        _bucketName = configuration["Settings:BucketName"];
    }

    public string GetUrl(string objectName)
    {
        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Expires = DateTime.UtcNow.AddMinutes(1),
            Key = objectName
        };

        AmazonS3Client client = new AmazonS3Client(Amazon.RegionEndpoint.EUWest2);
        
        return client.GetPreSignedURL(request);
    }
}