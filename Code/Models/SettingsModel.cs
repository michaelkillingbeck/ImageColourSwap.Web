namespace Web.Models;

public class SettingsModel
{
    public string BucketName { get; set; }

    public string ProcessingUri { get; set; }

    public SettingsModel()
    {
        BucketName = ProcessingUri = string.Empty;
    }
}