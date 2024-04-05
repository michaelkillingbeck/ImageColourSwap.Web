namespace Web.Models;

public class ResultsModel
{
    public const string Id = "ImageColourSwap";

    public string ResultsId { get; set; }

    public string OutputImage { get; set; }

    public string PalletteImage { get; set; }

    public string SourceImage { get; set; }

    public ResultsModel()
    {
        ResultsId = OutputImage = PalletteImage = SourceImage = string.Empty;
    }
}