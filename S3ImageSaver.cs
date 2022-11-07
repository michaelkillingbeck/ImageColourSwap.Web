using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Image_Colour_Swap.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using Web.Models;

namespace Web;

public class S3ImageSaver : IImageSaver
{
    private readonly AmazonS3Client _client;
    private readonly SettingsModel _settings;

    public S3ImageSaver(IOptions<SettingsModel> settings)
    {
        _client = new AmazonS3Client(RegionEndpoint.EUWest2);
        _settings = settings.Value;
    }

    public async Task<bool> SaveAsync(string filename, Stream imageStream)
    {
        try
        {
            var putRequest = new PutObjectRequest
            {
                BucketName = _settings.BucketName,
                ContentType = "text/plain",
                InputStream = imageStream,
                Key = filename
            };

            var putResponse = await _client.PutObjectAsync(putRequest);

            return putResponse.HttpStatusCode == HttpStatusCode.OK;
        }
        catch(Exception ex)
        {
            Console.Error.WriteLine($"Error saving {filename}");
            Console.Error.WriteLine(ex.Message);

            return false;            
        }
    }
}