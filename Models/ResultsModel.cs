namespace Web.Models;

public class ResultsModel
{
    public string OutputImage { get; set; }
    public string PalletteImage { get; set; }
    public string SourceImage { get; set; }

    public ResultsModel()
    {
        OutputImage = PalletteImage = SourceImage = String.Empty;
    }
}