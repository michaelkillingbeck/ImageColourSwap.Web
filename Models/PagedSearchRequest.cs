namespace Web.Models;

public class PagedSearchRequest
{
    public List<string> PageMarkers { get; set; }

    public PagedSearchRequest()
    {
        PageMarkers = new List<string>();
    }
}