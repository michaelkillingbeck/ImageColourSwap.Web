namespace Web.Models;

public class ResultsModel
{
    public string Id { get; set; }
    public string OutputImage { get; set; }
    public string PalletteImage { get; set; }
    public string SourceImage { get; set; }

    public ResultsModel()
    {
        Id = OutputImage = PalletteImage = SourceImage = String.Empty;
    }
}