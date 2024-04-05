using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using ImageHelpers.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using Web.Models;

namespace Web.Services;

public class S3ImageSaver(IOptions<SettingsModel> settings) : IImageSaver, IDisposable
{
    private readonly AmazonS3Client _client = new(RegionEndpoint.EUWest2);
    private bool _isDisposed;
    private readonly SettingsModel _settings = settings.Value;

    public async Task<bool> SaveAsync(string filename, Stream imageStream)
    {
        try
        {
            PutObjectRequest putRequest = new()
            {
                BucketName = _settings.BucketName,
                ContentType = "text/plain",
                InputStream = imageStream,
                Key = filename,
            };

            PutObjectResponse putResponse = await _client.PutObjectAsync(putRequest);

            return putResponse.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error saving {filename}");
            Console.Error.WriteLine(ex.Message);

            return false;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            _client.Dispose();
        }

        _isDisposed = true;
    }
}