namespace EasyCron.Api.Models
{
    public class JobLogListQuery
    {
        public string JobId { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public string Keywords { get; set; }
        public bool? IsSuccess { get; set; }
    }
}
