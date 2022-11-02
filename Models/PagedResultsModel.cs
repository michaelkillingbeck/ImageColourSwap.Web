namespace Web.Models;

public class PagedResultsModel
{
    public bool NextPage { get; set; }
    public List<string> PageMarkers { get; set; }
    public bool PreviousPage => PageMarkers.Count > 1;
    public List<ResultsModel> Results { get; set; }

    public PagedResultsModel()
    {
        PageMarkers = new List<string>();
        Results = new List<ResultsModel>();
    }
}