namespace Web.Models;

public class PagedSearchRequest
{
    public bool IsBackwards { get; set; }

    public List<string> PageMarkers { get; set; }

    public PagedSearchRequest()
    {
        IsBackwards = false;
        PageMarkers = [];
    }
}