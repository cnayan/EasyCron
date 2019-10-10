namespace EasyCron.Api.Models
{
    public class JobListQuery
    {
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public string Keywords { get; set; }
    }
}
