namespace TravelAndAccommodationBookingPlatform.Domain.Models.Common;

public class PageData
{
    public int TotalItemCount { get; set; }
    public int TotalPageCount { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }

    public PageData(int totalItemCount, int pageSize, int currentPage)
    {
        TotalItemCount = totalItemCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPageCount = pageSize == 0 ? 0 : (int)Math.Ceiling(totalItemCount / (double)pageSize);
    }
}