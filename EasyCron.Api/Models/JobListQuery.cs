namespace EasyCron.Api.Models
{
    public class JobListQuery
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string Keywords { get; set; }
    }
}
